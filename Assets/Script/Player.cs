using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //플레이어 이동속도
    public float moveSpeed;

    //발사 딜레이
    public float bulletDelayTime = .1f;
    private float curTime = 0f;

    //방향지정 숫자
    public int dirNum = 1;

    //방향 vector
    public Vector3 fireDirection;


    // DUMMYSYSTEM
    [SerializeField]
    private GameObject dummyObj;
    [SerializeField]
    private List<GameObject> dummyList = new List<GameObject>();

    public int dummyNum = 0;

    private float dx, dy;
    private float dummyRotTime = 0f;
    private float dummyRotSpeed = 4f;

    private bool getSpeedItem = false;
    private float speedTime = 0;

    private void Start()
    {
        fireDirection = Vector3.up;
    }

    private void Update()
    {
        if (GameManager.instance.curGameState == GameState.game)
        {
            PlayerFired();
            RotateDummy();
            #region 버프
            //Speed버프효과
            if (getSpeedItem)
            {
                speedTime += Time.deltaTime;
                if (speedTime >= 5f)
                {
                    moveSpeed = 3f;
                    getSpeedItem = false;
                    speedTime = 0f;
                }
            }
            #endregion
        }
    }

    //발사
    private void PlayerFired()
    {
        curTime += Time.deltaTime;

        if (BulletManager.instance.curBulletType == BulletManager.bulletType.laser)
        {
            bulletDelayTime = 1f;
        }
        else
        {
            bulletDelayTime = .1f;
        }
        if (curTime >= bulletDelayTime)
        {
            BulletManager.instance.FireBullets(this.transform.position);

            curTime = 0f;
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

    //플레이어 초기화
    public void InitPlayer()
    {
        //위치
        this.transform.position = Vector3.zero;

        //방향
        dirNum = 1;
        SetPlayerDirection(dirNum);

        //이동, 발사속도
        moveSpeed = 3f;
        bulletDelayTime = .1f;

        dummyNum = 0;
        getSpeedItem = false;
        speedTime = 0;

        this.gameObject.SetActive(false);
    }

    //더미플레이어 추가
    public void MakeDummy()
    {
        if (dummyObj == null)
            return;

        dummyNum++;
        GameObject _dummy = Instantiate(dummyObj);
        _dummy.transform.parent = this.transform;
        _dummy.transform.localRotation = Quaternion.identity;
        dummyList.Add(_dummy);
    }

    //더미플레이어 제거
    public void RemoveDummy()
    {
        if (dummyObj == null)
            return;

        dummyNum--;

        if (dummyNum < 0)
        {
            GameManager.instance.StateTransition(GameState.over);
        }

        if (dummyList.Count > 0)
        {
            //더미하나 삭제
            Destroy(dummyList[0]);
            dummyList.Remove(dummyList[0]);
        }
    }

    //dummy회전
    private void RotateDummy()
    {
        dummyRotTime += Time.deltaTime * dummyRotSpeed;

        for (int i = 0; i < dummyNum; i++)
        {
            dx = Mathf.Cos(dummyRotTime + (i * (6.28f / dummyNum)));
            dy = Mathf.Sin(dummyRotTime + (i * (6.28f / dummyNum)));

            if (this.dirNum == 1 || this.dirNum == 5)
            {
                dummyList[i].transform.localPosition = new Vector3(dx, 0f, dy);
            }
            else
            {
                dummyList[i].transform.localPosition = new Vector3(0f, dy, dx);
            }
        }
    }

    //충돌
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Bullet") || other.transform.tag.Equals("Warning"))
            return;

        switch (other.transform.tag)
        {
            case "DirItem":
                this.dirNum = other.GetComponent<DirItem>().DIRNUM;
                SetPlayerDirection(dirNum);
                break;
            case "DummyItem":
                MakeDummy();
                break;
            case "MoveItem":
                getSpeedItem = true;
                moveSpeed = 6f;
                break;
            //Item Random Box
            case "RandomBox":
                ItemManager.instance.GetItemBox();
                break;
            //적충돌
            case "Enemy":
                RemoveDummy();
                break;
        }

        other.transform.gameObject.SetActive(false);
    }
}