using System;
using System.Drawing;
using System.Drawing.Imaging;

class Program {
    static void Main(string[] args) {
        if (args.Length < 2) return;
        Bitmap original = new Bitmap(args[0]);
        Bitmap bmp = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(bmp)) {
            g.DrawImage(original, 0, 0);
        }
        original.Dispose();

        for(int y=0; y<bmp.Height; y++){
            for(int x=0; x<bmp.Width; x++){
                Color c = bmp.GetPixel(x,y);
                int max = Math.Max(c.R, Math.Max(c.G, c.B));
                
                // If it's near black, just make it completely transparent
                if (max < 15) {
                    bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                } else {
                    // For translucent parts, keep the brightness as alpha but scale it up slightly
                    int alpha = Math.Min(255, (int)(max * 1.5));
                    bmp.SetPixel(x, y, Color.FromArgb(alpha, c.R, c.G, c.B));
                }
            }
        }
        bmp.Save(args[1], ImageFormat.Png);
    }
}
