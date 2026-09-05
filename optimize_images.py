import os
from PIL import Image

def optimize_image(filepath):
    try:
        original_size = os.path.getsize(filepath)
        if original_size < 100 * 1024:
            # Skip images smaller than 100KB, they are already small
            return

        with Image.open(filepath) as img:
            format = img.format
            
            # Convert RGBA to RGB for JPEG if needed, though we will save back in same format
            if format == 'JPEG' and img.mode in ('RGBA', 'P'):
                img = img.convert('RGB')
            
            # Resize if too large (e.g. width > 1920)
            max_width = 1920
            if img.width > max_width:
                ratio = max_width / float(img.width)
                new_height = int((float(img.height) * float(ratio)))
                img = img.resize((max_width, new_height), Image.Resampling.LANCZOS)
                
            # Save optimized
            if format == 'PNG':
                img.save(filepath, format='PNG', optimize=True)
            elif format == 'JPEG':
                img.save(filepath, format='JPEG', optimize=True, quality=80)
                
        new_size = os.path.getsize(filepath)
        print(f"Optimized {filepath}: {original_size/1024:.1f}KB -> {new_size/1024:.1f}KB")
    except Exception as e:
        print(f"Failed to optimize {filepath}: {e}")

def process_directory(directory):
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.lower().endswith(('.png', '.jpg', '.jpeg')):
                filepath = os.path.join(root, file)
                optimize_image(filepath)

if __name__ == "__main__":
    process_directory(".")
