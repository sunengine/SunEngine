function Get-VariablesFromFile($filename = "BUILD") {
    $fullPath = Join-Path -Path $PWD -ChildPath $filename
    if (-not(Test-Path -Path $fullPath -PathType Leaf)) {
        Write-Output "BUILD file is missing"
        Write-Output "Script should execute from ""Scripts"" directory"
        exit
    }
    $content = Get-Content -Path $fullPath -Raw
    $pattern = @"
(?<key>^[A-Z]\w+)="(?<value>.+)"
"@
    $dict = New-Object "System.Collections.Generic.Dictionary[String,String]"
    $_matches = [System.Text.RegularExpressions.Regex]::Matches($content, $pattern, 
    [System.Text.RegularExpressions.RegexOptions]::Multiline)
    foreach ($match in $_matches) {
        $key = $match.Groups["key"].Value
        $value = $match.Groups["value"].Value
        $dict.Add($key, $value)
    }
    return $dict
}

function Remove-RootProjectFromString([string]$str) {
    $n = $str.IndexOf('/')
    return $str.Substring($n)
}

function Write-FormattedOutput {
    [CmdletBinding()]
    Param(
         [Parameter(Mandatory=$True,Position=1,ValueFromPipeline=$True,ValueFromPipelinebyPropertyName=$True)][Object] $Object,
         [Parameter(Mandatory=$False)][ConsoleColor] $BackgroundColor,
         [Parameter(Mandatory=$False)][ConsoleColor] $ForegroundColor
    )    

    # save the current color
    $bc = $host.UI.RawUI.BackgroundColor
    $fc = $host.UI.RawUI.ForegroundColor

    # set the new color
    if($BackgroundColor -ne $null) { 
       $host.UI.RawUI.BackgroundColor = $BackgroundColor
    }

    if($ForegroundColor -ne $null) {
        $host.UI.RawUI.ForegroundColor = $ForegroundColor
    }

    Write-Output $Object
  
    # restore the original color
    $host.UI.RawUI.BackgroundColor = $bc
    $host.UI.RawUI.ForegroundColor = $fc
}

$d = Get-VariablesFromFile -filename "BUILD"

$project_root
$server_path
$client_path
$config_path
$build_path
$npm_util = $d["NPM_UTIL"]

if ($d["PROJECT_ROOT"] -eq "auto") {
    while (-not (Test-Path -Path (Join-Path -Path $PWD -ChildPath "SunEngine.md"))) {
        Set-Location ..\ | Out-Null
    }
    $project_root = $PWD

}
else {
    $project_root = $d["PROJECT_ROOT"]
}

$server_path = Join-Path -Path $PWD -ChildPath (Remove-RootProjectFromString($d["SERVER_PATH"]))
$client_path = Join-Path -Path $PWD -ChildPath (Remove-RootProjectFromString($d["CLIENT_PATH"]))
$config_path = Join-Path -Path $PWD -ChildPath (Remove-RootProjectFromString($d["CONFIG_PATH"]))
$build_path = Join-Path -Path $PWD -ChildPath (Remove-RootProjectFromString($d["BUILD_PATH"]))

Write-FormattedOutput -Object "Building $($d["PROJECT_NAME"]) project." -ForegroundColor Green

Write-FormattedOutput -Object "PROJECT_ROOT = $($project_root)." -ForegroundColor Green
Write-FormattedOutput -Object "SERVER_PATH = $($server_path)." -ForegroundColor Green
Write-FormattedOutput -Object "CLIENT_PATH = $($client_path)." -ForegroundColor Green
Write-FormattedOutput -Object "CONFIG_PATH = $($config_path)." -ForegroundColor Green
Write-FormattedOutput -Object "BUILD_PATH = $($build_path)." -ForegroundColor Green

# check Config directory exists
if (Test-Path -Path (Join-Path -Path $project_root -ChildPath "Config.template")) {
    Write-FormattedOutput -Object "Creating Config directory from Config.template" -ForegroundColor Green
    $p = Join-Path -Path $project_root -ChildPath "Config.template"
    Copy-Item -Source $p -Destination $config_path -Force
}

Write-FormattedOutput -Object "Deleting old build" -ForegroundColor Green
Remove-Item -Path $build_path -Recurse
New-Item -Path $build_path -ItemType Directory | Out-Null

