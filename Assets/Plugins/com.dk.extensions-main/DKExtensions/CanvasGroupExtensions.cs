using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static partial class CanvasGroupExtensions
{
    /// <summary>Animate CanvasGroup's alpha</summary>
    /// <param name="endValue">Value of CanvasGroup.alpha at the end of animation.</param>
    /// <param name="animTime">Animation time.</param>
    /// <param name="animTime">Reduse animation time if it was in between 0 and 1.</param>
    public static async void AnimateAlpha(this CanvasGroup target, float endValue, float animTime, bool adjustAnimTime = true)
    {
        //if target.alpha was between 0 and 1 lowering the time it takes to animate 
        if (adjustAnimTime)
            animTime *= Mathf.InverseLerp(0, animTime, Mathf.Abs(endValue - target.alpha));

        float elapsed = 0;
        float elapsedNormal = 0;

        while (elapsed < animTime)
        {
            elapsedNormal = Mathf.InverseLerp(0, animTime, elapsed);
            target.alpha = Mathf.Lerp(target.alpha, endValue, elapsedNormal);
            await UniTask.NextFrame();
            elapsed += Time.deltaTime;
        }
        target.alpha = endValue;
    }
}
