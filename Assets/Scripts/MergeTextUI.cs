using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeTextUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;

    [Space]
    [SerializeField] List<string> mergeText;
    [SerializeField] float maxTimeBetweenMerge;

    public UnityEngine.Events.UnityEvent feedback;

    float _lastMerge;
    int mergeTextCount;

    public void OnMerge()
    {
        if (_lastMerge + maxTimeBetweenMerge > Time.time)
        {
            text.SetText(mergeText[mergeTextCount]);
            if(mergeTextCount < mergeText.Count - 1) mergeTextCount++;
            feedback.Invoke();
        }
        else
            mergeTextCount = 0;

        _lastMerge = Time.time;

    }
}
