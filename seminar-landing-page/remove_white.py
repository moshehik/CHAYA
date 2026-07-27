from PIL import Image
import numpy as np

def remove_bg(input_path, output_path, threshold=220):
    img = Image.open(input_path).convert("RGBA")
    data = np.array(img)
    
    # Calculate luminance or just check if R, G, B are all > threshold
    r, g, b, a = data.T
    white_areas = (r > threshold) & (g > threshold) & (b > threshold)
    
    # Make white areas transparent
    data[..., 3][white_areas.T] = 0
    
    img2 = Image.fromarray(data)
    img2.save(output_path)

remove_bg("image_3_new.jpg", "image_3_transparent.png", threshold=230)
