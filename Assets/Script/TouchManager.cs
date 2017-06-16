using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    /// <summary>
    /// 기존 카메라(SmoothCamera)로 좌표비교하여 isDrag를하면 
    /// 카메라 움직임에 따라 좌표가 정확하지 않아 subCam으로 isDrag만 보조
    /// </summary>
    [SerializeField]
    private Camera subCam; private Vector2 subStart, subDrag;

    [SerializeField]
    private SmoothCamera cam;
    [SerializeField]
    private GameObject player;

    private Vector2 startVec;       //마우스 시작위치
    private Vector2 dragVec;        //마우스 드래그 위치

    private Vector2 moveVec;        //플레이어 움직임 벡터

    //드래그 상태
    public bool isDrag;
  
    private void OnMouseDown()
    {
#region 게임상태전환 
        //게임상태 전환
        if (GameManager.instance.curGameState == GameState.main)
        {
            GameManager.instance.StateTransition(GameState.game);
        }
        else if (GameManager.instance.curGameState == GameState.over)
        {
            player.gameObject.SetActive(true);
            GameManager.instance.StateTransition(GameState.main);
        }
#endregion

        if (!isDrag)
        {
            //마우스 위치 변수저장
            startVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            subStart = subCam.ScreenToWorldPoint(Input.mousePosition);

            dragVec = startVec;
            subDrag = subStart;

            //마우스 위치와 현재 플레이어 위치 차이 값
            moveVec = new Vector2(startVec.x - player.transform.position.x * player.GetComponent<Player>().moveSpeed,
                                    startVec.y - player.transform.position.y * player.GetComponent<Player>().moveSpeed);
        }
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.curGameState != GameState.game)
            return;
        
        //드래그 위치 업데이트
        dragVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        subDrag = subCam.ScreenToWorldPoint(Input.mousePosition);
        float distacnce = Vector2.Distance(subStart, subDrag);

        if (distacnce >= 0.1f)
        {
            isDrag = true;
        }
        else
        {
            isDrag = false;
        }

        if (isDrag)
        {
            //moveVec 차이값만큼 간격을 두고 이동.
            player.transform.position = new Vector3(dragVec.x - moveVec.x,
                                                     dragVec.y - moveVec.y,
                                                        0f) / player.GetComponent<Player>().moveSpeed;
        }
    }

    private void OnMouseUp()
    {
        startVec = dragVec;
        //드래그 종료
        isDrag = false;
    }

    private void Update()
    {
        cam.ZoomCamera(isDrag);

        if (isDrag)
        {
            GameManager.instance.SetTimeScale(1);
        }
        else
        {
            GameManager.instance.SetTimeScale(0.1f);
        }
    }
}