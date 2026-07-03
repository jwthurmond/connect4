using System.Reflection;
using System.Runtime.Loader;
using connect4.tournament;

namespace connect4.runtournament;

public class PlayerPluginLoadContext(string pluginPath) : AssemblyLoadContext()
{
    private readonly AssemblyDependencyResolver _resolver = new(pluginPath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // Let the host resolve connect4.tournament and connect4.library so type identity matches
        var hostAssembly = Default.Assemblies
            .FirstOrDefault(a => a.GetName().Name == assemblyName.Name);
        if (hostAssembly is not null)
            return hostAssembly;

        var resolved = _resolver.ResolveAssemblyToPath(assemblyName);
        return resolved is not null ? LoadFromAssemblyPath(resolved) : null;
    }
}

public static class PlayerLoader
{
    public static IEnumerable<IConnect4Player> LoadFromDirectory(string pluginDirectory)
    {
        if (!Directory.Exists(pluginDirectory))
        {
            Console.WriteLine($"Plugin directory not found, skipping: {pluginDirectory}");
            yield break;
        }

        foreach (var dll in Directory.GetFiles(pluginDirectory, "*.dll"))
        {
            foreach (var player in LoadFromAssembly(dll))
                yield return player;
        }
    }

    private static IEnumerable<IConnect4Player> LoadFromAssembly(string dllPath)
    {
        var context = new PlayerPluginLoadContext(dllPath);
        Assembly assembly;
        try
        {
            assembly = context.LoadFromAssemblyPath(dllPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load {Path.GetFileName(dllPath)}: {ex.Message}");
            yield break;
        }

        var playerType = typeof(IConnect4Player);
        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsAbstract && !type.IsInterface && playerType.IsAssignableFrom(type))
            {
                IConnect4Player? instance = null;
                try
                {
                    instance = (IConnect4Player?)Activator.CreateInstance(type);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to instantiate {type.Name} from {Path.GetFileName(dllPath)}: {ex.Message}");
                }

                if (instance is not null)
                {
                    Console.WriteLine($"Loaded player '{instance.Name}' from {Path.GetFileName(dllPath)}");
                    yield return instance;
                }
            }
        }
    }
}
