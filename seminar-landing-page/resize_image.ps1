Add-Type -AssemblyName System.Drawing

$src = 'C:\Users\MOSHE\Desktop\דרכי שרה\seminar-landing-page\תמונה לאזור 1.jpg'
$dest = 'C:\Users\MOSHE\Desktop\דרכי שרה\seminar-landing-page\תמונה לאזור 1_new.jpg'

$img = [System.Drawing.Image]::FromFile($src)
$w = $img.Width
$h = $img.Height

$scale = 0.88
$new_w = [int]($w * $scale)
$new_h = [int]($h * $scale)

$bmp = New-Object System.Drawing.Bitmap($w, $h)
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic

# Use the top-left pixel color for the background
$bmp_src = New-Object System.Drawing.Bitmap($src)
$edgeColor = $bmp_src.GetPixel($w / 2, 5)

$brush = New-Object System.Drawing.SolidBrush($edgeColor)
$g.FillRectangle($brush, 0, 0, $w, $h)

$x = [int](($w - $new_w) / 2)
$y = [int](($h - $new_h) / 2) + [int](($h - $new_h) / 3) # Push down slightly more to leave canopy room

$g.DrawImage($img, $x, $y, $new_w, $new_h)

$bmp.Save($dest, [System.Drawing.Imaging.ImageFormat]::Jpeg)

$g.Dispose()
$bmp.Dispose()
$img.Dispose()
$bmp_src.Dispose()
