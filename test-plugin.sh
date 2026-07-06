#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PLUGIN_PROJECT="$SCRIPT_DIR/connect4.exampleplayer"
TOURNAMENT_PROJECT="$SCRIPT_DIR/connect4.runtournament"
PLUGIN_DLL="connect4.exampleplayer.dll"

# Build the tournament runner so we know its output path
echo "Building tournament runner..."
dotnet build "$TOURNAMENT_PROJECT/connect4.runtournament.csproj" --configuration Release -v quiet

TOURNAMENT_BIN="$TOURNAMENT_PROJECT/bin/Release/net10.0"
PLUGINS_DIR="$TOURNAMENT_BIN/plugins"

# Build the example player plugin
echo "Building example player plugin..."
dotnet build "$PLUGIN_PROJECT/connect4.exampleplayer.csproj" --configuration Release -v quiet

PLUGIN_SRC="$PLUGIN_PROJECT/bin/Release/net10.0/$PLUGIN_DLL"

if [[ ! -f "$PLUGIN_SRC" ]]; then
    echo "ERROR: Plugin DLL not found at $PLUGIN_SRC" >&2
    exit 1
fi

# Copy only the plugin DLL into the plugins directory
mkdir -p "$PLUGINS_DIR"
cp "$PLUGIN_SRC" "$PLUGINS_DIR/$PLUGIN_DLL"
echo "Copied $PLUGIN_DLL -> plugins/"

# Run the tournament pointing at the plugins directory
echo ""
echo "Running tournament..."
echo ""
dotnet run --project "$TOURNAMENT_PROJECT" --configuration Release -- "$PLUGINS_DIR"
