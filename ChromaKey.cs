using System;
using System.Drawing;
using System.Drawing.Imaging;

class Program {
    static void Main(string[] args) {
        if (args.Length < 2) return;
        Bitmap bmp = new Bitmap(args[0]);
        int minX = bmp.Width, minY = bmp.Height, maxX = 0, maxY = 0;
        
        Bitmap target = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
        
        for(int y=0; y<bmp.Height; y++){
            for(int x=0; x<bmp.Width; x++){
                Color c = bmp.GetPixel(x,y);
                int r = c.R, g = c.G, b = c.B;
                
                if (g > r + 30 && g > b + 30) {
                    int diff = g - Math.Max(r, b);
                    int alpha = 255 - diff * 3;
                    if (alpha < 0) alpha = 0;
                    
                    if (alpha > 0) {
                        int newG = Math.Min(g, Math.Max(r, b) + 10);
                        target.SetPixel(x, y, Color.FromArgb(alpha, r, newG, b));
                        
                        if(x < minX) minX = x;
                        if(y < minY) minY = y;
                        if(x > maxX) maxX = x;
                        if(y > maxY) maxY = y;
                    } else {
                        target.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                    }
                } else {
                    target.SetPixel(x, y, Color.FromArgb(255, r, g, b));
                    if(x < minX) minX = x;
                    if(y < minY) minY = y;
                    if(x > maxX) maxX = x;
                    if(y > maxY) maxY = y;
                }
            }
        }
        
        if (minX <= maxX && minY <= maxY) {
            Rectangle cropRect = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
            Bitmap cropped = new Bitmap(cropRect.Width, cropRect.Height, PixelFormat.Format32bppArgb);
            using(Graphics gr = Graphics.FromImage(cropped)){
                gr.DrawImage(target, new Rectangle(0, 0, cropped.Width, cropped.Height), cropRect, GraphicsUnit.Pixel);
            }
            cropped.Save(args[1], ImageFormat.Png);
        } else {
            target.Save(args[1], ImageFormat.Png);
        }
    }
}
