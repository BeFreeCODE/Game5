using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //플레이어 이동속도
    public float moveSpeed;

    //발사 딜레이
    public float bulletDelayTime = .1f;
    public float curTime = 0f;

    //방향지정 숫자
    public int dirNum = 1;

    //방향 vector
    public Vector3 lookDirection;
    public Vector3 fireDirection;

    [SerializeField]
    private GameObject getEffect;

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

    private SmoothCamera blurCam;

    //플레이어 타입 번호
    public int playerType = 0;
    public Sprite[] playerImage = new Sprite[7];
    public GameObject[] playerSprite = new GameObject[2];

    private void Start()
    {
        fireDirection = Vector3.up;
        blurCam = GameObject.Find("Main Camera").GetComponent<SmoothCamera>();
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
        if (this.transform.tag != "Player")
            return;

        curTime += Time.deltaTime;
        
        //총알 타입별로 시간 조절
        if (BulletManager.instance.curBulletType == BulletManager.bulletType.laser)
        {
            bulletDelayTime = 1f;
        }
        else if(BulletManager.instance.curBulletType == BulletManager.bulletType.guided 
            || BulletManager.instance.curBulletType == BulletManager.bulletType.explosion)
        {
            bulletDelayTime = .3f;
        }
        else if (BulletManager.instance.curBulletType == BulletManager.bulletType.sword)
        {
            bulletDelayTime = 3f;
        }
        else
        {
            bulletDelayTime = .1f;
        }

        if (curTime >= bulletDelayTime)
        {
            BulletManager.instance.FireBullets(this.transform.position);
            
            //발사 사운드,데미지
            switch(BulletManager.instance.curBulletType)
            {
                case BulletManager.bulletType.normal:
                    SoundManager.instance.PlayEffectSound(4);
                    break;
                case BulletManager.bulletType.big:
                    SoundManager.instance.PlayEffectSound(5);
                    break;
                case BulletManager.bulletType.laser:
                    SoundManager.instance.PlayEffectSound(6);
                    break;
                case BulletManager.bulletType.bounce:
                    SoundManager.instance.PlayEffectSound(7);
                    break;
                case BulletManager.bulletType.guided:
                    SoundManager.instance.PlayEffectSound(8);
                    break;
                case BulletManager.bulletType.sword:
                    SoundManager.instance.PlayEffectSound(9);
                    break;
                case BulletManager.bulletType.explosion:
                    SoundManager.instance.PlayEffectSound(10);
                    break;
            }

            //더미리스트가 0이 아닐시 동시에 같이 발사해줌.
            if (dummyList.Count != 0)
            {
                foreach(GameObject _dummy in dummyList)
                {
                    BulletManager.instance.FireBullets(_dummy.transform.position);
                }
            }
      
            curTime = 0f;
        }
    }

    //플레이어 방향 전환
    public void SetPlayerDirection(Vector3 dir)
    {
        fireDirection = dir;
        lookDirection = this.transform.position + dir;
        this.transform.LookAt(lookDirection);
    }

    //플레이어 초기화
    public void InitPlayer()
    {
        //위치
        this.transform.position = Vector3.zero;

        //방향
        dirNum = 1;
        //PlayerDirection(dirNum);

        //이동, 발사속도
        moveSpeed = 3f;
        bulletDelayTime = .1f;

        dummyNum = 0;
        getSpeedItem = false;
        speedTime = 0;

        this.gameObject.SetActive(false);
    }

    //비행기 타입 변경
    public void ChangePlayerType(int _typeNum)
    {
        this.playerType = _typeNum;

        //총알변경
        BulletManager.instance.SetBulletType(this.playerType);

        //Sprite Image변경
        playerSprite[0].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];
        playerSprite[1].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];

    }

    #region 더미관련
    //더미플레이어 추가
    public void MakeDummy()
    {
        if (dummyObj == null)
            return;

        dummyNum++;
        GameObject _dummy = Instantiate(dummyObj);
        _dummy.transform.parent = this.transform;
        _dummy.transform.localRotation = Quaternion.identity;

        //Sprite Image변경
        _dummy.GetComponent<Player>().playerSprite[0].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];
        _dummy.GetComponent<Player>().playerSprite[1].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];

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

            blurCam.OnBlur();
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

            dummyList[i].transform.localPosition = new Vector3(0f, dy, dx);   
        }
    }
#endregion

    //충돌
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.curGameState != GameState.game)
            return;

        if (other.transform.tag.Equals("Bullet") 
            || other.transform.tag.Equals("Warning")
            || other.transform.tag.Equals("BobmEffect"))
            return;

        switch (other.transform.tag)
        {
            case "DirItem":
                this.dirNum = other.GetComponent<DirItem>().DIRNUM;
                //PlayerDirection(dirNum);
                break;
            case "DummyItem":
                MakeDummy();
                break;
            case "MoveItem":
                getSpeedItem = true;
                moveSpeed = 6f;

                GetItem(0);
                break;
            //Item Random Box
            case "RandomBox":
                ItemManager.instance.GetItemBox();
                
                GetItem(1);
                break;
            //적충돌
            case "Enemy":
                RemoveDummy();
                break;
        }

        other.transform.gameObject.SetActive(false);
    }

    //아이템 획득
    void GetItem(int _sNum)
    {
        GameObject _effect = Instantiate(getEffect);
        _effect.GetComponent<GetEffect>().SetSprite(_sNum);
        _effect.transform.parent = this.transform;
        _effect.transform.localPosition = Vector3.zero;
        _effect.transform.position += Vector3.forward;
    }
}