using System;
using System.Drawing;
using System.Drawing.Imaging;

class Program {
    static void Main(string[] args) {
        if (args.Length < 2) return;
        Bitmap bmp = new Bitmap(args[0]);
        int minX = bmp.Width, minY = bmp.Height, maxX = 0, maxY = 0;
        
        for(int y=0; y<bmp.Height; y++){
            for(int x=0; x<bmp.Width; x++){
                Color c = bmp.GetPixel(x,y);
                if(c.A > 10){ // non-transparent
                    if(x < minX) minX = x;
                    if(y < minY) minY = y;
                    if(x > maxX) maxX = x;
                    if(y > maxY) maxY = y;
                }
            }
        }
        
        if (minX <= maxX && minY <= maxY) {
            Rectangle cropRect = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height, PixelFormat.Format32bppArgb);
            using(Graphics g = Graphics.FromImage(target)){
                g.DrawImage(bmp, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }
            target.Save(args[1], ImageFormat.Png);
        } else {
            bmp.Save(args[1], ImageFormat.Png);
        }
    }
}
