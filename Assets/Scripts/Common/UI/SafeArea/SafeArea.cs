﻿namespace Evu.Common.UI
{
    using UnityEngine;

    public class SafeArea : MonoBehaviour
    {
        RectTransform Panel;
        Rect LastSafeArea = new Rect(0, 0, 0, 0);
        Vector2Int LastScreenSize = new Vector2Int(0, 0);
        ScreenOrientation LastOrientation = ScreenOrientation.AutoRotation;
        [SerializeField] bool ConformX = true;  // Conform to screen safe area on X-axis (default true, disable to ignore)
        [SerializeField] bool ConformY = true;  // Conform to screen safe area on Y-axis (default true, disable to ignore)
        [SerializeField] bool Logging = false;  // Conform to screen safe area on Y-axis (default true, disable to ignore)

        void Awake()
        {
            Panel = GetComponent<RectTransform>();

            if (Panel == null)
            {
                Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
                Destroy(gameObject);
            }

            Refresh();
        }

        void Update()
        {
            Refresh();
        }

        void Refresh()
        {
            Rect safeArea = Screen.safeArea;

            if (safeArea != LastSafeArea
                || Screen.width != LastScreenSize.x
                || Screen.height != LastScreenSize.y
                || Screen.orientation != LastOrientation)
            {
                // Fix for having auto-rotate off and manually forcing a screen orientation.
                // See https://forum.unity.com/threads/569236/#post-4473253 and https://forum.unity.com/threads/569236/page-2#post-5166467
                LastScreenSize.x = Screen.width;
                LastScreenSize.y = Screen.height;
                LastOrientation = Screen.orientation;

                ApplySafeArea(safeArea);
            }
        }

        void ApplySafeArea(Rect r)
        {
            LastSafeArea = r;

            // Ignore x-axis?
            if (!ConformX)
            {
                r.x = 0;
                r.width = Screen.width;
            }

            // Ignore y-axis?
            if (!ConformY)
            {
                r.y = 0;
                r.height = Screen.height;
            }

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            Panel.anchorMin = anchorMin;
            Panel.anchorMax = anchorMax;

            if (Logging)
            {
                Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
            }
        }
    }
}
