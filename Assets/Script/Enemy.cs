﻿using UnityEngine;
using System.Collections;

public enum EnemyType
{
    normal,
    speeder,
    tanker,
    laser,
    boss
}

public class Enemy : MonoBehaviour
{
    private EnemyType enemyType = EnemyType.normal;

    private float moveSpeed = 1.5f;
    private float rotSpeed = 200f;

    //Sprite Renderer
    public SpriteRenderer renderer;
    public Sprite[] enemyImage = new Sprite[3];
    public Sprite[] bossImage = new Sprite[6];

    //Laser Pattern
    public Vector3 laserTarget;
    private Vector3 laserMoveVec;
    private float laserDelay = 0f;
    private bool aming = false;

    Player player;

    //보스 체력
    private int Life;

    //Enemy활성화시
    private void OnEnable()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        aming = false;
        laserDelay = 0f;

        if (this.enemyType == EnemyType.boss) { Life = 50; }
        else if(this.enemyType == EnemyType.tanker) { Life = 10; }
        else { Life = 1; }
    }

    void Update()
    {
        MoveToPlayer();
    }

    //이동
    private void MoveToPlayer()
    {
        //레이저패턴
        if (this.enemyType == EnemyType.laser)
        {
            this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotSpeed));
            if (!aming)
            {
                laserMoveVec = laserTarget - this.transform.position;
                laserMoveVec.Normalize();
                aming = true;
            }
            else
            {
                laserDelay += Time.deltaTime;
                if (laserDelay >= .5f)
                {
                    this.transform.position += laserMoveVec * Time.deltaTime * moveSpeed;
                }
            }
        }
        //보스패턴
        else if (this.enemyType == EnemyType.boss)
        {
            //보스는 계속 이동
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                            player.transform.position,
                                                            moveSpeed);

            this.transform.Rotate(new Vector3(0, 0, 0.03f * rotSpeed));

  
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                  player.transform.position,
                                                  moveSpeed * Time.deltaTime);
            this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotSpeed));
        }
    }

    //Setting Enemy Type
    public void SetType(EnemyType _type)
    {
        enemyType = _type;

        switch (enemyType)
        {
            case EnemyType.normal:
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[0];
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 1.5f;
                break;
            case EnemyType.speeder:
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[1];
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 2.5f;
                break;
            case EnemyType.tanker:
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[2];
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 1.5f;
                break;
            case EnemyType.laser:
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[3];
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 15f;
                break;
            case EnemyType.boss:
                this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, .5f);
                this.renderer.sprite = bossImage[Random.Range(0, 6)];
                this.transform.localScale = new Vector3(2f, 2f, 1f);
                moveSpeed = 0.03f;
                break;
        }
    }

    //충돌체크
    private void OnTriggerEnter(Collider other)
    {
        //총알에 닿으면
        if (other.transform.tag.Equals("Bullet"))
        {
            if (this.enemyType == EnemyType.laser)
                return;

            if(BulletManager.instance.curBulletType != BulletManager.bulletType.laser)
                other.gameObject.SetActive(false);

            //tanker일시 normal로 변경.
            if (this.enemyType == EnemyType.tanker)
            {
                Life--;

                if (Life <= 0)
                {
                    Life = 1;
                    this.SetType(EnemyType.normal);
                }
            }
            else
            {
                Life--;

                if (Life <= 0)
                {
                    if (this.enemyType == EnemyType.boss)
                    {
                        GameManager.instance.AddScore(10);      //10점
                    }
                    else
                    {
                        GameManager.instance.AddScore(1);      //1점
                    }
                    this.gameObject.SetActive(false);
                }
            }
        }
        else if (other.transform.tag.Equals("Warning"))
        {
            if (this.enemyType == EnemyType.laser)
                other.gameObject.SetActive(false);
        }
    }
}