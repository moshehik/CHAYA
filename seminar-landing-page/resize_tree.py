import cv2
import numpy as np
import os

input_file = "תמונה לאזור 1.jpg"
output_file = "תמונה לאזור 1_new.jpg"

try:
    with open(input_file, "rb") as f:
        chunk = f.read()
    chunk_arr = np.frombuffer(chunk, dtype=np.uint8)
    img = cv2.imdecode(chunk_arr, cv2.IMREAD_COLOR)

    h, w = img.shape[:2]

    # Shrink by 10% (scale 0.90) to keep it large but fit the canopy
    scale = 0.90
    new_w, new_h = int(w * scale), int(h * scale)
    resized = cv2.resize(img, (new_w, new_h), interpolation=cv2.INTER_AREA)

    # Distribute border: more space on top for the canopy
    top = int((h - new_h) * 0.6)
    bottom = h - new_h - top
    left = (w - new_w) // 2
    right = w - new_w - left

    # Replicate border pixels
    padded = cv2.copyMakeBorder(resized, top, bottom, left, right, cv2.BORDER_REPLICATE)
    
    # Optional: apply slight blur to the borders to make the replication less obvious
    # We will just apply a blur to the whole image mask-based or leave it. BORDER_REPLICATE usually looks like a stretched canvas edge.

    is_success, im_buf_arr = cv2.imencode(".jpg", padded)
    if is_success:
        im_buf_arr.tofile(output_file)
        print("Successfully created optimized image.")
    else:
        print("Failed to encode image.")
except Exception as e:
    print(f"Error: {e}")
