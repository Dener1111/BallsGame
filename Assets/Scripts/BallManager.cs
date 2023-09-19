using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using DG.Tweening;

public class BallManager : MonoBehaviour
{
    [SerializeField] BallSettings settings;
    public BallSpawner spawner;

    [Space]
    [SerializeField] int maxStartBallCount = 4;
    [SerializeField] int maxPlayerScale = 7;

    [Space]
    [SerializeField] Vector2 spawnZonePos;
    [SerializeField] Vector2 spawnZoneSize;

    List<BallController> balls = new();

    public static UnityEngine.Events.UnityEvent<BallController> onLastBall = new();


    public static BallManager inst;

    void Start()
    {
        inst = this;

        BallController.onSpawn.AddListener(AddBall);
        BallController.onDespawn.AddListener(RemoveBall);

        PopulateGameField();
    }

    public void PopulateGameField()
    {
        int count = Random.Range(1, maxStartBallCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = spawnZonePos + new Vector2(spawnZoneSize.x.GetRandomSign(), spawnZoneSize.y.GetRandomSign());
            var ball = spawner.SpawnBall(pos, i);
            ball.Spawned();
        }
    }

    public BallController SpawnPlayerBall(Vector3 pos)
    {
        return spawner.SpawnBall(pos, GetRandomScale());
    }

    int GetRandomScale()
    {
        int scale = balls.Max(x => x.GetScale());//getting max ball size on a scene
        scale = Mathf.Min(scale, maxPlayerScale);//clamping to max possible scale
        scale = scale.GetRandom();//getting random scale between possible
        return scale;
    }

    public void AddBall(BallController ball)
    {
        balls.Add(ball);
    }

    public void RemoveBall(BallController ball)
    {
        balls.Remove(ball);
        CheckMaxSize();
    }

    void CheckMaxSize()
    {
        var maxBall = balls.FirstOrDefault(x => x.GetScale() == settings.MaxScaleIndex());
        if (maxBall)
        {
            foreach (var item in balls)
            {
                item.Freeze();
                item.transform.position = maxBall.transform.position;
                item.transform.DOMove(transform.position, .5f);
            }

            onLastBall.Invoke(maxBall);
        }
    }

    void OnDestroy()
    {
        onLastBall.RemoveAllListeners();

        BallController.onSpawn.RemoveListener(AddBall);
        BallController.onDespawn.RemoveListener(RemoveBall);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnZonePos, spawnZoneSize * 2);
    }
}
