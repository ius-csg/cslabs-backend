@echo off
setlocal
cd CSLabs.ConsoleUtil
dotnet build > nul && dotnet run --no-build %*
endlocal