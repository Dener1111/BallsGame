using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BallInfo
{
    public Color color;
    public float scale;
}

[CreateAssetMenu(fileName = "BallSettings", menuName = "ScriptableObjects/BallSettings", order = 1)]
public class BallSettings : ScriptableObject
{
    public List<BallInfo> balls;

    public int MaxScaleIndex() => balls.Count - 1;
}
