using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

class Program {
    static bool IsSimilar(Color c, Color target, int threshold) {
        return Math.Abs(c.R - target.R) < threshold &&
               Math.Abs(c.G - target.G) < threshold &&
               Math.Abs(c.B - target.B) < threshold;
    }

    static void Main(string[] args) {
        if (args.Length < 2) return;
        Bitmap bmp = new Bitmap(args[0]);
        int w = bmp.Width;
        int h = bmp.Height;
        
        bool[,] visited = new bool[w, h];
        Queue<Point> q = new Queue<Point>();
        
        Color bg = bmp.GetPixel(0, 0);
        
        Point[] starts = new Point[] { new Point(0,0), new Point(w-1,0), new Point(0,h-1), new Point(w-1,h-1), new Point(w/2, h/2) };
        foreach(Point p in starts) {
            if(!visited[p.X, p.Y]) {
                q.Enqueue(p);
                visited[p.X, p.Y] = true;
            }
        }
        
        int threshold = 30;

        while(q.Count > 0) {
            Point p = q.Dequeue();
            int x = p.X;
            int y = p.Y;
            
            bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
            
            Point[] neighbors = new Point[] { new Point(x+1, y), new Point(x-1, y), new Point(x, y+1), new Point(x, y-1) };
            foreach(Point n in neighbors) {
                if(n.X >= 0 && n.X < w && n.Y >= 0 && n.Y < h) {
                    if(!visited[n.X, n.Y]) {
                        if(IsSimilar(bmp.GetPixel(n.X, n.Y), bg, threshold)) {
                            visited[n.X, n.Y] = true;
                            q.Enqueue(n);
                        }
                    }
                }
            }
        }
        
        int minX = w, minY = h, maxX = 0, maxY = 0;
        for(int y=0; y<h; y++) {
            for(int x=0; x<w; x++) {
                if(bmp.GetPixel(x, y).A > 0) {
                    if(x < minX) minX = x;
                    if(y < minY) minY = y;
                    if(x > maxX) maxX = x;
                    if(y > maxY) maxY = y;
                }
            }
        }
        
        if (minX <= maxX && minY <= maxY) {
            minX = Math.Max(0, minX - 10);
            minY = Math.Max(0, minY - 10);
            maxX = Math.Min(w - 1, maxX + 10);
            maxY = Math.Min(h - 1, maxY + 10);
            
            Rectangle cropRect = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
            Bitmap cropped = new Bitmap(cropRect.Width, cropRect.Height, PixelFormat.Format32bppArgb);
            using(Graphics gr = Graphics.FromImage(cropped)){
                gr.DrawImage(bmp, new Rectangle(0, 0, cropped.Width, cropped.Height), cropRect, GraphicsUnit.Pixel);
            }
            cropped.Save(args[1], ImageFormat.Png);
        } else {
            bmp.Save(args[1], ImageFormat.Png);
        }
    }
}
