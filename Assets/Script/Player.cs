using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private BulletManager bulletManager;

    private float bulletDelayTime = 1f;
    private float curTime = 0f;

    public int dirNum = 1;

    public Vector3 fireDirection;

    private void Start()
    {
        //게임시작 시 총알생성
        bulletManager.MakeBullets();

        fireDirection = Vector3.up;
    }

    private void Update()
    {
        curTime += Time.deltaTime;

        if(curTime >= bulletDelayTime)
        {
            bulletManager.FireBullets(this.transform.position);

            curTime = 0f;
        }
    }

    //충돌
    private void OnTriggerEnter(Collider other)
    {
        //방향전환 아이템.
        if (other.transform.tag.Equals("DirItem"))
        {
            this.dirNum = other.GetComponent<DirItem>().DIRNUM;

            SetPlayerDirection(dirNum);

            other.gameObject.SetActive(false);
        }
    }

    //플레이어 방향 전환
    private void SetPlayerDirection(int _dirNum)
    {
        switch (_dirNum)
        {
            case 1:
                fireDirection = this.transform.position + Vector3.up;
                break;
            case 2:
                fireDirection = this.transform.position + Vector3.up + Vector3.left;
                break;
            case 3:
                fireDirection = this.transform.position + Vector3.left;
                break;
            case 4:
                fireDirection = this.transform.position + Vector3.left + Vector3.down;
                break;
            case 5:
                fireDirection = this.transform.position + Vector3.down;
                break;
            case 6:
                fireDirection = this.transform.position + Vector3.down + Vector3.right;
                break;
            case 7:
                fireDirection = this.transform.position + Vector3.right;
                break;
            case 8:
                fireDirection = this.transform.position + Vector3.up + Vector3.right;
                break;
        }

        if (fireDirection != Vector3.zero)
        {
            //플레이어 바라보는 방향 전환
            this.transform.LookAt(fireDirection);
        }
    }


}