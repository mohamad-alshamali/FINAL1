# bone_analyzer.py
import cv2
import numpy as np
import sys
import os

def analyze_bone(image_path):
    if not os.path.exists(image_path):
        return f"Error: Image not found at {image_path}"
    
    try:
        img = cv2.imread(image_path)
        gray_image = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        gray_image = cv2.resize(gray_image, (600, 600))
        blurred = cv2.GaussianBlur(gray_image, (5, 5), 0)
        processed_edges = cv2.Canny(blurred, 50, 150)
        
        edge_pixels = np.sum(processed_edges > 0)
        bone_core_density = np.mean(gray_image)
        
        if edge_pixels > 25000:
            if bone_core_density < 110:
                return "Fracture_Detected" # تم رصد كسر حديث
            else:
                return "Bone_Healing" # العظم في مرحلة الشفاء
        else:
            if bone_core_density > 130:
                return "Normal_Bone" # العظم سليم
            else:
                return "Low_Density" # هشاشة عظام
    except Exception as e:
        return f"Error: {str(e)}"

if __name__ == "__main__":
    # استقبال مسار الصورة الممرر من تطبيق C#
    if len(sys.argv) > 1:
        image_path_input = sys.argv[1]
        result = analyze_bone(image_path_input)
        print(result) # طباعة النتيجة لتقرأها واجهة C#
