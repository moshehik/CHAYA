using System;
using System.Drawing;
using System.Drawing.Imaging;

public class ToneDownBlue
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: ToneDownBlue.exe <input_image> <output_image>");
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
                    
                    if (pixelColor.A == 0) continue;

                    float h = pixelColor.GetHue();
                    float s = pixelColor.GetSaturation();
                    float b = pixelColor.GetBrightness();

                    // Cyan / Light blue hue is generally between 170 and 260
                    if (h >= 170 && h <= 260 && s > 0.05f)
                    {
                        // Decrease saturation (tone it down)
                        float newS = s * 0.75f; // was multiplied by 1.5, now going down
                        // Increase brightness a bit (lighten it up)
                        float newB = Math.Min(1.0f, b * 1.1f); // was multiplied by 0.85
                        
                        // Slightly revert the hue shift if it was pushed too far, but let's keep the hue intact
                        float newH = h;
                        
                        Color newColor = ColorFromAhsb(pixelColor.A, newH, newS, newB);
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
