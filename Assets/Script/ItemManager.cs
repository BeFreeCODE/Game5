using UnityEngine;
using System.Collections;

public class ItemManager : ObjectManager  
{
    public static ItemManager instance;
    
    [SerializeField]
    private float rendDeleyTime = 1.5f;
    private float curTime = 0f;
    private float x, y;

    [SerializeField]
    private float maxRange, minRange;

    private void Awake()
    {
        if(instance == null)
            instance = this;
       
    }

    private void Start()
    {
        this.maxNum = 10;
        MakeObjs();
    }

    //Item Render
    public void RendItem()
    {
        curTime += Time.deltaTime;

        if (curTime >= rendDeleyTime)
        {
            GameObject newItem = GetObj();

            if (newItem)
            {
                SetPos();

                newItem.transform.position = new Vector3(x, y, 0);
                newItem.SetActive(true);

                curTime = 0f;
            }
        }
    }

    //x,y 위치지정
    private void SetPos()
    {
        if(maxRange < minRange)
        {
            return;
        }
        x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
        y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);


        while (Mathf.Abs(x - player.transform.position.x) <= minRange &&
                Mathf.Abs(y - player.transform.position.y) <= minRange)
        {
            x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
            y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);
        }
    }
}
