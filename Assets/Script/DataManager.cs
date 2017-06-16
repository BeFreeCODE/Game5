using UnityEngine;
using System.Collections;

public class DataManager
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DataManager();

            return instance;
        }
    }

    public void SetData()
    {
        PlayerPrefs.SetInt("TOPSCORE", GameManager.instance.topScore);
    }

    public void GetData()
    {
        GameManager.instance.topScore = PlayerPrefs.GetInt("TOPSCORE");
    }

    public void InitData()
    {
        GameManager.instance.curScore = 0;
        GameManager.instance.topScore = 0;

        SetData();
    }
}