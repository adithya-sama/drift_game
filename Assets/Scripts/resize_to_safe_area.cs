using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class resize_to_safe_area : MonoBehaviour
{
    void Awake(){
        RectTransform safeAreaTransform = GetComponent<RectTransform>();
        Canvas canvas = GetComponentInParent<Canvas>();

        Rect safeArea = Screen.safeArea;
   
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;
   
        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;
    }
}