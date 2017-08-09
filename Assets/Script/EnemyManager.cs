using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : ObjectManager
{
    public static EnemyManager instance;

    public TouchManager touchManager;

    private float rendDeleyTime = 1f;

    //적 Tpye에따른 시간
    private float normalTime = 0f, speederTime = 0f, tankerTime = 0f, laserTime = 0f;
    private float x, y;

    [SerializeField]
    private float maxRange, minRange;

    [SerializeField]
    private GameObject warningMark;

    private List<GameObject> warningList = new List<GameObject>();

    public Camera uiCam;

    public bool bossState = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        MakeObjs(this.makeObj[0]);

        for (int i = 0; i < 200; i++)
        {
            GameObject _warning = Instantiate(warningMark);
            _warning.transform.parent = this.transform;
            _warning.SetActive(false);

            warningList.Add(_warning);
        }
    }

    public void InitEnemyData()
    {
        bossState = false;
        GameManager.instance.enemyRend = true;

        GameManager.instance.stageCurTime = 0f;

        normalTime = 0f;
        speederTime = 0f;
        tankerTime = 0f;
        laserTime = 0f;
    }

    //Warning 오브젝트
    private GameObject GetWarning()
    {
        foreach (GameObject _warning in warningList)
        {
            if (!_warning.activeInHierarchy)
            {
                return _warning;
            }
        }
        return null;
    }

    //Enemy Render
    public void RendEnemy()
    {
        if (GameManager.instance.enemyRend)
        {
            //stage Time
            GameManager.instance.stageCurTime += Time.deltaTime;

            //현재 stage시간이 limit타임이 되면 더이상 적을 생성하지 않는다.
            if (GameManager.instance.stageCurTime >= GameManager.instance.stageLimitTime)
            {
                GameManager.instance.enemyRend = false;
            }

            normalTime += Time.deltaTime;
            speederTime += Time.deltaTime;
            tankerTime += Time.deltaTime;
            laserTime += Time.deltaTime;

            //Enemy 타입별로 시간비율설정
            if (normalTime >= rendDeleyTime)
            {
                GameObject newEnemy = GetObj();

                if (newEnemy)
                {
                    //위치지정
                    SetRandomPos();
                    newEnemy.transform.position = new Vector3(x, y, 0);

                    newEnemy.GetComponent<Enemy>().SetType(EnemyType.normal);
                    newEnemy.SetActive(true);

                    normalTime = 0f;
                }
            }
            if (speederTime >= rendDeleyTime * 3f)
            {
                GameObject newEnemy = GetObj();

                if (newEnemy)
                {
                    //위치지정
                    SetRandomPos();
                    newEnemy.transform.position = new Vector3(x, y, 0);

                    newEnemy.GetComponent<Enemy>().SetType(EnemyType.speeder);
                    newEnemy.SetActive(true);

                    speederTime = 0f;
                }
            }
            if (tankerTime >= rendDeleyTime * 2f)
            {
                GameObject newEnemy = GetObj();

                if (newEnemy)
                {
                    //위치지정
                    SetRandomPos();
                    newEnemy.transform.position = new Vector3(x, y, 0);

                    newEnemy.GetComponent<Enemy>().SetType(EnemyType.tanker);
                    newEnemy.SetActive(true);

                    tankerTime = 0f;
                }
            }
            if (laserTime >= rendDeleyTime * 10f)
            {
                int repeatNum = Random.Range(1, 6);
                int dirNum = Random.Range(1, 5);

                for (int i = 0; i < repeatNum; i++)
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        if (dirNum == 1)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x + (i * 2) - repeatNum;
                            y = player.transform.position.y + 10f;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(i * 2 - repeatNum, -y, 0);

                            for (int j = -20; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x, y + j, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().Play();
                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                        else if (dirNum == 2)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x + (i * 2) - repeatNum;
                            y = player.transform.position.y - 10f;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(i * 2 - repeatNum, -y, 0);

                            for (int j = -20; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x, y - j, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().Play();
                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                        else if (dirNum == 3)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x + 20f;
                            y = player.transform.position.y + (i * 2) - repeatNum;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(-x, i * 2 - repeatNum, 0);

                            for (int j = -40; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x + j, y, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().Play();

                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                        else if (dirNum == 4)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x - 20f;
                            y = player.transform.position.y + (i * 2) - repeatNum;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(-x, i * 2 - repeatNum, 0);

                            for (int j = -40; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x - j, y, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().Play();
                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                    }
                    if (i == repeatNum - 1)
                    {
                        laserTime = 0f;
                    }
                }
            }
        }

        //Boss
        else
        {
            if (!bossState)
            {
                GameObject newEnemy = GetObj();

                if (newEnemy)
                {
                    //위치지정
                    SetRandomPos();
                    newEnemy.transform.position = new Vector3(x, y, 0);

                    newEnemy.GetComponent<Enemy>().SetType(EnemyType.boss);
                    newEnemy.SetActive(true);

                    bossState = true;
                }
            }
            //보스 등장 시 라인 줄어들음
            touchManager.limitDistance -= Time.deltaTime;

            //stage 시간이 다 지났을 때 적을 검사
            if (!CheckObj())
            {
                //Next Stage
                GameManager.instance.enemyRend = true;

                bossState = false;

                touchManager.limitDistance = 30f;

                GameManager.instance.stageNum++;
                GameManager.instance.stageCurTime = 0f;
                GameManager.instance.stageLimitTime += 10f;

                //적이 없을시(스테이지 클리어시) ready 상태로 전환
                GameManager.instance.StateTransition(GameState.ready);
            }
        }
    }
    
    private bool CheckObj()
    {
        return GameObject.FindWithTag("Enemy");
    }

    //x,y 위치지정
    private void SetRandomPos()
    {
        x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
        y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);

        while (Mathf.Abs(x - player.transform.position.x) <= minRange &&
                Mathf.Abs(y - player.transform.position.y) <= minRange)
        {
            x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
            y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);
        }
    }
}
