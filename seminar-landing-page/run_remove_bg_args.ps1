param (
    [string]$InputFile,
    [string]$OutputFile
)

$dir = Split-Path -Parent $MyInvocation.MyCommand.Path
Add-Type -TypeDefinition (Get-Content (Join-Path $dir "remove_bg_advanced.cs") -Raw) -ReferencedAssemblies System.Drawing
[ImageProcessor]::RemoveWhiteBackground($InputFile, $OutputFile)
