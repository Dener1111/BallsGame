using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Level,
    Finish
}
public class GameController : MonoBehaviour
{
    [SerializeField] float endScreenTime = 3f;

    GameState _current;

    public GameState Current
    {
        get => _current;
        set
        {
            _current = value;
            onStateChange.Invoke(_current);
        }
    }

    public static UnityEngine.Events.UnityEvent<GameState> onStateChange = new();

    void Start()
    {
        BallManager.onLastBall.AddListener(OnLastBall);
    }

    async void OnLastBall(BallController _)
    {
        if(_current == GameState.Finish) return;

        _current = GameState.Finish;

        await this.WaitSeconds(endScreenTime);

       Reload();
    }

    void Reload()
    {
        SceneManager.LoadSceneAsync(0);
    }

    void OnDestroy()
    {
        onStateChange.RemoveAllListeners();
    }
}
