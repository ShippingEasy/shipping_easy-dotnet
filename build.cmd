@ECHO OFF
SETLOCAL

REM Builds, tests, and packages the client
REM
REM By default, it runs a debug build
REM
REM To create nuget packages, run a release build: build.cmd release

SET CONFIG=%1
IF NOT DEFINED CONFIG (SET CONFIG=debug)
ECHO BUILDING %CONFIG%

IF DEFINED FRAMEWORKDIR GOTO NUGETRESTORE
echo Setting Visual Studio environment variables
call "%VS120COMNTOOLS%\vsvars32.bat"

:NUGETRESTORE
.nuget\nuget.exe restore

:BUILD
set /p GIT_SHA=<.git\refs\heads\master
echo [assembly: System.Reflection.AssemblyTrademark("%GIT_SHA%")] > ShippingEasy/Properties/BuildInfo.cs
msbuild ShippingEasy.sln /p:Configuration=%CONFIG%
if %ERRORLEVEL% neq 0 GOTO FAIL

:TEST
packages\NUnit.Runners.2.6.4\tools\nunit-console.exe Tests\bin\%CONFIG%\Tests.dll /noxml
if %ERRORLEVEL% neq 0 GOTO FAIL

IF /I NOT %CONFIG%==RELEASE GOTO DONE
@echo Creating nuget package
.nuget\nuget.exe pack ShippingEasy\ShippingEasy.csproj -Prop Configuration=Release -sym

:DONE
@echo BUILD PASSED
GOTO :EOF

:FAIL
@echo BUILD FAILED
exit /b %ERRROLEVEL%