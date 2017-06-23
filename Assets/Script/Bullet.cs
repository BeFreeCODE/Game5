using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public bool isFire = false;

    [SerializeField]
    private Vector2 fireDirection = Vector2.up;

    //발사속도
    private float bulletSpeed = 8f;
   
    private BulletManager.bulletType thisType = BulletManager.bulletType.normal;
    float laserX = 1f;
    float laserZ = 0f;

    private void OnEnable()
    {
        thisType = BulletManager.instance.curBulletType;

        if (thisType == BulletManager.bulletType.laser)
        {
            laserX = 1f;
            this.transform.localScale = new Vector3(laserX, 10f, 1f);
        }
    }

    void Update()
    {
        if (isFire)
        {
            FireBullet();

            //거리가 10이상이면 꺼줌
            if (DistanceToPlayer() >= 10f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    
    //발사
    private void FireBullet()
    {
        //레이저일떄
        if (thisType == BulletManager.bulletType.laser)
        {
            laserX -= Time.deltaTime * 2f;
            this.transform.localScale = new Vector3(laserX, 20f, 1f);
            
            if (laserX <= 0f)
            {
                this.gameObject.SetActive(false);
                laserX = 1f;
            }
        }
        else
        {
            this.transform.Translate(fireDirection * Time.deltaTime * bulletSpeed, Space.Self);
        }
    }


    //플레리어와 거리
    private float DistanceToPlayer()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = this.transform.position;

        return Vector3.Distance(myPos, playerPos);
    }

    //총알 발사방향
    public void SetFireDirection(int _num)
    {
        if (thisType == BulletManager.bulletType.laser)
        {
            laserZ = (_num - 1) * 45f;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, laserZ));
        }
        switch (_num)
        {
            case 1:
                fireDirection = Vector3.up;
                break;
            case 2:
                fireDirection = Vector3.up + Vector3.left;
                break;
            case 3:
                fireDirection = Vector3.left;
                break;
            case 4:
                fireDirection = Vector3.left + Vector3.down;
                break;
            case 5:
                fireDirection = Vector3.down;
                break;
            case 6:
                fireDirection = Vector3.down + Vector3.right;
                break;
            case 7:
                fireDirection = Vector3.right;
                break;
            case 8:
                fireDirection = Vector3.up + Vector3.right;
                break;
        }
    }
}
