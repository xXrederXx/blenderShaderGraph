@echo off
setlocal

echo ====================================
echo         BUILD PROCESS STARTED
echo ====================================

:: Step 0: Go to project root
cd ..
if errorlevel 1 (
    echo [ERROR] Failed to change to parent directory.
    pause
    exit /b 1
)

:: Step 1: Build C# backend
echo.
echo === Step 1: Build C# backend ===
cd logic-src
if errorlevel 1 (
    echo [ERROR] Failed to enter logic-src directory.
    pause
    exit /b 1
)

dotnet publish -c Release -r win-x64 --self-contained true -o ../dist/logic
if errorlevel 1 (
    echo [ERROR] .NET publish failed.
    pause
    exit /b 1
)

cd ..
if errorlevel 1 (
    echo [ERROR] Failed to return to root directory after C# build.
    pause
    exit /b 1
)

:: Step 2: Package Python GUI
echo.
echo === Step 2: Package Python GUI ===
pyinstaller --noconfirm --specpath dist --onedir GUI-src/main.py --name main_gui
if errorlevel 1 (
    echo [ERROR] PyInstaller failed to build the Python GUI.
    pause
    exit /b 1
)

:: Step 3: Prepare folder for final build
echo.
echo === Step 3: Prepare folder for final build ===

copy /Y builder\starter.bat dist
if errorlevel 1 (
    echo [ERROR] Failed to copy starter.py.
    pause
    exit /b 1
)

echo.
echo ====================================
echo        BUILD PROCESS COMPLETE
echo ====================================
echo Check output at: dist\final\dist\final_app.exe

pause
