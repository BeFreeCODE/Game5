using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : ObjectManager
{
    public static EnemyManager instance;

    private float rendDeleyTime = 1f;

    //적 Tpye에따른 시간
    private float normalTime = 0f, speederTime = 0f, tankerTime = 0f, bossTime = 0f, laserTime = 0f;
    private float x, y;

    [SerializeField]
    private float maxRange, minRange;

    [SerializeField]
    private GameObject warningMark;

    private List<GameObject> warningList = new List<GameObject>();

    public Camera uiCam;

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
        normalTime += Time.deltaTime;
        speederTime += Time.deltaTime;
        tankerTime += Time.deltaTime;
        bossTime += Time.deltaTime;
        laserTime += Time.deltaTime;
        ReduceRendTime();

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
        if (speederTime >= rendDeleyTime * 5f)
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
                    else if(dirNum == 2)
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
                if(i == repeatNum-1)
                {
                    laserTime = 0f;
                }
            }
        }
        if (bossTime >= rendDeleyTime * 20f)
        {
            GameObject newEnemy = GetObj();

            if (newEnemy)
            {
                //위치지정
                SetRandomPos();
                newEnemy.transform.position = new Vector3(x, y, 0);

                newEnemy.GetComponent<Enemy>().SetType(EnemyType.boss);
                newEnemy.SetActive(true);

                bossTime = 0f;
            }
        }
    }

    //시간마다 RendTime설정
    private void ReduceRendTime()
    {
        if (GameManager.instance.curScore >= 1000)
        {
            rendDeleyTime = 0.3f;
        }
        else if (GameManager.instance.curScore >= 800)
        {
            rendDeleyTime = 0.5f;
        }
        else if (GameManager.instance.curScore >= 500)
        {
            rendDeleyTime = 0.6f;
        }
        else if (GameManager.instance.curScore >= 250)
        {
            rendDeleyTime = 0.7f;
        }
        else if (GameManager.instance.curScore >= 100)
        {
            rendDeleyTime = 0.8f;
        }
        else
        {
            rendDeleyTime = 1f;
        }
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
