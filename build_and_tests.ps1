
$homeDirectory = $Env:HOME

$nugetPackageCacheDirectory = Join-Path $homeDirectory ".nuget" "packages" "MSBuild.AdditionalTasks"

Write-Host $nugetPackageCacheDirectory

Get-ChildItem -Path $nugetPackageCacheDirectory -Recurse -Hidden | Remove-Item -Force

Remove-Item -Path $nugetPackageCacheDirectory -Force -Recurse


& dotnet build ./src/MSBuild.AdditionalTasks.sln /binaryLogger:msbuild.binlog /nodeReuse:false

& dotnet build ./tests/Tests.sln /binaryLogger:msbuild.tests.binlog /nodeReuse:false
