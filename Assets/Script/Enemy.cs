using UnityEngine;
using System.Collections;

public enum EnemyType
{
    normal,
    speeder,
    tanker,
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

    Player player;

    //보스 체력
    private int Life;


    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if(this.enemyType == EnemyType.boss)
        {
            Life = 10;
        }
        else
        {
            Life = 1;
        }
    }

    //이동함수
    private void MoveToPlayer()
    {
        if (this.enemyType != EnemyType.boss)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                            player.transform.position,
                                                            moveSpeed * Time.deltaTime);
            this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotSpeed));
        }
        else
        {
            //보스는 계속 이동
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                            player.transform.position,
                                                            moveSpeed);

            this.transform.Rotate(new Vector3(0, 0, 0.03f * rotSpeed));
        }
    }

    //Setting Enemy Type
    public void SetType(EnemyType _type)
    {
        enemyType = _type;

        switch (enemyType)
        {
            case EnemyType.normal:
                this.renderer.sprite = enemyImage[0];
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 1.5f;
                break;
            case EnemyType.speeder:
                this.renderer.sprite = enemyImage[1];
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 2.5f;
                break;
            case EnemyType.tanker:
                this.renderer.sprite = enemyImage[2];
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 1.5f;
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
            other.gameObject.SetActive(false);

            //tanker일시 normal로 변경.
            if (this.enemyType == EnemyType.tanker)
            {
                this.SetType(EnemyType.normal);
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

        //Player에 닿으면 GameOver
        else if (other.transform.tag.Equals("Player"))
        {
            GameManager.instance.StateTransition(GameState.over);
        }
    }

    void Update()
    {
        MoveToPlayer();
       
    }
}