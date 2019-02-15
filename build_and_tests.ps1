param (
 [string]$Configuration = "Debug"
)

$homeDirectory = $Env:HOME

if (!$homeDirectory) {
    $homeDirectory = $Env:UserProfile
}

$nugetPackageCacheDirectory = Join-Path $homeDirectory ".nuget" "packages" "MSBuild.AdditionalTasks"

Write-Host $nugetPackageCacheDirectory

if (!(Test-Path $nugetPackageCacheDirectory)) {
    Get-ChildItem -Path $nugetPackageCacheDirectory -Recurse -Hidden | Remove-Item -Force
    Remove-Item -Path $nugetPackageCacheDirectory -Force -Recurse
}

& dotnet build ./src/MSBuild.AdditionalTasks.sln /binaryLogger:msbuild.binlog /nodeReuse:false -c $Configuration

& dotnet build ./tests/Tests.sln /binaryLogger:msbuild.tests.binlog /nodeReuse:false -c $Configuration
