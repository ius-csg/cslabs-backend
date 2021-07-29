#!/bin/bash
(cd CSLabs.ConsoleUtil && dotnet build > /dev/null && dotnet run --no-build "$@")