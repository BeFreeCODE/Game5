using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    public Camera uiCam;

    string[] layer = new string[] { "UI", "Bullet" };

    [SerializeField]
    private GameObject[] gameStateUI;

    [SerializeField]
    private GameObject[] tweenUI;

    [SerializeField]
    private UILabel[] curScore;

    [SerializeField]
    private UILabel[] topScore;

    [SerializeField]
    private UILabel[] coin;
    
    [SerializeField]
    private GameObject[] overButtons;

    [SerializeField]
    private GameObject overMenu;

    [SerializeField]
    private GameObject[] gaugeBar;

    [SerializeField]
    private UILabel gaugeLevel;

    [SerializeField]
    private UILabel stageTime;

    [SerializeField]
    private UILabel[] loot;

    [SerializeField]
    private UILabel[] stone;

    private void Update()
    {
        UIState(GameManager.instance.curGameState);
    }

    //게임상태에 따른 UI상태 정리
    private void UIState(GameState _state)
    {
        switch(_state)
        {
            case GameState.main:
                layer = new string[] { "UI" };

                uiCam.cullingMask = LayerMask.GetMask(layer);

                gameStateUI[0].SetActive(true);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(false);
                gameStateUI[3].SetActive(false);
                gameStateUI[4].SetActive(false);

                break;
            case GameState.game:
                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(true);
                gameStateUI[2].SetActive(false);
                gameStateUI[3].SetActive(false);
                gameStateUI[4].SetActive(false);

                tweenUI[1].GetComponent<TweenScale>().Play();

                RendGauge();
                break;
            case GameState.over:
                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(true);
                gameStateUI[3].SetActive(false);
                gameStateUI[4].SetActive(false);

                overMenu.GetComponent<TweenScale>().Play();

                for (int i = 0; i < overButtons.Length; i++)
                {
                    overButtons[i].GetComponent<TweenPosition>().Play();
                }

                InitTweenUI();
                break;
            case GameState.store:
                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(false);
                gameStateUI[3].SetActive(true);
                gameStateUI[4].SetActive(false);
                break;
            case GameState.store2:
                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(false);
                gameStateUI[3].SetActive(false);
                gameStateUI[4].SetActive(true);

                layer = new string[] { "UI", "Bullet" };
                uiCam.cullingMask = LayerMask.GetMask(layer);
                break;
        }
        RendText();
    }

    //uilabel rend
    private void RendText()
    {
        //현재점수 표시
        for (int i = 0; i < curScore.Length; i++)
        {
            curScore[i].text = GameManager.instance.curScore.ToString();
        }
        //최고점수 표시
        for (int i = 0; i < topScore.Length; i++)
        {
            topScore[i].text = GameManager.instance.topScore.ToString();
        }
        //코인 표시
        for (int i = 0; i < coin.Length; i++)
        {
            coin[i].text = GameManager.instance.coin.ToString();
        }
        //전리품 표시
        loot[0].text = GameManager.instance.redLoot.ToString();
        loot[1].text = GameManager.instance.blueLoot.ToString();
        loot[2].text = GameManager.instance.greenLoot.ToString();

        //Stage 시간표시
        stageTime.text = GameManager.instance.stageCurTime.ToString("N0");

        stone[0].text = GameManager.instance.redCoin.ToString();
        stone[1].text = GameManager.instance.blueCoin.ToString();
        stone[2].text = GameManager.instance.greenCoin.ToString();
    }

    //Tween 초기화.
    private void InitTweenUI()
    {
        for (int i = 0; i < tweenUI.Length; i++)
        {
            tweenUI[i].GetComponent<TweenScale>().ResetToBeginning();
            if (i == 1)
                continue;
            tweenUI[i].GetComponent<TweenScale>().Play();
        }
    }

    private void RendGauge()
    {
        Player _player = GameManager.instance.player;

        gaugeBar[0].GetComponent<UISprite>().fillAmount
            = _player.powerGauge / _player.maxGauge;

        gaugeLevel.text = "lv " + (GameManager.instance.player.powerNum + 1).ToString() ;

        if(_player.getSpeedItem)
        {
            gaugeBar[1].SetActive(true);
            gaugeBar[1].GetComponent<UISprite>().fillAmount
                = (5f - _player.speedTime) / 5f;
        }
        else
        {
            gaugeBar[1].SetActive(false);
        }

        if (_player.getMagnetItem)
        {
            gaugeBar[2].SetActive(true);
            gaugeBar[2].GetComponent<UISprite>().fillAmount
                = (10f - _player.magnetTime) / 10f;
        }
        else
        {
            gaugeBar[2].SetActive(false);
        }
    }

#region UIButtons
    public void HomeButton()
    {
#region OverTween초기화
        curScore[1].GetComponent<TweenScale>().ResetToBeginning();
        curScore[1].GetComponent<TweenScale>().Play();
        curScore[1].GetComponent<TweenPosition>().ResetToBeginning();
        curScore[1].GetComponent<TweenPosition>().Play();


        topScore[2].GetComponent<TweenScale>().ResetToBeginning();
        topScore[2].GetComponent<TweenScale>().Play();
        topScore[2].GetComponent<TweenPosition>().ResetToBeginning();
        topScore[2].GetComponent<TweenPosition>().Play();

        overMenu.GetComponent<TweenScale>().ResetToBeginning();

        for(int i=0;i<overButtons.Length;i++)
        {
            overButtons[i].GetComponent<TweenPosition>().ResetToBeginning();
        }
        #endregion
        GameManager.instance.StateTransition(GameState.main);
        GameManager.instance.player.gameObject.SetActive(true);
        GameManager.instance.player.ChangePlayerType(0);
        
        SoundManager.instance.PlayEffectSound(0);
    }
    public void ReplayButton()
    {
#region OverTween초기화
        curScore[1].GetComponent<TweenScale>().ResetToBeginning();
        curScore[1].GetComponent<TweenScale>().Play();
        curScore[1].GetComponent<TweenPosition>().ResetToBeginning();
        curScore[1].GetComponent<TweenPosition>().Play();


        topScore[2].GetComponent<TweenScale>().ResetToBeginning();
        topScore[2].GetComponent<TweenScale>().Play();
        topScore[2].GetComponent<TweenPosition>().ResetToBeginning();
        topScore[2].GetComponent<TweenPosition>().Play();

        overMenu.GetComponent<TweenScale>().ResetToBeginning();
        for (int i = 0; i < overButtons.Length; i++)
        {
            overButtons[i].GetComponent<TweenPosition>().ResetToBeginning();
        }
        #endregion
        GameManager.instance.player.gameObject.SetActive(true);
        GameManager.instance.curScore = 0;
        GameManager.instance.StateTransition(GameState.game);

        SoundManager.instance.PlayEffectSound(0);
    }
    public void StoreButton()
    {
        GameManager.instance.player.gameObject.SetActive(false);
        GameManager.instance.StateTransition(GameState.store);

        SoundManager.instance.PlayEffectSound(0);
    }
    public void DecompositionButton()
    {
        GameManager.instance.player.gameObject.SetActive(false);
        GameManager.instance.StateTransition(GameState.store2);

        SoundManager.instance.PlayEffectSound(0);
    }
    public void RankButton()
    {
        SoundManager.instance.PlayEffectSound(0);
    }
    public void AchButton()
    {
        SoundManager.instance.PlayEffectSound(0);
    }
    public void ShareButton()
    {
        SoundManager.instance.PlayEffectSound(0);
    }
#endregion
}
