param($aDir,$aVersion)
# Work out current path
#$dir = Split-Path $MyInvocation.MyCommand.Path 
$aDir += "\"

# Download RELEASES file
$localFileName = $aDir + "RELEASES"
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
$localFileName = $aDir + $lastFileName
$remoteFileName = "http://publish.slions.net/CIC/" + $lastFileName
Invoke-WebRequest -OutFile $localFileName $remoteFileName;

# Success
exit 0