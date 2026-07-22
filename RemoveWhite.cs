using System;
using System.Drawing;
using System.Drawing.Imaging;

class Program {
    static void Main(string[] args) {
        if (args.Length < 2) return;
        Bitmap bmp = new Bitmap(args[0]);
        Bitmap target = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
        
        for(int y=0; y<bmp.Height; y++){
            for(int x=0; x<bmp.Width; x++){
                Color c = bmp.GetPixel(x,y);
                int brightness = (c.R + c.G + c.B) / 3;
                
                // Pure white to transparent.
                // We use multiply logic: if we just convert white to alpha.
                // An elegant way to convert a white-background image to transparent PNG:
                // alpha = 255 - brightness
                // color = black, or keep original color and divide by alpha
                // Since we want to keep colors (the heart is red, hands are dark),
                // we can map brightness to transparency.
                
                if (brightness > 250) {
                    target.SetPixel(x, y, Color.FromArgb(0, c.R, c.G, c.B));
                } else if (brightness > 180) {
                    int alpha = (int)(255 - (brightness - 180) * (255.0 / 70.0));
                    target.SetPixel(x, y, Color.FromArgb(alpha, c.R, c.G, c.B));
                } else {
                    target.SetPixel(x, y, Color.FromArgb(255, c.R, c.G, c.B));
                }
            }
        }
        
        target.Save(args[1], ImageFormat.Png);
    }
}
