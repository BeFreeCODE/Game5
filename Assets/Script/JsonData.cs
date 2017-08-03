using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class StatData
{
    public string type;
    public int level;
    public int damage;
    public int moveSpeed;
    public int fireSpeed;
}

public class JsonData : MonoBehaviour
{
    string[] dataPath = new string[7];
    TextAsset[] androidPath = new TextAsset[7];

    string allJson;
    string _name;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            androidPath[0] = Resources.Load("normalData") as TextAsset;
            androidPath[1] = Resources.Load("bigData") as TextAsset;
            androidPath[2] = Resources.Load("laserData") as TextAsset;
            androidPath[3] = Resources.Load("bounceData") as TextAsset;
            androidPath[4] = Resources.Load("guidedData") as TextAsset;
            androidPath[5] = Resources.Load("swordData") as TextAsset;
            androidPath[6] = Resources.Load("explosionData") as TextAsset;

        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
        else
        {
            dataPath[0] = Application.dataPath + "/Resources/normalData.json";
            dataPath[1] = Application.dataPath + "/Resources/bigData.json";
            dataPath[2] = Application.dataPath + "/Resources/laserData.json";
            dataPath[3] = Application.dataPath + "/Resources/bounceData.json";
            dataPath[4] = Application.dataPath + "/Resources/guidedData.json";
            dataPath[5] = Application.dataPath + "/Resources/swordData.json";
            dataPath[6] = Application.dataPath + "/Resources/explosionData.json";
        }
    }

    //초기 jsonData file setting
    public void SaveData(int pathNum, string name)
    {
        StatData[] statData = new StatData[5];

        for (int i = 0; i < 5; i++)
        {

            statData[i] = new StatData();
            statData[i].type = name;
            statData[i].level = i + 1;
            statData[i].damage = i + 1;
            statData[i].moveSpeed = i + 1;
            statData[i].fireSpeed = 5 - i;

        }

        string toJson = JsonHelper.ToJson(statData, prettyPrint: true);

        File.WriteAllText(dataPath[pathNum], toJson);
    }

    //type,level,필요 data별로 return
    public int LoadData(int _type, int level, string data)
    {
        string json;

        StatData[] statData = null;

        if (Application.platform == RuntimePlatform.Android)
        {
            json = androidPath[_type].ToString();

            statData = JsonHelper.FromJson<StatData>(json);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
        else
        {
            json = File.ReadAllText(dataPath[_type]);

            statData = JsonHelper.FromJson<StatData>(json);
        }

        switch (data)
        {
            case "damage":
                return statData[level].damage;
                break;
            case "moveSpeed":
                return statData[level].moveSpeed;
                break;
            case "fireSpeed":
                return statData[level].fireSpeed;
                break;
        }

        return 0;
    }
}