using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] gameStateUI;

    [SerializeField]
    private GameObject[] tweenUI;

    [SerializeField]
    private UILabel curScore;
    [SerializeField]
    private UILabel[] topScore;


    //게임상태에 따른 UI상태 정리
    private void UIState(GameState _state)
    {
        switch(_state)
        {
            case GameState.main:
                gameStateUI[0].SetActive(true);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(false);
                break;
            case GameState.game:
                tweenUI[1].GetComponent<TweenScale>().Play();

                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(true);
                gameStateUI[2].SetActive(false);
                break;
            case GameState.over:
                gameStateUI[0].SetActive(false);
                gameStateUI[1].SetActive(false);
                gameStateUI[2].SetActive(true);

                InitTweenUI();
                break;
        }
        RendText();
    }

    private void RendText()
    {
        //현재점수 표시
        curScore.text = GameManager.instance.curScore.ToString();

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

    private void Update()
    {
        UIState(GameManager.instance.curGameState);
    }
}
