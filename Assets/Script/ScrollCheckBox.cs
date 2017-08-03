using UnityEngine;
using System.Collections;

public class ScrollCheckBox : MonoBehaviour {
    public int checkNum = 0;
    public int LootNum = 0;
    public UILabel LootLabel;
    public UILabel haveCoin;

    public GameObject effectPrefab;
    public GameObject grid;
    public GameObject homeButton;

    public GameObject buyPop;
    public UISprite buyImage;
    public UISprite buyImage2;
    public GameObject getCoinLabel;

    float buyTime;
    public bool buyState = false;

    public GameObject[] leverImage;
    
    int getCoin = 0;
    int randNum = 0;

    private void Update()
    {
        LootLabel.text = LootNum.ToString();
        getCoinLabel.GetComponent<UILabel>().text = getCoin.ToString();
        haveCoin.text = GameManager.instance.coin.ToString();

        if (buyState)
        {
            buyTime += Time.deltaTime;

            homeButton.SetActive(false);
            grid.GetComponent<UIDragScrollView>().enabled = false;

            if(buyTime >= 5f)
            {
                buyPop.SetActive(true);

                leverImage[1].SetActive(false);
                leverImage[0].SetActive(true);

                homeButton.SetActive(true);
                grid.GetComponent<UIDragScrollView>().enabled = true;
                buyState = false;
                buyTime = 0f;
            }
        }
    }

    public void BuyButton()
    {
        if (!buyState)
        {
            if (GameManager.instance.coin >= 100 && LootNum >= 10)
            {
                GameManager.instance.coin -= 100;
                GameObject effect = Instantiate(effectPrefab);
                effect.transform.position = new Vector3(0, .5f, 0f);

                randNum = Random.Range(30, 51);
                getCoin = (int)(LootNum * (randNum * 0.01f));

                leverImage[0].SetActive(false);
                leverImage[1].SetActive(true);

                switch (checkNum)
                {
                    case 1:
                        GameManager.instance.redLoot = 0;
                        GameManager.instance.redCoin += getCoin;
                        LootNum = 0;
                        break;
                    case 2:
                        GameManager.instance.blueLoot = 0;
                        GameManager.instance.blueCoin += getCoin;
                        LootNum = 0;
                        break;
                    case 3:
                        GameManager.instance.greenLoot = 0;
                        GameManager.instance.greenCoin += getCoin;
                        LootNum = 0;
                        break;
                }

                DataManager.Instance.SetData();

                buyState = true;
            }
            else
            {
                this.GetComponent<CameraShake>().shake = .5f;
                Debug.Log("not enough coin");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "1"
            || other.transform.name == "2"
            || other.transform.name == "3")
        {
            checkNum = int.Parse(other.transform.name);
            
            other.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        }
        
        switch(other.transform.name)
        {
            case "1":
                LootNum = GameManager.instance.redLoot;
                buyImage.spriteName = "enemy";
                buyImage2.spriteName = "stone";
                break;
            case "2":
                LootNum = GameManager.instance.blueLoot;
                buyImage.spriteName = "enemy2";
                buyImage2.spriteName = "stone3";
                break;
            case "3":
                LootNum = GameManager.instance.greenLoot;
                buyImage.spriteName = "enemy3";
                buyImage2.spriteName = "stone2";
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "1"
       || other.transform.name == "2"
       || other.transform.name == "3")
        {
            other.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
