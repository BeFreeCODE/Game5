using UnityEngine;
using System.Collections;

public enum GameState
{
    main,
    game,
    over
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState curGameState = GameState.main;
    
    public Player player;

    public int curScore = 0;
    public int topScore = 0;

    private bool overState = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        //게임시작 시 시간정지
        SetTimeScale(0f);

        //데이터 불러옴
        DataManager.Instance.GetData();

        Input.multiTouchEnabled = false;
    }

    private void OnApplicationQuit()
    {
        //종료시 데이터 저장
        DataManager.Instance.SetData();
    }

    //게임 초기세팅
    public void InitGame()
    {
        //EnemyInit
        EnemyManager.instance.InitObjs();

        //bulletInit
        BulletManager.instance.InitObjs();

        //ItemInit
        ItemManager.instance.InitObjs();

        //Player,CamInit
        player.InitPlayer();
        
    }

    //시간조정
    public void SetTimeScale(float _scale)
    {
        Time.timeScale = _scale;
    }

    //상태전환
    public void StateTransition(GameState _state)
    {
        curGameState = _state;
    }

    public void AddScore(int num)
    {
        curScore += num;

        if(topScore <= curScore)
        {
            topScore = curScore;
        }
    }

    private void Game()
    {
        switch (curGameState)
        {
            case GameState.main:
                curScore = 0;
                overState = false;
                break;
            case GameState.game:
                EnemyManager.instance.RendEnemy();
                ItemManager.instance.RendItem();
                overState = false;
                break;
            case GameState.over:
                if (!overState)
                {
                    InitGame();
                    DataManager.Instance.SetData();
                    overState = true;
                }
                break;
        }
    }

    private void Update()
    {
        Game();
    }
}
