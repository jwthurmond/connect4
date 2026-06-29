# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build everything
dotnet build connect4.sln

# Run tests
dotnet test connect4.sln

# Run a single test
dotnet test connect4.tests/connect4.tests.csproj --filter "FullyQualifiedName~TestHorizontalWinRow1Column4To7"

# Play a game (interactive)
dotnet run --project connect4

# Run a tournament (built-in players only)
dotnet run --project connect4.runtournament

# Run a tournament with a plugin directory
dotnet run --project connect4.runtournament -- /path/to/plugins
```

## Architecture

The solution has four runtime projects with a clear dependency hierarchy:

```
connect4.library          ← no dependencies; owns GameBoard
connect4.tournament       ← depends on library; owns IConnect4Player, Match, Tournament
connect4 / connect4.runtournament  ← depend on both; are the two runnable entry points
connect4.exampleplayer    ← depends on tournament (compile-time only); ships as a plugin DLL
```

**`connect4.library`** — `GameBoard` is the core game object. It is mutable and passed by reference through `Move()`, which returns a `MoveResult` containing the (same) board. Columns are 1-based externally; all internal indexing is 0-based. `IsMoveValid(col)` is the safe check before committing a move. The board tracks `Winner` (0 = none, 1 = P1, 2 = P2) and enforces a 5-strike rule for invalid moves (`InvalidMoveCountMax`).

**`connect4.tournament`** — `IConnect4Player` is the plugin contract. All built-in players (`RandomPlayer`, `IncrementBy1`, `Always4`, `Lowest`, `Highest`, `HumanInput`) live in `IConnect4Player.cs` alongside the interface. `Match` runs N rounds between two players and tracks per-player win counts. `Tournament` builds a round-robin bracket (each pair plays once; each player also plays itself), runs all matches, and prints results.

**`connect4.runtournament`** — `PlayerLoader` uses `AssemblyLoadContext` to dynamically load player DLLs from a plugins directory at startup. The critical behaviour: host-loaded assemblies (`connect4.tournament`, `connect4.library`) are resolved back to the host so type identity is shared — plugins must NOT copy these DLLs into their output (use `ExcludeAssets="runtime"` and `CopyLocalLockFileAssemblies=false`).


## Writing a player plugin

Implement `IConnect4Player` in a class library, reference `connect4.tournament` with `ExcludeAssets="runtime"`, build, and drop the DLL in the plugins folder. See `connect4.exampleplayer` for a complete working template.
