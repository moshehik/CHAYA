$dir = Split-Path -Parent $MyInvocation.MyCommand.Path
Add-Type -TypeDefinition (Get-Content (Join-Path $dir "remove_bg.cs") -Raw) -ReferencedAssemblies System.Drawing
$inputPath = Join-Path $dir "logo_darchei_sarah.png"
$outputPath = Join-Path $dir "logo_darchei_sarah_transparent.png"
[ImageProcessor]::RemoveWhiteBackground($inputPath, $outputPath)
