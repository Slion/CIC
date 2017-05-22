# Work out current path
$dir = Split-Path $MyInvocation.MyCommand.Path 
$dir += "\"

# Download RELEASES file
$localFileName = $dir + "RELEASES"
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

# Download last package
$localFileName = $dir + $lastFileName
$remoteFileName = "http://publish.slions.net/CIC/" + $lastFileName
Invoke-WebRequest -OutFile $localFileName $remoteFileName;

#
 Write-Error ("Some error")
 exit 1