# dotnet build
try {
    $path1 = Join-Path -Path $server_path -ChildPath "SunEngine.Cli"
    $path2 = Join-Path -Path $build_path -ChildPath "Server"
    & dotnet publish --configuration Release $path1 --output $path2 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw new [System.Exception]
    }
}
catch [System.Management.Automation.CommandNotFoundException] {
    Write-FormattedOutput -Object ".NET Core not install." -ForegroundColor Red
    exit
}
catch {
    Set-Location -Path (Join-Path -Path $project_root -ChildPath "Scripts")
    Write-FormattedOutput -Object "Crush build .net applicaton" -ForegroundColor Red
    exit
}

Write-FormattedOutput -Object "Building Client" -ForegroundColor Green
Set-Location -Path $client_path | Out-Null
# install node_modules
try {
    & $npm_util install | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw new [System.Exception]
    }
}
catch [System.Management.Automation.CommandNotFoundException] {
    Write-FormattedOutput -Object "npm util not found" -ForegroundColor Red
    exit
}
catch {
    Set-Location -Path (Join-Path -Path $project_root -ChildPath "Scripts")
    Write-FormattedOutput -Object "Fail npm install" -ForegroundColor Red
    exit
}

# check Client/src/site exists
if (-not(Test-Path -Path (Join-Path -Path $client_path -ChildPath "src/site"))) {
    Write-FormattedOutput -Object "Copying $($client_path)/src/site.template => $($client_path)/src/site" -ForegroundColor Green
    $path1 = Join-Path -Path $client_path -ChildPath "src\site.template\*"
    $path2 = Join-Path -Path $client_path -ChildPath "src\site"
    New-Item -Path $path2 -ItemType Directory | Out-Null
    Copy-Item -Path $path1 -Destination $path2 -Recurse
}
# quasar build
Write-FormattedOutput -Object "Quasar building" -ForegroundColor Blue
try {
    & quasar build | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw new [System.Exception]
    }
}
catch [System.Management.Automation.CommandNotFoundException] {
    Write-FormattedOutput -Object "Quasar not install please install by command ""npm install -g @quasar/cli""" -ForegroundColor Red
    exit
}
catch {
    Set-Location -Path (Join-Path -Path $project_root -ChildPath "Scripts")
    Write-FormattedOutput -Object "Quasar build was failed" -ForegroundColor Red
    exit
}

# copy dirs and files
Write-FormattedOutput -Object "Copying Client to wwwroot directory" -ForegroundColor Green
New-Item -Path (Join-Path -Path $build_path -ChildPath "wwwroot") -ItemType Directory | Out-Null
Copy-Item -Path (Join-Path -Path $client_path -ChildPath "dist\spa\*.*") -Destination (Join-Path -Path $build_path -ChildPath "wwwroot") -Recurse

Write-FormattedOutput -Object "Clearing dist directory" -ForegroundColor Green
Remove-Item -Path (Join-Path -Path $client_path -ChildPath "dist") -Recurse | Out-Null

Write-FormattedOutput -Object "Copying Config to build directory" -ForegroundColor Green
New-Item -Path (Join-Path -Path $build_path -ChildPath "config") -ItemType Directory | Out-Null
Copy-Item -Path $config_path -Destination (Join-Path -Path $build_path -ChildPath "config\") -Recurse

Write-FormattedOutput -Object "Copying Data to build directory" -ForegroundColor Green
New-Item -Path (Join-Path -Path $build_path -ChildPath "Resources") -ItemType Directory | Out-Null
Copy-Item -Path (Join-Path -Path $project_root -ChildPath "Resources\*") -Destination (Join-Path -Path $build_path -ChildPath "Resources\") -Recurse

Write-FormattedOutput -Object "Copying SunEngine.md file" -ForegroundColor Green
Copy-Item -Path (Join-Path -Path $project_root -ChildPath "SunEngine.md") -Destination $build_path

Set-Location -Path (Join-Path -Path $project_root -ChildPath "Scripts")
Write-FormattedOutput -Object "All Done!" -ForegroundColor Blue
[System.Media.SystemSounds]::Beep.Play()
Add-Type -AssemblyName System.Windows.Forms
[System.Windows.Forms.MessageBox]::Show("All Done!")
