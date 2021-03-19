#!/bin/bash
(cd CSLabs.Console && dotnet build > /dev/null && dotnet run --no-build "$@")