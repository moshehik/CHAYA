from PIL import Image
import sys

img_path = sys.argv[1]
out_path = sys.argv[2]

img = Image.open(img_path).convert("RGBA")
pixels = img.load()

for y in range(img.height):
    for x in range(img.width):
        r, g, b, a = pixels[x, y]
        # if green dominates
        if g > r + 30 and g > b + 30:
            diff = g - max(r, b)
            alpha = max(0, 255 - diff * 3)
            if alpha == 0:
                pixels[x, y] = (0, 0, 0, 0)
            else:
                # To remove the green spill, we clamp green to the average of R and B
                avg = (r + b) // 2
                pixels[x, y] = (r, avg, b, int(alpha))
                
img.save(out_path, "PNG")
print("Saved transparent PNG to", out_path)
