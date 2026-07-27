import cv2
import numpy as np
import os

input_file = "תמונה 7.png"
output_file = "תמונה 7_new.png"

try:
    with open(input_file, "rb") as f:
        chunk = f.read()
    chunk_arr = np.frombuffer(chunk, dtype=np.uint8)
    # Read with alpha if present
    img = cv2.imdecode(chunk_arr, cv2.IMREAD_UNCHANGED)

    h, w = img.shape[:2]

    # Shrink to 70% to make it noticeably smaller
    scale = 0.70
    new_w, new_h = int(w * scale), int(h * scale)
    resized = cv2.resize(img, (new_w, new_h), interpolation=cv2.INTER_AREA)

    # Distribute border evenly
    top = (h - new_h) // 2
    bottom = h - new_h - top
    left = (w - new_w) // 2
    right = w - new_w - left

    # Replicate border pixels
    padded = cv2.copyMakeBorder(resized, top, bottom, left, right, cv2.BORDER_REPLICATE)

    is_success, im_buf_arr = cv2.imencode(".png", padded)
    if is_success:
        im_buf_arr.tofile(output_file)
        print("Successfully created optimized image:", output_file)
    else:
        print("Failed to encode image.")
except Exception as e:
    print(f"Error: {e}")
