using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] BallController ballPrefab;
    [SerializeField] float scaleTime = .25f;

    public BallController SpawnBall(Vector3 pos, int scaleIndex)
    {
        var ball = Instantiate(ballPrefab, pos, Quaternion.identity);
        ball.SetScale(scaleIndex);

        ball.transform.localScale = Vector3.zero;
        ball.transform.DOScale(1f, scaleTime);

        return ball;
    }
}
