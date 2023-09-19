using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] Transform spawnPos;

    [Space]
    [SerializeField] float launchForce = 10f;
    [SerializeField] float spawnWait = .5f;

    bool _canControl = true;
    BallController _currentBall;


    void Start()
    {
        spawnPos ??= transform;

        GameController.onStateChange.AddListener(OnGameStateChange);

        SpawnBall();
    }

    void Update()
    {
        PlayerInput();
    }

    void OnGameStateChange(GameState state)
    {
        if (state != GameState.Finish) return;

        _canControl = false;

        if (_currentBall)
            Destroy(_currentBall.gameObject);
    }

    void PlayerInput()
    {
        if (!_canControl) return;

        Vector3 dir = GetDirection();
        line.transform.rotation = Quaternion.LookRotation(Vector3.forward, -dir);

        if (Input.GetMouseButtonDown(0))
        {
            line.enabled = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;

            if (!_currentBall) return;

            LaunchBall(dir);
            SpawnBall();
        }
    }

    Vector3 GetDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return spawnPos.position.Direction(worldPosition);
    }

    async void SpawnBall()
    {
        await this.WaitSeconds(spawnWait);

        if (!_canControl) return;

        _currentBall = BallManager.inst.SpawnPlayerBall(spawnPos.position);
        _currentBall.Freeze();
    }

    void LaunchBall(Vector3 dir)
    {
        _currentBall.Launch(dir, launchForce);
        _currentBall.Spawned();
        _currentBall = null;
    }

    void OnDestroy()
    {
        GameController.onStateChange.RemoveListener(OnGameStateChange);
    }

    void OnDrawGizmos()
    {
        if (!spawnPos) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPos.position, .5f);
    }
}
