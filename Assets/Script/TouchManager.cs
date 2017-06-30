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

    private Vector2 dragVec;        //마우스 드래그 위치
    private Vector2 startVec;
    private Vector2 calPos;         //차이 벡터

    //드래그 상태
    public bool isDrag;

    [SerializeField]
    GameObject startDisPlay, dragDisPlay;

    public float distance = 0f;


    private void Update()
    {
        cam.ZoomCamera(isDrag);

        if (isDrag)
        {
            //드래그 길이 체크.
            distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(startVec), Camera.main.ScreenToWorldPoint(dragVec));
            
            //드래그 표시
            if (distance >= 2)
            {
                distance = 2;
                dragDisPlay.transform.position =  startDisPlay.transform.position 
                    + ((Camera.main.ScreenToWorldPoint(dragVec) + Vector3.forward * 9) - startDisPlay.transform.position).normalized * 2f;
            }
            else
            {
                dragDisPlay.transform.position = Camera.main.ScreenToWorldPoint(dragVec) + Vector3.forward * 9;

            }
            //드래그 거리만큼 시간조정
            GameManager.instance.SetTimeScale(distance * 0.5f);

            dragDisPlay.SetActive(true);

            calPos = new Vector2(dragVec.x - startVec.x,
                        dragVec.y - startVec.y);

            calPos = calPos.normalized;

            //calPos 방향으로 moveSpeed로 이동
            player.transform.Translate(calPos * Time.deltaTime * player.GetComponent<Player>().moveSpeed,
                                        Space.World);

        }
        else
        {
            GameManager.instance.SetTimeScale(0f);
            dragDisPlay.SetActive(false);

            distance = 0;
        }
    }

    private void OnMouseDown()
    {
        #region 게임상태전환 
        //게임상태 전환
        if (GameManager.instance.curGameState == GameState.main)
        {
            GameManager.instance.StateTransition(GameState.game);
        }
        #endregion

        if (GameManager.instance.curGameState != GameState.game)
            return;

        if (!isDrag)
        {
            startVec = Input.mousePosition;

            //터치 시작점 표시
            startDisPlay.transform.position = Camera.main.ScreenToWorldPoint(startVec);
            startDisPlay.transform.position += Vector3.forward * 9f;
            startDisPlay.gameObject.SetActive(true);

            //마우스 위치 변수저장
            subStart = subCam.ScreenToWorldPoint(Input.mousePosition);
            subDrag = subStart;
        }

    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.curGameState != GameState.game)
            return;

        //드래그 위치 업데이트
        dragVec = Input.mousePosition;
        

        subDrag = subCam.ScreenToWorldPoint(Input.mousePosition);
        float subdistacnce = Vector2.Distance(subStart, subDrag);


        if (subdistacnce >= 0.1f)
        {
            isDrag = true;
        }
        else
        {
            isDrag = false;
        }
    }

    private void OnMouseUp()
    {
        //드래그 종료
        isDrag = false;
        startDisPlay.gameObject.SetActive(false);
    }


}