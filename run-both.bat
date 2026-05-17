@echo off
REM ====================================================================
REM LoginApp - Run Frontend + Backend Sekaligus
REM ====================================================================
REM Script ini membuka 2 command prompt terpisah untuk backend dan frontend
REM Jalankan dari folder projectTest: .\run-both.bat atau double-click
REM ====================================================================

setlocal enabledelayedexpansion

echo.
echo ====================================================================
echo   LoginApp - Starting Backend and Frontend
echo ====================================================================
echo.

REM Get current directory
cd /d "%~dp0"
set ProjectRoot=%cd%

echo 📁 Project Root: %ProjectRoot%
echo.

REM Path untuk backend dan frontend
set BackendPath=%ProjectRoot%\backend\LoginApp.API
set FrontendPath=%ProjectRoot%\frontend\LoginApp.Client

REM Check apakah folder ada
if not exist "%BackendPath%" (
    echo ❌ Backend folder not found: %BackendPath%
    echo.
    pause
    exit /b 1
)

if not exist "%FrontendPath%" (
    echo ❌ Frontend folder not found: %FrontendPath%
    echo.
    pause
    exit /b 1
)

echo ✅ Backend folder found: %BackendPath%
echo ✅ Frontend folder found: %FrontendPath%
echo.

REM ====================================================================
REM TERMINAL 1: Backend
REM ====================================================================
echo 🚀 Terminal 1: Opening Backend (http://localhost:5162)...
start "LoginApp Backend" cmd /k "cd /d "%BackendPath%" && echo. && echo ============================================ && echo   Backend Terminal - http://localhost:5162 && echo ============================================ && echo. && dotnet run"

echo ⏳ Waiting 3 seconds for backend to start...
timeout /t 3 /nobreak

REM ====================================================================
REM TERMINAL 2: Frontend
REM ====================================================================
echo 🚀 Terminal 2: Opening Frontend (https://localhost:7002)...
start "LoginApp Frontend" cmd /k "cd /d "%FrontendPath%" && echo. && echo ============================================ && echo   Frontend Terminal - https://localhost:7002 && echo ============================================ && echo. && dotnet run"

echo.
echo ====================================================================
echo   ✅ BOTH APPLICATIONS ARE STARTING!
echo ====================================================================
echo.
echo 📍 Backend:  http://localhost:5162
echo 📍 Frontend: https://localhost:7002
echo.
echo 🌐 Open in browser: https://localhost:7002
echo.
echo 💡 To stop, close both command prompt windows
echo.

pause
