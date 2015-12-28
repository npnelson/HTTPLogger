#Requires -Version 3.0
 
param($vsoProjectName, $projectName, $buildConfiguration, $buildSourcesDirectory)
 
$VerbosePreference = "continue"
$ErrorActionPreference = "Stop"

$env:DNX_BUILD_VERSION = $env:BUILD_BUILDID
 Write-Verbose "Version Number=$($env:BUILD_BUILDID)" 
     
&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}
$globalJson = Get-Content -Path $PSScriptRoot\global.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore
 
if($globalJson)
{
    $dnxVersion = $globalJson.sdk.version
}
else
{
    Write-Warning "Unable to locate global.json to determine using 'latest'"
    $dnxVersion = "latest"
}
 
& $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -Persistent
 
$dnxRuntimePath = "$($env:USERPROFILE)\.dnx\runtimes\dnx-clr-win-x86.$dnxVersion"

#& $env:DNX_BUILD_VERSION=$BUILD_BUILDID

     
& "dnu" "restore"  "$PSScriptRoot\src\"

& "dnu" "build" "$PSScriptRoot\src\NPNelson.HTTPLogger" "--configuration" "release"

& "dnu" "pack" "$PSScriptRoot\src\NPNelson.HTTPLogger" "--out" "$PSScriptRoot\artifacts\bin\Publish\" "--configuration" "release"

