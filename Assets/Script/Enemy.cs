using UnityEngine;
using System.Collections;

public enum EnemyType
{
    normal,
    speeder,
    tanker
}

public class Enemy : MonoBehaviour
{
    private EnemyType enemyType = EnemyType.normal;

    private float moveSpeed = 1.5f;
    private float rotSpeed = 200f;

    //Sprite Renderer
    public SpriteRenderer renderer;
    public Sprite[] enemyImage = new Sprite[3];

    Player player;

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void MoveToPlayer()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                        player.transform.position,
                                                        moveSpeed * Time.deltaTime);
    }

    //Setting Enemy Type
    public void SetType(EnemyType _type)
    {
        enemyType = _type;

        switch (enemyType)
        {
            case EnemyType.normal:
                this.renderer.sprite = enemyImage[0];
                moveSpeed = 1.5f;
                break;
            case EnemyType.speeder:
                this.renderer.sprite = enemyImage[1];
                moveSpeed = 2.5f;
                break;
            case EnemyType.tanker:
                this.renderer.sprite = enemyImage[2];
                moveSpeed = 1.5f;
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
                this.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        MoveToPlayer();

        this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotSpeed));
    }
}