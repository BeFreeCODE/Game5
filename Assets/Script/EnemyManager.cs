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

        if (normalTime >= rendDeleyTime)
        {
            GameObject newEnemy = GetObj();

            if (newEnemy)
            {
                //위치지정
                SetPos();
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
                SetPos();
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
                SetPos();
                newEnemy.transform.position = new Vector3(x, y, 0);

                newEnemy.GetComponent<Enemy>().SetType(EnemyType.tanker);
                newEnemy.SetActive(true);

                tankerTime = 0f;
            }
        }
        if (laserTime >= rendDeleyTime * 6f)
        {
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                SetPos();

                GameObject newEnemy = GetObj();
                float _dx, _dy;

                if (newEnemy)
                {
                    _dx = Random.Range(-5f, 5f);
                    _dy = Random.Range(-5f, 5f);

                    //위치지정
                    newEnemy.transform.position = new Vector3(x, y, 0);


                    newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                    newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position
                                                                 + new Vector3(_dx,_dy,0) ;
                                    

                    for (int j = -20; j < 10; j++)
                    {
                        GameObject warning = GetWarning();

                        if (warning != null)
                        {
                            warning.transform.position = player.transform.position + new Vector3(_dx, _dy, 0)
                            + (newEnemy.transform.position - (player.transform.position + new Vector3(_dx, _dy, 0)) ) * (0.1f * j);

                            warning.SetActive(true);
                        }
                    }
                    newEnemy.SetActive(true);

                    laserTime = 0f;
                }
            }
        }
        if (bossTime >= rendDeleyTime * 10f)
        {
            GameObject newEnemy = GetObj();

            if (newEnemy)
            {
                //위치지정
                SetPos();
                newEnemy.transform.position = new Vector3(x, y, 0);

                newEnemy.GetComponent<Enemy>().SetType(EnemyType.boss);
                newEnemy.SetActive(true);

                bossTime = 0f;
            }
        }
    }

    private void ReduceRendTime()
    {
        if(GameManager.instance.curScore >= 1000)
        {
            rendDeleyTime = 0.3f;
        }
        else if(GameManager.instance.curScore >= 800)
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
    private void SetPos()
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
