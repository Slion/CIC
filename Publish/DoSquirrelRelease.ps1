param(
# Full path to NuGet.exe
$aNuGetExe,
# Full path to Squirrel.exe
$aSquirrelExe,
# Output directory including trailing backslash
$aOutDir,
# Version string typically in the following format: "1.2.3"
$aVersion,
# Full path to nuspec
$aNuSpecPath,
# Application download URL, including trailing forwardslash
$aUrl,
# Application ID, should match the name of the NuSpec file
$aAppId
)

# 
#Write-Output("NuGetExe: $aNuGetExe")
#Write-Output("SquirrelExe: $aSquirrelExe")
#Write-Output("OutDir: $aOutDir")
#Write-Output("Version: $aVersion")
#Write-Output("NuSpecPath: $aNuSpecPath")
#Write-Output("Url: $aUrl")
#Write-Output("AppId: $aAppId")

#
# Run a command with arguments and provide its standard and error output
# TODO: Find a way to return the exit code, that's apparently rather tricky in PowerShell
Function Execute-Command ($aCommand, $aArguments)
{

    # See: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.management/start-process?view=powershell-7.2
    #Start-Process $aCommand -ArgumentList $aArguments -NoNewWindow -Windowstyle Hidden -PassThru -Wait


    Write-Output("Execute:`n$aCommand $aArguments")

    #Try {
        $pinfo = New-Object System.Diagnostics.ProcessStartInfo
        $pinfo.FileName = $aCommand
        $pinfo.RedirectStandardError = $true
        $pinfo.RedirectStandardOutput = $true
        $pinfo.UseShellExecute = $false
        $pinfo.WindowStyle = 'Hidden'
        $pinfo.CreateNoWindow = $True
        $pinfo.Arguments = $aArguments
        $p = New-Object System.Diagnostics.Process
        $p.StartInfo = $pinfo
        #Write-Host ($p.StartInfo | Format-List | Out-String)
        $p.Start() | Out-Null
        $stdout = $p.StandardOutput.ReadToEnd().Trim()
        $stderr = $p.StandardError.ReadToEnd().Trim()
        $p.WaitForExit()
        # Need to do that as trying to get it directly from Write-Output is not working
        $exitCode = $p.ExitCode

        if (![string]::IsNullOrEmpty($stdout))
        {
            Write-Output("${stdout}")
        }

        if (![string]::IsNullOrEmpty($stderr))
        {
            Write-Output("${stderr}")
        }
        
        Write-Output("ExitCode: ${exitCode}")


        #$p | Add-Member "StdOut" $stdout
        #$p | Add-Member "StdErr" $stderr
    #}
    
    #Catch {
    #    Write-Output("ERROR!")
    #}
    
    #return $p
}

# Generate nupkg from nuspec, version and output directory
Write-Output("Generating nupkg...")
#Write-Output("$aNuGetExe  pack $aNuSpecPath -Properties Configuration=Release;Version=$aVersion -OutputDirectory $aOutDir -BasePath $aOutDir")
#[System.Diagnostics.Process]::Start($aNuGetExe,
#"pack $aNuSpecPath -Properties Configuration=Release;Version=$aVersion -OutputDirectory $aOutDir -BasePath $aOutDir"
#).WaitForExit()

Execute-Command $aNuGetExe "pack $aNuSpecPath -Properties Configuration=Release;Version=$aVersion -OutputDirectory $aOutDir -BasePath $aOutDir"

#Write-Host ($res | Format-List | Out-String)

# Create download folder below our output directory, if needed
$squirrelReleaseDir = $aOutDir + "Squirrel\";
if (-not(Test-Path $squirrelReleaseDir))
{
    New-Item $squirrelReleaseDir -ItemType Directory | Out-Null;
}

# Download RELEASES file
Write-Output("Downloading Squirrel RELEASES...")
$localFileName = $squirrelReleaseDir + "RELEASES"
$remoteFileName = $aUrl + "RELEASES"
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
    # Warn
    Write-Warning ("Version $version already published!")
    # Delete downloaded RELEASES to avoid uploading them back pointlessly 
    Remove-Item $localFileName
    # Still a successful exit as this should not fail the build
    exit 0
}

# Download last package
Write-Output("Downloading last Squirrel package...")
$localFileName = $squirrelReleaseDir + $lastFileName
$remoteFileName = $aUrl + $lastFileName
Invoke-WebRequest -OutFile $localFileName $remoteFileName;

# Do our Squirrel release
Write-Output("Generate Squirrel release...")
#Write-Output("$aSquirrelExe  --r $squirrelReleaseDir --releasify $aOutDir$aAppId.$aVersion.nupkg")
#[System.Diagnostics.Process]::Start($aSquirrelExe,
#" --r $squirrelReleaseDir --releasify $aOutDir$aAppId.$aVersion.nupkg").WaitForExit()
Execute-Command $aSquirrelExe " --r $squirrelReleaseDir --releasify $aOutDir$aAppId.$aVersion.nupkg"

# Clean-up by removing the downloaded Squirrel package
Remove-Item $localFileName

# From here on the Squirrel folder contains only stuff we need to upload

#TODO: report error from executions
# Success
exit 0


