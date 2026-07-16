@echo off
setlocal

echo Compiling Tinos3 main kernel...

cd /d "%~dp0Tinos3" || exit /b 1

dotnet publish Tinos3.csproj -c Debug -r linux-x64 -p:CosmosArch=x64
if errorlevel 1 (
    echo.
    echo Build failed.
    pause
    exit /b %errorlevel%
)

echo.
echo Build completed successfully.
pause