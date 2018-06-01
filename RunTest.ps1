#!/usr/bin/env powershell -File
# AmiKo|CoMed
param([string]$application)
Write-Host $application

# NOTE:
#
# > powershell.exe -ExecutionPolicy Bypass -File .\RunTest.ps1 "AmiKo"
# > powershell.exe -ExecutionPolicy Bypass -File .\RunTest.ps1 "CoMed"
#

if ($application -ne "AmiKo" -and $application -ne "CoMed") {
  exit 1
}

taskkill /im 'MSBuild.exe' /f
taskkill /im "$application Desitin.exe" /f

# Build
MSBuild.exe .\AmiKoWindows.Tests\"$application"Desitin.Test.csproj /t:Clean
MSBuild.exe .\AmiKoWindows.Tests\"$application"Desitin.Test.csproj /t:Build `
  /p:Configuration=Debug `
  /p:Platform=AnyCPU `
  /p:Log=Trace

if ($lastexitcode -ne 0) {
  Write-Host "Build faild with status: $lastexitcode"
  exit
}

# Run unit tests
$origin = $PWD
$location = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

Push-Location $location
[Environment]::CurrentDirectory = $location

$runner = "$location\Packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe"

& "$runner" $location\AmiKoWindows.Tests\bin\Debug\"$application"\"$application"Desitin.Test.dll `
  --output TestOutput.log

Pop-Location
[Environment]::CurrentDirectory = $origin
