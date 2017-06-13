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

}
