$dir = Split-Path -Parent $MyInvocation.MyCommand.Path
Add-Type -TypeDefinition (Get-Content (Join-Path $dir "remove_bg.cs") -Raw) -ReferencedAssemblies System.Drawing
$inputPath = Join-Path $dir "image_3_new.jpg"
$outputPath = Join-Path $dir "image_3_transparent.png"
[ImageProcessor]::RemoveWhiteBackground($inputPath, $outputPath)
