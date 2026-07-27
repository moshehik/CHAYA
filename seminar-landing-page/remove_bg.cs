using System;
using System.Drawing;
using System.Drawing.Imaging;

public class ImageProcessor
{
    public static void RemoveWhiteBackground(string inputPath, string outputPath)
    {
        using (Bitmap bmp = new Bitmap(inputPath))
        {
            bmp.MakeTransparent(Color.White);
            bmp.Save(outputPath, ImageFormat.Png);
        }
    }
}
