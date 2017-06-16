using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //플레이어 이동속도
    public float moveSpeed = 3f;

    //발사 딜레이
    private float bulletDelayTime = .4f;
    private float curTime = 0f;

    //방향지정 숫자
    public int dirNum = 1;

    //방향 vector
    public Vector3 fireDirection;


    // DUMMYSYSTEM
    [SerializeField]
    private GameObject dummyObj;

    private List<GameObject> dummyList = new List<GameObject>();

    public int dummyNum = 0;

    private float dx, dy;
    private float dummyRotTime = 0f;
    private float dummyRotSpeed = 4f;

    private void Start()
    {
        //게임시작 시 총알생성
        BulletManager.instance.MakeObjs();

        fireDirection = Vector3.up;

    }

    private void Update()
    {
        if (GameManager.instance.curGameState == GameState.game)
        {
            PlayerFired();
            RotateDummy();

            #region Zoom2보류
            //foreach (GameObject enemy in EnemyManager.instance.objList)
            //{
            //    float dis = Vector2.Distance(enemy.transform.position, this.transform.position);

            //    if (dis <= 2f)
            //    {
            //        Camera.main.GetComponent<SmoothCamera>().ZoomCamera2(dis);
            //    }
            //}
            #endregion
        }
    }

    //발사
    private void PlayerFired()
    {
        curTime += Time.deltaTime;

        if (curTime >= bulletDelayTime)
        {
            BulletManager.instance.FireBullets(this.transform.position);

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
        else if (other.transform.tag.Equals("DummyItem"))
        {
            MakeDummy();

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

    //플레이어 초기화
    public void InitPlayer()
    {
        //위치
        this.transform.position = Vector3.zero;

        //방향
        dirNum = 1;
        SetPlayerDirection(dirNum);

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
        //더미하나 삭제
        Destroy(dummyList[0]);
        dummyList.Remove(dummyList[0]);

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
                dummyList[i].transform.localPosition = new Vector3(dx * 0.7f, 0f, dy * 0.7f);
            }
            else
            {
                dummyList[i].transform.localPosition = new Vector3(0f, dy * 0.7f, dx * 0.7f);
            }
        }
    }
}