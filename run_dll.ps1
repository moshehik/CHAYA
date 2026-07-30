[Reflection.Assembly]::LoadFile("$PSScriptRoot\ToneDownBlue.dll")
[ToneDownBlue]::Main([string[]]@('assets\ring_blue.png', 'assets\ring_blue_new.png'))
if (Test-Path 'assets\ring_blue_new.png') {
    Move-Item -Force 'assets\ring_blue_new.png' 'assets\ring_blue.png'
}
