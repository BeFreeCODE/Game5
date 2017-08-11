using UnityEngine;
using System.Collections;

public enum GameState
{
    main,
    game,
    over,
    store,
    store2,
    ready
}



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState curGameState = GameState.main;

    public JsonData jsonData;
    public Player player;

    //점수, 자원
    public int curScore = 0;
    public int topScore = 0;
    public int coin;

    public int redLoot, greenLoot, blueLoot;
    public int redCoin, blueCoin, greenCoin;
    public int[] redLevel = new int[7];
    public int[] blueLevel = new int[7];
    public int[] greenLevel = new int[7];

    //스테이지
    public int stageNum = 0;
    public float stageCurTime = 0f;
    public float stageLimitTime = 30f;

    public bool enemyRend = false;
    private bool overState = false;

    [SerializeField]
    private GameObject quitPop;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        //데이터 불러옴
        DataManager.Instance.GetData();

        enemyRend = true;
    }

    private void Start()
    {
        //게임시작 시 시간정지
        SetTimeScale(1f);
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
        EnemyManager.instance.InitEnemyData();

        //한계점 원상복귀
        EnemyManager.instance.touchManager.limitDistance = 30f;

        //bulletInit
        BulletManager.instance.InitObjs();

        //ItemInit
        ItemManager.instance.InitObjs();

        //Player,CamInit
        player.InitPlayer();

        overState = true;
    }

    //시간조정
    public void SetTimeScale(float _scale)
    {
        Time.timeScale = _scale;
    }

    //플레이어 타입 세팅
    public void SetPlayerType(int _num)
    {
        player.ChangePlayerType(_num);
    }

    //상태전환
    public void StateTransition(GameState _state)
    {
        curGameState = _state;
    }

    //점수획득
    public void AddScore(int num)
    {
        curScore += num;

        if (topScore <= curScore)
        {
            topScore = curScore;
        }
    }

    public bool quitActive = false;

    public void BackButton()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (GameManager.instance.curGameState)
            {
                case GameState.main:
                    if (!quitActive)
                    {
                        quitPop.SetActive(true);
                        quitActive = true;
                    }
                    break;
                case GameState.game:
                    break;
                case GameState.over:
                    break;
                case GameState.ready:
                    break;
                case GameState.store:
                    break;
                case GameState.store2:
                    break;
            }
        }
    }
    public void YesButton()
    {
        Application.Quit();
    }
    public void NoButton()
    {
        if (quitActive)
        {
            quitPop.SetActive(false);
            quitActive = false;
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
                    SoundManager.instance.StopBGMSound();
                    SoundManager.instance.PlayEffectSound(3);

                    InitGame();
                    DataManager.Instance.SetData();

                }
                break;
            case GameState.store:
                SetTimeScale(1);
                break;
            case GameState.store2:
                SetTimeScale(1);
                break;
            case GameState.ready:
                BulletManager.instance.InitObjs();
                break;
        }
    }

    private void Update()
    {
        Game();
        BackButton();
    }
}
