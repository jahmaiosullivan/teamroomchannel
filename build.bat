@echo off

if ERRORLEVEL 1 goto end

set Configuration=Release

if /i "%1"=="DEBUG" set Configuration=Debug

if "%1"=="/n" goto NewScript
if "%1"=="/N" goto NewScript

echo abc | powershell -NoProfile -ExecutionPolicy unrestricted -Command "%~dp0\ExternalBinaries\psake\psake.ps1 .\BuildScript.ps1 -properties @{configuration='%Configuration%'} -framework '4.0x64' %*"
goto end


:NewScript
Database\Tools\DatabaseMigrationManager.exe add /p Database\Main /n %2
echo Script Created
goto end

:end
IF %ERRORLEVEL% NEQ 0 EXIT /B %ERRORLEVEL%