# connect4

## Projects

- **connect4** — Console app to play a single game
- **connect4.runtournament** — Console app to run a round-robin tournament between all registered players
- **connect4.library** — Game board library (shared)
- **connect4.tournament** — Tournament engine and `IConnect4Player` interface
- **connect4.tests** — Unit tests
- **connect4.exampleplayer** — Example custom player plugin (see below)

## Running a tournament

```
dotnet run --project connect4.runtournament
```

By default the runner looks for a `plugins/` folder next to the executable and loads any player DLLs found there. You can pass a custom path as the first argument:

```
dotnet run --project connect4.runtournament -- /path/to/plugins
```

## Creating a custom player plugin

1. Create a new class library project targeting `net10.0`.

2. Add a reference to `connect4.tournament.dll` (or the project) with `ExcludeAssets="runtime"` so the host's copies of `connect4.tournament` and `connect4.library` are used at runtime — this is required for the type system to recognise your player:

   ```xml
   <ItemGroup>
     <ProjectReference Include="..\connect4.tournament\connect4.tournament.csproj">
       <ExcludeAssets>runtime</ExcludeAssets>
     </ProjectReference>
   </ItemGroup>
   ```

   Also set `CopyLocalLockFileAssemblies` to `false` in your `<PropertyGroup>` for the same reason:

   ```xml
   <CopyLocalLockFileAssemblies>false</CopyLocalLockFileAssemblies>
   ```

3. Implement `IConnect4Player`:

   ```csharp
   using connect4.library;
   using connect4.tournament;

   public class MyPlayer : IConnect4Player
   {
       public string Name { get; set; } = "My Player";
       public ConsoleColor Color { get; set; } = ConsoleColor.Cyan;
       public ConsoleColor AlternateColor { get; set; } = ConsoleColor.DarkCyan;
       public bool ShowBoardBeforeMove => false;
       public bool AcceptsCustomName => false;

       public void StartNewGame() { }

       public int GetMove(GameBoard board)
       {
           // Return a column number (1-based) for your next move.
           // Use board.IsMoveValid(col) to check a column before returning it.
           // Use board.CurrentBoard[row, col] to read the board state (0 = empty).
           return 4;
       }
   }
   ```

   See [connect4.exampleplayer/CenterPlayer.cs](connect4.exampleplayer/CenterPlayer.cs) for a complete working example.

4. Build the project and copy **only** the output DLL (not the dependency DLLs) into the `plugins/` directory next to `connect4.runtournament`.

5. Run the tournament — your player will be discovered automatically and included in the bracket.
