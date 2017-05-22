param($aNuGetExe,$aSquirrelExe,$aDir,$aVersion)
#$aDir += "\"

# Generate nupkg
Write-Output("Generating nupkg...")
[System.Diagnostics.Process]::Start($aNuGetExe,
"pack ..\Publish\CIC.nuspec -Properties Configuration=Release;Version=$aVersion -OutputDirectory $aDir -BasePath $aDir"
).WaitForExit()

# Work out current path
#$dir = Split-Path $MyInvocation.MyCommand.Path 

# Create download folder if needed
$squirrelReleaseDir = $aDir + "Squirrel\";
if (-not(Test-Path $squirrelReleaseDir))
{
    New-Item $squirrelReleaseDir -ItemType Directory | Out-Null;
}

# Download RELEASES file
Write-Output("Downloading Squirrel RELEASES...")
$localFileName = $squirrelReleaseDir + "RELEASES"
$remoteFileName = "http://publish.slions.net/CIC/RELEASES"
Invoke-WebRequest -OutFile $localFileName $remoteFileName;

# Parse RELEASES file to obtain the name of our last package
$reader = [System.IO.File]::OpenText( $localFileName )
while($null -ne ($line = $reader.ReadLine())) {
    $lastFileName = $line.Split(" ")[1]	
}
$reader.Close();

# Work out version number
$version = $lastFileName.Split("-")[1]
$major = $version.Split(".")[0]
$minor = $version.Split(".")[1]
$build = $version.Split(".")[2]

if ($aVersion -eq $version)
{
    Write-Error ("Version $version already published!")
    exit 1
}

# Download last package
Write-Output("Downloading last Squirrel package...")
$localFileName = $squirrelReleaseDir + $lastFileName
$remoteFileName = "http://publish.slions.net/CIC/" + $lastFileName
Invoke-WebRequest -OutFile $localFileName $remoteFileName;

# Do our Squirrel release
Write-Output("Generate Squirrel release...")
[System.Diagnostics.Process]::Start($aSquirrelExe,
" --r $squirrelReleaseDir --releasify $aDir" + "CIC.$aVersion.nupkg").WaitForExit()

# Clean-up by removing the downloaded Squirrel package
Remove-Item $localFileName

# From here on the Squirrel folder contains only stuff we need to upload

# Success
exit 0