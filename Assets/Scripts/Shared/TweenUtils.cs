using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Droppy.Shared
{
    public static class TweenUtils
    {
        public static IEnumerator InterpolateAlpha(this Image image, float initialAlpha, float finalAlpha, float duration)
        {
            yield return InterpolateValue(SetAlphaInterpolatedValue, duration);
            yield break;
            
            void SetAlphaInterpolatedValue(float t)
            {
                image.SetAlpha(Mathf.Lerp(initialAlpha, finalAlpha, t));
            }
        }
        
        public static IEnumerator InterpolateValue(Action<float> lerp, float duration)
        {
            float timer = 0.0f;
        
            lerp(0.0f);
        
            while (timer < duration)
            {
                float t = timer / duration;
                lerp(Mathf.Clamp01(t));

                timer += Time.deltaTime;
                yield return null;
            }
        
            lerp(1.0f);
        }
    }
}