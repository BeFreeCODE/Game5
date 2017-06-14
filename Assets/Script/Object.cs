﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Object : MonoBehaviour
{
    //생성할 프리팹 오브젝트
    public GameObject makeObj;

    public Player player;

    //오브젝트 리스트
    public List<GameObject> objList = new List<GameObject>();

    //오브젝트 최대개수
    public int maxNum = 200;

    //생성
    public void MakeObjs()
    {
        for (int i = 0; i < maxNum; i++)
        {
            GameObject newObj = Instantiate(makeObj);
            newObj.transform.parent = this.transform;
            newObj.SetActive(false);

            objList.Add(newObj);
        }
    }

    //오브젝트 가져오기
    public GameObject GetObj()
    {
        foreach (GameObject _obj in objList)
        {
            if (!_obj.activeInHierarchy)
            {
                return _obj;
            }
        }
        return null;
    }

    //오브젝트 전체 지우기
    public void DestroObjs()
    {
        foreach (GameObject _obj in objList)
        {
            Destroy(_obj);  
        }
        objList.Clear();
    }
}
