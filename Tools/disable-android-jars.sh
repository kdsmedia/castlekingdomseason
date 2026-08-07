#!/usr/bin/env bash
# Tool: disable-android-jars.sh
# Purpose: Find .jar files under Assets/Plugins/Android and rename them to .jar.disabled
# to prevent Gradle duplicate-class issues when dependencies are managed by Gradle/Maven.

set -euo pipefail
ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
SEARCH_DIR="$ROOT_DIR/Assets/Plugins/Android"

if [ ! -d "$SEARCH_DIR" ]; then
  echo "No Android plugins directory found at $SEARCH_DIR"
  exit 0
fi

echo "Scanning for .jar files under $SEARCH_DIR..."
find "$SEARCH_DIR" -type f -name "*.jar" | while read -r JAR; do
  DISABLED="$JAR.disabled"
  if [ ! -f "$DISABLED" ]; then
    echo "Renaming $JAR -> $DISABLED"
    git mv "$JAR" "$DISABLED" || {
      # If git mv fails (e.g., running outside git), fallback to mv but warn
      echo "git mv failed; using mv"
      mv "$JAR" "$DISABLED"
    }
  else
    echo "Already disabled: $DISABLED"
  fi
done

echo "Done. Remember to commit and push the changes if running locally."
