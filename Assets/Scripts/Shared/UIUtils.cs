using UnityEngine;
using UnityEngine.UI;

namespace Droppy.Shared
{
    public static class UIUtils
    {
        public static void SetAlpha(this Image image, float alpha)
        {
            Color fadeImageColor = image.color;
            fadeImageColor.a = alpha;
            image.color = fadeImageColor;
        }
    }
}