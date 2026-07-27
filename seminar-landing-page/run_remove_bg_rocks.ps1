$dir = Split-Path -Parent $MyInvocation.MyCommand.Path
Add-Type -TypeDefinition (Get-Content (Join-Path $dir "remove_bg_advanced.cs") -Raw) -ReferencedAssemblies System.Drawing
$inputPath = Join-Path $dir "rocks_white.jpg"
$outputPath = Join-Path $dir "rocks_transparent.png"
[ImageProcessor]::RemoveWhiteBackground($inputPath, $outputPath)
