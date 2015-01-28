@ECHO OFF
call "%VS120COMNTOOLS%\vsvars32.bat"

IF EXIST .\packages GOTO BUILD
:NUGET
.nuget\nuget.exe restore

:BUILD
msbuild ShippingEasy.sln
if %ERRORLEVEL% neq 0 GOTO FAIL

:TEST
packages\NUnit.Runners.2.6.4\tools\nunit-console.exe Tests\bin\Debug\Tests.dll /noxml
if %ERRORLEVEL% neq 0 GOTO FAIL

@echo BUILD PASSED
GOTO :EOF

:FAIL
@echo BUILD FAILED
exit /b %ERRROLEVEL%