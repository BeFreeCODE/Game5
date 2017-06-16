using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : ObjectManager
{
    public static EnemyManager instance;
    
    private float rendDeleyTime = 1f;

    //적 Tpye에따른 시간
    private float normalTime = 0f, speederTime = 0f, tankerTime = 0f, bossTime = 0f;
    private float x, y;

    [SerializeField]
    private float maxRange, minRange;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        MakeObjs();
    }

    //Enemy Render
    public void RendEnemy()
    {
        normalTime += Time.deltaTime;
        speederTime += Time.deltaTime;
        tankerTime += Time.deltaTime;
        bossTime += Time.deltaTime;

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
        if (tankerTime >= rendDeleyTime * 3f)
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
        if(bossTime >= rendDeleyTime * 10f)
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

    //x,y 위치지정
    private void SetPos()
    {
        x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
        y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);
        

        while(Mathf.Abs(x - player.transform.position.x) <= minRange &&
                Mathf.Abs(y - player.transform.position.y) <= minRange )
        {
            x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
            y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);
        }
    }
}
