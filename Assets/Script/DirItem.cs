using UnityEngine;
using System.Collections;

public class DirItem : MonoBehaviour {
    private int dirNum;
    public int DIRNUM { get {return dirNum; } }

    private void OnEnable()
    {
        //활성화 되었을때 dirNum 정해줌
        dirNum = Random.Range(1, 9);

        SetDirection(dirNum);
    }

    private void SetDirection(int _dirNum)
    {
        //방향변경
        this.transform.localRotation = Quaternion.Euler(0, 0, (_dirNum - 1) * 45);
    }

    //플레리어와 거리
    private float DistanceToPlayer()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = this.transform.position;

        return Vector3.Distance(myPos, playerPos);

    }

    private void Update()
    {
        if(GameManager.instance.curGameState == GameState.game)
        {
            //거리가 10이상이면 꺼줌.
            if(DistanceToPlayer() >= 10f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
