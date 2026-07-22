using System;
using System.Drawing;
using System.Drawing.Imaging;

public class ChangePurpleToPink
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: ChangePurpleToPink.exe <input_image> <output_image>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        try
        {
            Bitmap bmp = new Bitmap(inputPath);
            
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    
                    float h = pixelColor.GetHue();
                    float s = pixelColor.GetSaturation();
                    float b = pixelColor.GetBrightness();

                    // Purple hue is generally between 260 and 310
                    // We want to shift it towards pink which is around 320-350
                    if (h >= 260 && h <= 310 && s > 0.15f && b > 0.15f)
                    {
                        // Set hue to 335 (pinkish)
                        Color newColor = ColorFromAhsb(pixelColor.A, 335f, s, b);
                        bmp.SetPixel(x, y, newColor);
                    }
                }
            }

            ImageFormat format = ImageFormat.Jpeg;
            if (outputPath.ToLower().EndsWith(".png"))
            {
                format = ImageFormat.Png;
            }
            bmp.Save(outputPath, format);
            Console.WriteLine("Processed image saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public static Color ColorFromAhsb(int a, float h, float s, float b)
    {
        if (0 == s)
        {
            return Color.FromArgb(a, Convert.ToInt32(b * 255), Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
        }

        float fMax, fMid, fMin;
        int iSextant, iMax, iMid, iMin;

        if (0.5 < b)
        {
            fMax = b - (b * s) + s;
            fMin = b + (b * s) - s;
        }
        else
        {
            fMax = b + (b * s);
            fMin = b - (b * s);
        }

        iSextant = (int)Math.Floor(h / 60f);
        if (300f <= h)
        {
            h -= 360f;
        }
        h /= 60f;
        h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
        if (0 == iSextant % 2)
        {
            fMid = h * (fMax - fMin) + fMin;
        }
        else
        {
            fMid = fMin - h * (fMax - fMin);
        }

        iMax = Convert.ToInt32(fMax * 255);
        iMid = Convert.ToInt32(fMid * 255);
        iMin = Convert.ToInt32(fMin * 255);

        switch (iSextant)
        {
            case 1:
                return Color.FromArgb(a, iMid, iMax, iMin);
            case 2:
                return Color.FromArgb(a, iMin, iMax, iMid);
            case 3:
                return Color.FromArgb(a, iMin, iMid, iMax);
            case 4:
                return Color.FromArgb(a, iMid, iMin, iMax);
            case 5:
                return Color.FromArgb(a, iMax, iMin, iMid);
            default:
                return Color.FromArgb(a, iMax, iMid, iMin);
        }
    }
}
