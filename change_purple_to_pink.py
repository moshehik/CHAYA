from PIL import Image
import colorsys
import sys

def shift_hue(img_path, out_path):
    img = Image.open(img_path).convert("RGBA")
    pixels = img.load()
    width, height = img.size

    for y in range(height):
        for x in range(width):
            r, g, b, a = pixels[x, y]
            
            # Convert RGB to HSV
            # colorsys expects values in 0.0 - 1.0
            h, s, v = colorsys.rgb_to_hsv(r/255.0, g/255.0, b/255.0)
            
            # Purple hue is roughly between 260 and 290 degrees (in 0.0-1.0 that's 0.72 - 0.85)
            # Pink hue is roughly between 310 and 340 degrees (in 0.0-1.0 that's 0.86 - 0.94)
            # We also check saturation and value to avoid changing greys/whites/blacks
            if 0.70 <= h <= 0.85 and s > 0.15 and v > 0.15:
                # Shift hue towards pink (around 0.92)
                # You can adjust this shift value based on the exact pink desired
                new_h = 0.92
                # Convert back to RGB
                nr, ng, nb = colorsys.hsv_to_rgb(new_h, s, v)
                pixels[x, y] = (int(nr*255), int(ng*255), int(nb*255), a)
                
    # Save as JPEG (we need to convert back to RGB first)
    if out_path.lower().endswith(".jpg") or out_path.lower().endswith(".jpeg"):
        img = img.convert("RGB")
        img.save(out_path, "JPEG", quality=95)
    else:
        img.save(out_path, "PNG")

if __name__ == "__main__":
    shift_hue(sys.argv[1], sys.argv[2])
    print(f"Processed {sys.argv[1]} and saved to {sys.argv[2]}")
