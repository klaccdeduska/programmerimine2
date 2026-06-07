$ErrorActionPreference = "Stop"

$projectFile = ".\KooliProjekt.Application.UnitTests.csproj"
$resultsDir = ".\TestResults"
$reportDir = ".\TestReport"
$toolsDir = ".\.tools"

if (Test-Path $resultsDir) {
    Remove-Item $resultsDir -Recurse -Force
}

if (Test-Path $reportDir) {
    Remove-Item $reportDir -Recurse -Force
}

if (!(Test-Path $toolsDir)) {
    New-Item -ItemType Directory -Path $toolsDir | Out-Null
}

if (!(Test-Path "$toolsDir\reportgenerator.exe")) {
    dotnet tool install dotnet-reportgenerator-globaltool --tool-path $toolsDir
}

dotnet test $projectFile `
    --collect:"XPlat Code Coverage" `
    --results-directory $resultsDir

& "$toolsDir\reportgenerator.exe" `
    "-reports:$resultsDir\**\coverage.cobertura.xml" `
    "-targetdir:$reportDir" `
    "-reporttypes:Html"

Start-Process "$reportDir\index.html"