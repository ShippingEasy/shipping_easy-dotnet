@ECHO OFF
SETLOCAL

SET CONFIG=%1
IF NOT DEFINED CONFIG (SET CONFIG=debug)
ECHO BUILDING %CONFIG%

IF DEFINED FRAMEWORKDIR GOTO NUGET
echo Setting Visual Studio environment variables
call "%VS120COMNTOOLS%\vsvars32.bat"

:NUGET
IF EXIST .\packages GOTO BUILD
.nuget\nuget.exe restore

:BUILD
set /p GIT_SHA=<.git\refs\heads\master
echo [assembly: System.Reflection.AssemblyTrademark("%GIT_SHA%")] > ShippingEasy/Properties/BuildInfo.cs
msbuild ShippingEasy.sln /p:Configuration=%CONFIG%
if %ERRORLEVEL% neq 0 GOTO FAIL

:TEST
packages\NUnit.Runners.2.6.4\tools\nunit-console.exe Tests\bin\%CONFIG%\Tests.dll /noxml
if %ERRORLEVEL% neq 0 GOTO FAIL

@echo BUILD PASSED
GOTO :EOF

:FAIL
@echo BUILD FAILED
exit /b %ERRROLEVEL%