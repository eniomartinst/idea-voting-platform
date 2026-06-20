#!/bin/bash

# FUTURE IMPLEMENTATION NOTE:
# Currently, there are no online database containers provisioned in our CI environment.
# This conditional ensures the pipeline passes cleanly for now while allowing local execution.
# Once cloud databases are available, this condition can be adjusted or removed.

if [ "$GITLAB_CI" == "true" ]; then
    echo "☁️ CI environment detected. Skipping actual migration (cloud databases not yet provisioned)."
    exit 0
else
    echo "💻 Local environment detected. Executing migration script..."
    python3 scripts/migrate_cis.py
fi
