@echo off
chcp 65001 > nul
echo ============================================================
echo  Build All Demos
echo ============================================================
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build-all.ps1" %*

echo.
if %ERRORLEVEL% EQU 0 (
    echo [DONE] Build completed successfully.
) else (
    echo [FAIL] Build finished with errors. Check output above.
)
pause
