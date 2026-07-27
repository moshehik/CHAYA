$dir = Split-Path -Parent $MyInvocation.MyCommand.Path
Add-Type -TypeDefinition (Get-Content (Join-Path $dir "remove_bg_advanced.cs") -Raw) -ReferencedAssemblies System.Drawing
$inputPath = Join-Path $dir "תמונה 6_watercolor_day.jpg"
$outputPath = Join-Path $dir "תמונה 6_watercolor_day_transparent.png"
[ImageProcessor]::RemoveWhiteBackground($inputPath, $outputPath)
