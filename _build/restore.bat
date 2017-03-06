@echo off
set scripts=%~dp0
set scripts=%scripts:~0,-1%

set solution=AspNetCore.CleanStart.sln

dotnet restore %solution%
