using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] BallController ballPrefab;

    public BallController SpawnBall(Vector3 pos, int scaleIndex)
    {
        var ball = Instantiate(ballPrefab, pos, Quaternion.identity);
        ball.SetScale(scaleIndex);

        return ball;
    }
}
