Add-Type -AssemblyName System.Drawing

$src = 'C:\Users\MOSHE\Desktop\דרכי שרה\seminar-landing-page\תמונה 7.png'
$dest = 'C:\Users\MOSHE\Desktop\דרכי שרה\seminar-landing-page\תמונה 7_new.png'

$img = [System.Drawing.Image]::FromFile($src)
$w = $img.Width
$h = $img.Height

$scale = 0.65
$new_w = [int]($w * $scale)
$new_h = [int]($h * $scale)

$bmp = New-Object System.Drawing.Bitmap($w, $h)
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic

$bmp_src = New-Object System.Drawing.Bitmap($src)
# Try to get edge color from the top center
$edgeColor = $bmp_src.GetPixel($w / 2, 5)

$brush = New-Object System.Drawing.SolidBrush($edgeColor)
$g.FillRectangle($brush, 0, 0, $w, $h)

$x = [int](($w - $new_w) / 2)
$y = [int](($h - $new_h) / 2)

$g.DrawImage($img, $x, $y, $new_w, $new_h)

$bmp.Save($dest, [System.Drawing.Imaging.ImageFormat]::Png)

$g.Dispose()
$bmp.Dispose()
$img.Dispose()
$bmp_src.Dispose()
