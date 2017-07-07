using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public bool isFire = false;

    [SerializeField]
    private Vector2 fireDirection = Vector2.up;

    //발사속도
    private float bulletSpeed = 8f;
    private float laserWidth = 1f;

    public BulletManager.bulletType thisType = BulletManager.bulletType.normal;

    [SerializeField]
    private GameObject[] exEffect;

    public int bulletDamage;
    private int bounceNum = 3;

    
    private void OnEnable()
    {
        thisType = BulletManager.instance.curBulletType;

        bounceNum = 3;

        if (thisType == BulletManager.bulletType.laser)
        {
            laserWidth = 1f;
            this.transform.localScale = new Vector3(laserWidth, 10f, 1f);
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
                isFire = false;
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
            laserWidth -= Time.deltaTime * 5f;
            this.transform.localScale = new Vector3(laserWidth, 20f, 1f);
            
            if (laserWidth <= 0f)
            {
                this.gameObject.SetActive(false);
                laserWidth = 1f;
            }
        }
        else if(thisType == BulletManager.bulletType.guided || thisType == BulletManager.bulletType.sword)
        {
            //homing
        }
        else if(thisType == BulletManager.bulletType.explosion)
        {
            this.transform.Translate(fireDirection * Time.deltaTime * bulletSpeed * 0.5f, Space.Self);
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
    public void SetFireDirection(Vector3 dir)
    {
        if (thisType == BulletManager.bulletType.laser)
        {
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            this.transform.rotation = Quaternion.Euler(0, 0, -angle);
  
        }
        else if (thisType == BulletManager.bulletType.sword)
        {
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            this.GetComponent<TweenRotation>().from = new Vector3(0, 0, -angle );
            this.GetComponent<TweenRotation>().to = new Vector3(0, 0, -angle - 180f);
        }
        this.fireDirection = dir;
    
    }

    public void SetDamage()
    {
        switch(this.thisType)
        {
            case BulletManager.bulletType.normal:
                bulletDamage = 1;
                break;
            case BulletManager.bulletType.big:
                bulletDamage = 1;
                break;
            case BulletManager.bulletType.laser:
                bulletDamage = 5;
                break;
            case BulletManager.bulletType.bounce:
                bulletDamage = 1;
                break;
            case BulletManager.bulletType.guided:
                bulletDamage = 1;
                break;
            case BulletManager.bulletType.sword:
                bulletDamage = 50;
                break;
            case BulletManager.bulletType.explosion:
                bulletDamage = 1;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Enemy"))
        {
            if(this.thisType == BulletManager.bulletType.bounce)
            {
                bounceNum--;
             
                //팅기는 방향
                int _randNum = Random.Range(1, 9);
                Vector3 randPos = Vector3.up;
              
                switch (_randNum)
                {
                    case 1:
                        randPos =  Vector3.up;
                        break;
                    case 2:
                        randPos =  Vector3.up + Vector3.right;
                        break;
                    case 3:
                        randPos =  Vector3.right;
                        break;
                    case 4:
                        randPos =  Vector3.right + Vector3.down;
                        break;
                    case 5:
                        randPos =  Vector3.down;
                        break;
                    case 6:
                        randPos =  Vector3.down + Vector3.left;
                        break;
                    case 7:
                        randPos =  Vector3.left;
                        break;
                    case 8:
                        randPos =  Vector3.left + Vector3.up;
                        break;

                }
                
                SetFireDirection(randPos.normalized);

                //더이상 팅길수 없을때 Off
                if(bounceNum <=0)
                {
                    OffBullet();
                }

                //피격이펙트
                GameObject _effect = Instantiate(exEffect[2]);
                _effect.transform.position = other.transform.position;
                Destroy(_effect, 1.5f);
            }
            else if (this.thisType == BulletManager.bulletType.sword)
            {
                GameObject _effect = Instantiate(exEffect[1]);
                _effect.transform.position = other.transform.position;
                Destroy(_effect, 1.5f);
            }
            else if (this.thisType == BulletManager.bulletType.explosion)
            {
                GameObject _effect = Instantiate(exEffect[0]);
                _effect.transform.position = other.transform.position;
                Destroy(_effect, 1.5f);
            }
            else
            {
                GameObject _effect = Instantiate(exEffect[2]);
                _effect.transform.position = other.transform.position;
                Destroy(_effect, 1.5f);
            }
        }
    }

    public void OffBullet()
    {
        this.gameObject.SetActive(false);
    }
}
