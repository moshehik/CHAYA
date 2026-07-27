using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class ImageProcessor
{
    public static void RemoveWhiteBackground(string inputPath, string outputPath)
    {
        using (Bitmap bmp = new Bitmap(inputPath))
        {
            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int pixelSize = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            bool hasAlpha = pixelSize == 4;

            for (int counter = 0; counter < rgbValues.Length; counter += pixelSize)
            {
                byte b = rgbValues[counter];
                byte g = rgbValues[counter + 1];
                byte r = rgbValues[counter + 2];
                
                // If it's very light (near white)
                if (r > 210 && g > 210 && b > 210)
                {
                    if (hasAlpha)
                    {
                        rgbValues[counter + 3] = 0; // Set alpha to 0
                    }
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);

            // If the original image didn't have an alpha channel, we need to create a new one that does
            if (!hasAlpha)
            {
                Bitmap transparentBmp = bmp.Clone(rect, PixelFormat.Format32bppArgb);
                BitmapData transData = transparentBmp.LockBits(rect, ImageLockMode.ReadWrite, transparentBmp.PixelFormat);
                
                int transBytes = Math.Abs(transData.Stride) * transparentBmp.Height;
                byte[] transRgbValues = new byte[transBytes];
                Marshal.Copy(transData.Scan0, transRgbValues, 0, transBytes);
                
                for (int counter = 0; counter < transRgbValues.Length; counter += 4)
                {
                    byte bb = transRgbValues[counter];
                    byte gg = transRgbValues[counter + 1];
                    byte rr = transRgbValues[counter + 2];
                    
                    if (rr > 220 && gg > 220 && bb > 220)
                    {
                        transRgbValues[counter + 3] = 0; // Alpha 0
                    }
                    else
                    {
                        transRgbValues[counter + 3] = 255; // Alpha 255
                    }
                }
                
                Marshal.Copy(transRgbValues, 0, transData.Scan0, transBytes);
                transparentBmp.UnlockBits(transData);
                transparentBmp.Save(outputPath, ImageFormat.Png);
                transparentBmp.Dispose();
            }
            else
            {
                bmp.Save(outputPath, ImageFormat.Png);
            }
        }
    }
}
