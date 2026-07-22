using System;
using System.Drawing;
using System.Drawing.Imaging;

public class Program {
    public static void Main(string[] args) {
        if(args.Length < 2) return;
        Bitmap orig = new Bitmap(args[0]);
        int w = orig.Width;
        int h = orig.Height;
        
        Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
        using(Graphics g = Graphics.FromImage(bmp)) {
            g.DrawImage(orig, 0, 0);
        }
        
        Color bg = bmp.GetPixel(0, 0);
        int threshold = 45;
        
        int minX = w, minY = h, maxX = 0, maxY = 0;
        
        for(int y=0; y<h; y++) {
            for(int x=0; x<w; x++) {
                Color c = bmp.GetPixel(x, y);
                // Background removal
                if(Math.Abs(c.R - bg.R) < threshold && Math.Abs(c.G - bg.G) < threshold && Math.Abs(c.B - bg.B) < threshold) {
                    bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                } else {
                    // Update bounds
                    if(x < minX) minX = x;
                    if(y < minY) minY = y;
                    if(x > maxX) maxX = x;
                    if(y > maxY) maxY = y;
                }
            }
        }
        
        // Sometimes AI images have stray artifacts. 
        // Let's do a second pass to find the TRUE bounding box of the main object!
        // We know the rings are roughly around 26,27 to 300,300.
        // Let's just find the first dense cluster of non-transparent pixels!
        
        // Actually, just find minX/maxX ignoring stray pixels by counting them.
        minX = w; minY = h; maxX = 0; maxY = 0;
        for(int y=0; y<h; y++) {
            int rowCount = 0;
            for(int x=0; x<w; x++) {
                if(bmp.GetPixel(x, y).A > 0) rowCount++;
            }
            if(rowCount > 5) {
                if(y < minY) minY = y;
                if(y > maxY) maxY = y;
            }
        }
        for(int x=0; x<w; x++) {
            int colCount = 0;
            for(int y=0; y<h; y++) {
                if(bmp.GetPixel(x, y).A > 0) colCount++;
            }
            if(colCount > 5) {
                if(x < minX) minX = x;
                if(x > maxX) maxX = x;
            }
        }
        
        minX = Math.Max(0, minX - 5);
        minY = Math.Max(0, minY - 5);
        maxX = Math.Min(w - 1, maxX + 5);
        maxY = Math.Min(h - 1, maxY + 5);
        
        Rectangle cropRect = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
        Bitmap cropped = new Bitmap(cropRect.Width, cropRect.Height, PixelFormat.Format32bppArgb);
        using(Graphics gr = Graphics.FromImage(cropped)){
            gr.DrawImage(bmp, new Rectangle(0, 0, cropped.Width, cropped.Height), cropRect, GraphicsUnit.Pixel);
        }
        
        cropped.Save(args[1], ImageFormat.Png);
        
        Console.WriteLine("Saved " + args[1] + " with size " + cropped.Width + "x" + cropped.Height);
    }
}
