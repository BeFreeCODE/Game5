using UnityEngine;
using System.Collections;

public enum GameState
{
    main,
    game,
    over
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameState curGameState = GameState.main;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        //게임시작 시 시간정지
        SetTimeScale(0f);
    }

    private void Update()
    {
        Game();
    }

    private void Game()
    {
        switch(curGameState)
        {
            case GameState.main:
                break;
            case GameState.game:
                break;
            case GameState.over:
                break;
        }
    }

    public void SetTimeScale(float _scale)
    {
        Time.timeScale = _scale;
    }
}
