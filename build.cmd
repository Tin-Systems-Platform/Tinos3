@echo off
setlocal

set "ROOT=%~dp0"

echo Cleaning previous build output...
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%ROOT%clean.ps1"
if errorlevel 1 (
    echo.
    echo Cleanup failed.
    pause
    exit /b 1
)

where cosmos >nul 2>&1
if errorlevel 1 (
    echo.
    echo Cosmos CLI was not found. Install Cosmos and try again.
    pause
    exit /b 1
)

echo.
echo Compiling Tinos3 main kernel...
pushd "%ROOT%Tinos3" || exit /b 1

call cosmos build
set "BUILD_EXIT=%errorlevel%"

popd

if not "%BUILD_EXIT%"=="0" (
    echo.
    echo Build failed.
    pause
    exit /b %BUILD_EXIT%
)

echo.
echo Build completed successfully.
pause