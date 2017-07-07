using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] gameStateUI;

    [SerializeField]
    private GameObject[] tweenUI;

    [SerializeField]
    private UILabel[] curScore;

    [SerializeField]
    private UILabel[] topScore;

    [SerializeField]
    private GameObject[] overButtons;

    [SerializeField]
    private GameObject overMenu;

 

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
                gameStateUI[0].SetActive(true);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(false);
                gameStateUI[3].SetActive(false);

                break;
            case GameState.game:
                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(true);
                gameStateUI[2].SetActive(false);
                gameStateUI[3].SetActive(false);

                tweenUI[1].GetComponent<TweenScale>().Play();
                break;
            case GameState.over:
                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(true);
                gameStateUI[3].SetActive(false);

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
