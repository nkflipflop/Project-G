using UnityEngine;

namespace Utilities
{
    public static partial class Extensions
    {
        private static Vector3[] corners = new Vector3[4];
        public static Vector2 GetBottomPosition(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(corners);
            return (corners[0] + corners[3]) / 2f;
        }
	
        public static Vector2 GetTopPosition(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(corners);
            return (corners[1] + corners[2]) / 2f;
        }
	
        public static Vector2 GetLeftPosition(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(corners);
            return (corners[0] + corners[3]) / 2f;
        }
	
        public static Vector2 GetRightPosition(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(corners);
            return (corners[1] + corners[2]) / 2f;
        }
        
        public static float GetWorldWidth(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(corners);
            return Mathf.Abs((corners[2] - corners[1]).x);
        }
	
        public static float GetWorldHeight(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(corners);
            return Mathf.Abs((corners[3] - corners[1]).y);
        }
		
        // <summary> Set pivot without changing the position of the element </summary>
        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            Vector3 deltaPosition = rectTransform.pivot - pivot;            // get change in pivot
            deltaPosition.Scale(rectTransform.rect.size);                   // apply sizing
            deltaPosition.Scale(rectTransform.localScale);                  // apply scaling
            deltaPosition = rectTransform.localRotation * deltaPosition;    // apply rotation

            rectTransform.pivot = pivot;                                    // change the pivot
            rectTransform.localPosition -= deltaPosition;                   // reverse the position change
        }
        
        public static void SetLocalPositionX(this RectTransform rectTransform, float x)
        {
            Vector3 position = rectTransform.localPosition;
            position.x = x;
            rectTransform.localPosition = position;
        }

        public static void SetLocalPositionY(this RectTransform rectTransform, float y)
        {
            Vector3 position = rectTransform.localPosition;
            position.y = y;
            rectTransform.localPosition = position;
        }

        public static void SetLocalPositionZ(this RectTransform rectTransform, float z)
        {
            Vector3 position = rectTransform.localPosition;
            position.z = z;
            rectTransform.localPosition = position;
        }

        public static void SetAnchoredPositionX(this RectTransform rect, float x)
        {
            Vector2 position = rect.anchoredPosition;
            position.x = x;
            rect.anchoredPosition = position;
        }
	
        public static void SetAnchoredPositionY(this RectTransform rect, float y)
        {
            Vector2 position = rect.anchoredPosition;
            position.y = y;
            rect.anchoredPosition = position;
        }
    }
}