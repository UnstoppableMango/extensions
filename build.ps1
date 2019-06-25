#!/usr/bin/env pwsh
[CmdletBinding(PositionalBinding = $false)]
param(
    [ValidateSet('Debug', 'Release')]
    $Configuration = $null,
    [switch]
    $ci,
    [ValidateSet('x86', 'x64', 'Arm')]
    $Architecture = $null,
    [switch]
    $sign,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$MSBuildArgs
)

Set-StrictMode -Version 1
$ErrorActionPreference = 'Stop'

Import-Module -Force -Scope Local "$PSScriptRoot/src/common.psm1"

#
# Main
#

if ($env:CI -eq 'true') {
    $ci = $true
}

if (!$Configuration) {
    $Configuration = if ($ci) { 'Release' } else { 'Debug' }
}

if (!$Architecture) {
    $Architecture = 'x64'
}

$MSBuildArgs += '/p:Architecture=' + $Architecture

if ($ci) {
    $MSBuildArgs += '/p:CI=true'
}

$isPr = ($env:BUILD_REASON -eq 'PullRequest')
if (-not (Test-Path variable:\IsCoreCLR)) {
    $IsWindows = $true
}

$artifacts = "$PSScriptRoot/artifacts/"

Remove-Item -Recurse $artifacts -ErrorAction Ignore
exec dotnet msbuild /t:UpdateCiSettings @MSBuildArgs
exec dotnet build --configuration $Configuration @MSBuildArgs
exec dotnet pack --no-restore --no-build --configuration $Configuration -o $artifacts @MSBuildArgs

[string[]] $testArgs=@()
if ($PSVersionTable.PSEdition -eq 'Core' -and -not $IsWindows) {
    $testArgs += '--framework','netcoreapp2.2'
}
if ($env:TF_BUILD) {
    $testArgs += '--logger', 'trx'
}

#exec dotnet test --no-restore --no-build --configuration $Configuration '-clp:Summary' `
#    "$PSScriptRoot/test/KG.Loyalty.MobGate.Tests/KG.Loyalty.MobGate.Tests.csproj" `
#    @testArgs `
#    @MSBuildArgs

write-host -f magenta 'Done'
