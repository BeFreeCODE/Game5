using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour
{
    public Camera uiCam;

    public UISprite image;

    public Player player;

    public GameObject arrow;
    private GameObject target;

    private bool arrowMove = false;
    public int selectNum = 0;

    public int[] levels = new int[3];

    public GameObject[] damageGraphs = new GameObject[5];
    public GameObject[] moveSpeedGraphs = new GameObject[5];
    public GameObject[] fireSpeedGraphs = new GameObject[5];

    public UILabel[] stoneLabel = new UILabel[3];

    private void Start()
    {
        selectNum = 0;

        levels[0] = GameManager.instance.redLevel[selectNum];
        levels[1] = GameManager.instance.greenLevel[selectNum];
        levels[2] = GameManager.instance.blueLevel[selectNum];
    }

    private void OnEnable()
    {
        player.GetPlayerJsonData();
        arrow.transform.localPosition = new Vector3(-1000, -1500, 0);
        selectNum = 0;
        levels[0] = GameManager.instance.redLevel[selectNum];
        levels[1] = GameManager.instance.greenLevel[selectNum];
        levels[2] = GameManager.instance.blueLevel[selectNum];
    }

    private void SelectPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = uiCam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            {
                target = hit.collider.gameObject;

                switch (target.name)
                {
                    case "normal":
                        arrowMove = true;
                        image.spriteName = "player01";
                        selectNum = 0;
                        break;
                    case "big":
                        arrowMove = true;
                        image.spriteName = "player02";
                        selectNum = 1;
                        break;
                    case "laser":
                        arrowMove = true;
                        image.spriteName = "player03";
                        selectNum = 2;
                        break;
                    case "bounce":
                        arrowMove = true;
                        image.spriteName = "player04";
                        selectNum = 3;
                        break;
                    case "guided":
                        arrowMove = true;
                        image.spriteName = "player05";
                        selectNum = 4;
                        break;
                    case "sword":
                        arrowMove = true;
                        image.spriteName = "player06";
                        selectNum = 5;
                        break;
                    case "explosion":
                        arrowMove = true;
                        image.spriteName = "player07";
                        selectNum = 6;
                        break;
                }

                //type change
                player.ChangePlayerType(selectNum);

                if (target != null)
                {
                    levels[0] = GameManager.instance.redLevel[selectNum];
                    levels[1] = GameManager.instance.greenLevel[selectNum];
                    levels[2] = GameManager.instance.blueLevel[selectNum];
                }
            }
        }
    }

    //stats 그래프 출력
    private void RendGraphs()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i <= levels[0])
                damageGraphs[i].GetComponent<UISprite>().spriteName = "rectred";
            else
                damageGraphs[i].GetComponent<UISprite>().spriteName = "rectwhite";
        }
        for (int i = 0; i < 5; i++)
        {
            if (i <= levels[1])
                moveSpeedGraphs[i].GetComponent<UISprite>().spriteName = "rectgreen";
            else
                moveSpeedGraphs[i].GetComponent<UISprite>().spriteName = "rectwhite";
        }
        for (int i = 0; i < 5; i++)
        {
            if (i <= levels[2])
                fireSpeedGraphs[i].GetComponent<UISprite>().spriteName = "rectblue";
            else
                fireSpeedGraphs[i].GetComponent<UISprite>().spriteName = "rectwhite";
        }
    }

    public void DamageButton()
    {
        if (GameManager.instance.redCoin >= 100)
        {
            if (GameManager.instance.redLevel[selectNum] < 4)
            {
                GameManager.instance.redLevel[selectNum]++;
                GameManager.instance.redCoin -= 100;
                levels[0]++;
                player.GetPlayerJsonData();
            }
        }
    }
    public void MoveSpeedButton()
    {
        if (GameManager.instance.blueCoin >= 100)
        {
            if (GameManager.instance.greenLevel[selectNum] < 4)
            {
                GameManager.instance.greenLevel[selectNum]++;
                GameManager.instance.blueCoin -= 100;
                levels[1]++;
                player.GetPlayerJsonData();
            }
        }
    }
    public void FireSpeedButton()
    {
        if (GameManager.instance.greenCoin >= 100)
        {
            if (GameManager.instance.blueLevel[selectNum] < 4)
            {
                GameManager.instance.blueLevel[selectNum]++;
                GameManager.instance.greenCoin -= 100;
                levels[2]++;
                player.GetPlayerJsonData();
            }
        }
    }

    private void Update()
    {
        if (arrowMove)
        {
            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position,
                                                target.transform.position,
                                               .3f);
            if (arrow.transform.position == target.transform.position)
            {
                arrowMove = false;
            }
        }

        SelectPlayer();
        RendGraphs();

        stoneLabel[0].text = GameManager.instance.redCoin.ToString();
        stoneLabel[1].text = GameManager.instance.blueCoin.ToString();
        stoneLabel[2].text = GameManager.instance.greenCoin.ToString();
    }

}
