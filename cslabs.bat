@echo off
setlocal
cd CSLabs.Console
dotnet build > nul && dotnet run --no-build %*
endlocal