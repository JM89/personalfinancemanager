#!/bin/sh
set -eu

repo_root=$(CDPATH= cd -- "$(dirname -- "$0")/../.." && pwd)
toolkit=${PLATFORM_TOOLKIT_DIR:-"$repo_root/../ai-platform-toolkit"}
renderer="$toolkit/tools/architecture/render-structurizr.sh"
output="$repo_root/docs/sites/static/img/pfm/Architecture/current"

if [ ! -f "$renderer" ]; then
    printf '%s\n' "Platform toolkit renderer not found. Set PLATFORM_TOOLKIT_DIR to the toolkit checkout." >&2
    exit 2
fi

# The shared renderer mounts its root read-write. Stage only the model and helper.
stage=$(mktemp -d)
trap 'rm -f "$stage/tools/architecture/render-structurizr.sh" "$stage/pfm.dsl" "$stage/out/"*.svg; rmdir "$stage/out" "$stage/tools/architecture" "$stage/tools" "$stage"' EXIT
mkdir -p "$stage/tools/architecture" "$stage/out"
cp "$renderer" "$stage/tools/architecture/render-structurizr.sh"
cp "$repo_root/docs/architecture/pfm.dsl" "$stage/pfm.dsl"
sh "$stage/tools/architecture/render-structurizr.sh" pfm.dsl out

for view in pfm-system-context pfm-live-containers pfm-offline-containers; do
    if [ ! -s "$stage/out/$view.svg" ] || [ ! -s "$stage/out/$view-key.svg" ]; then
        printf 'Expected diagram was not generated: %s\n' "$view" >&2
        exit 1
    fi
done

mkdir -p "$output"
cp "$stage/out/"*.svg "$output/"
