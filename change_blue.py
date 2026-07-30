from PIL import Image
import colorsys
import sys

def enhance_blue(img_path, out_path):
    img = Image.open(img_path).convert("RGBA")
    pixels = img.load()
    width, height = img.size

    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            if a == 0:
                continue
            
            h, s, v = colorsys.rgb_to_hsv(r/255.0, g/255.0, b/255.0)
            
            # Identify blue/cyan hues (roughly 0.45 to 0.7)
            if 0.45 <= h <= 0.7 and s > 0.05:
                # Make it more vibrant (increase saturation)
                s = min(1.0, s * 1.5)
                # Make it slightly darker (decrease value)
                v = max(0.0, v * 0.85)
                # Optionally shift hue slightly towards a richer blue if it's too cyan
                if h < 0.55:
                    h = h + 0.02
                
                nr, ng, nb = colorsys.hsv_to_rgb(h, s, v)
                pixels[x, y] = (int(nr*255), int(ng*255), int(nb*255), a)
                
    img.save(out_path, "PNG")

if __name__ == "__main__":
    enhance_blue(sys.argv[1], sys.argv[2])
    print(f"Processed {sys.argv[1]} and saved to {sys.argv[2]}")
