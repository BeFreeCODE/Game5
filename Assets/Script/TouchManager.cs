using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    /// <summary>
    /// 기존 카메라(SmoothCamera)로 좌표비교하여 Drag를하면 
    /// 카메라 움직임에 따라 좌표가 정확하지 않아 subCam으로 Drag 보조
    /// </summary>
    [SerializeField]
    private Camera subCam;

    [SerializeField]
    private SmoothCamera cam;

    [SerializeField]
    private GameObject player;

    public GameObject joyStick;
    public GameObject dirStick;

    private Vector3 joyDragVec, dirDragVec;        //마우스 드래그 위치
    private Vector3 startVec, startVec2;
    private Vector3 calPos, calPos2;         //차이 벡터

    //드래그 상태
    public bool joyDrag, dirDrag;

    public float distance = 0f;
    public float limitDistance = 20f;

    int joyNum = -1, dirNum = -1;

    private void Update()
    {
        if (GameManager.instance.curGameState == GameState.store)
            return;

        cam.ZoomCamera(joyDrag);
        PlayerMove();
        OnTouch();
    }

    private void Start()
    {
        startVec = joyStick.transform.position;
        startVec2 = dirStick.transform.position;
    }

    //게임상태 터치조작
    private void OnTouch()
    {
        #region 유니티에디터
        //#if UNITY_EDITOR
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (GameManager.instance.curGameState == GameState.main)
        //    {
        //        GameManager.instance.StateTransition(GameState.game);
        //    }
        //    else if (GameManager.instance.curGameState == GameState.game)
        //    {
        //        Ray ray = subCam.ScreenPointToRay(Input.mousePosition);

        //        RaycastHit hit;

        //        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        //        {
        //            if (hit.collider.transform.tag.Equals("JoyStick"))
        //            {
        //                joyDrag = true;
        //            }
        //            else if (hit.collider.transform.tag.Equals("DirectionStick"))
        //            {
        //                dirDrag = true;
        //            }
        //        }
        //    }
        //}

        //if (Input.GetMouseButton(0))
        //{
        //    if (joyDrag)
        //    {
        //        joyDragVec = subCam.ScreenToWorldPoint(Input.mousePosition);
        //    }

        //    if (dirDrag)
        //    {
        //        dirDragVec = subCam.ScreenToWorldPoint(Input.mousePosition);
        //    }
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    joyDrag = false; dirDrag = false;
        //    joyDragVec = startVec; dirDragVec = startVec2;
        //    joyStick.transform.position = startVec;
        //    dirStick.transform.position = startVec2;
        //}
        //#else
#endregion

        if (Input.touchCount > 0)
        {
            Touch[] _touch = Input.touches;

            for (int i = 0; i < Input.touchCount; i++)
            {
                //터치시작
                if (_touch[i].phase.Equals(TouchPhase.Began))
                {
                    if (GameManager.instance.curGameState == GameState.main)
                    {
                        Ray ray = subCam.ScreenPointToRay(_touch[i].position);

                        RaycastHit hit;

                        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
                        {
                            //버튼 말고 배경을 누르면 시작
                            if (!hit.collider.tag.Equals("Button"))
                            {
                                GameManager.instance.StateTransition(GameState.game);
                                SoundManager.instance.PlayEffectSound(0);
                                SoundManager.instance.PlayBGMSound();
                            }
                        }
                    }
                    else if(GameManager.instance.curGameState == GameState.game)
                    {
                        Ray ray = subCam.ScreenPointToRay(_touch[i].position);

                        RaycastHit hit;

                        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
                        {
                            if (hit.collider.tag.Equals("JoyStick"))
                            {
                                joyNum = i;
                                joyDrag = true;
                            }
                            else if (hit.collider.tag.Equals("DirectionStick"))
                            {
                                dirNum = i;
                                dirDrag = true;
                            }
                        }
                    }
                }

                //드래그
                if (_touch[i].phase.Equals(TouchPhase.Moved))
                {
                    if(joyDrag && i == joyNum)
                    {
                        joyDragVec = subCam.ScreenToWorldPoint(_touch[i].position);                       
                    }
                    else if(dirDrag && i == dirNum)
                    {
                        dirDragVec = subCam.ScreenToWorldPoint(_touch[i].position);
                    }
                    ////현태 드래그 위치에 ray를 쏨
                    //Ray ray = subCam.ScreenPointToRay(_touch[i].position);

                    //RaycastHit hit;

                    //if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
                    //{
                    //    if (hit.collider.transform.tag.Equals("JoyStick"))
                    //    {
                    //        joyDragVec = subCam.ScreenToWorldPoint(_touch[i].position);
                    //        joyNum = i;
                    //        joyDrag = true;
                    //    }
                    //    if (hit.collider.transform.tag.Equals("DirectionStick"))
                    //    {
                    //        dirDragVec = subCam.ScreenToWorldPoint(_touch[i].position);
                    //        dirNum = i;
                    //        dirDrag = true;
                    //    }
                    //}
                }

                //터치 뗄때
                if (_touch[i].phase.Equals(TouchPhase.Ended))
                {
                    if (i == joyNum)
                    {
                        joyDrag = false;
                        joyDragVec = startVec;
                        joyStick.transform.position = startVec;
                        joyNum = -1;
                        dirNum = 0;
                    }
                    else if(i == dirNum)
                    {
                        dirDrag = false;
                        dirDragVec = startVec2;
                        dirStick.transform.position = startVec2;
                        dirNum = -1;
                        joyNum = 0; 
                    }                
                }
            }
        }
//#endif
    }

    private void PlayerMove()
    {
        //이동 조이스틱 드래그 상태일때
        if (joyDrag)
        {
            //드래그 길이 체크.
            distance = Vector2.Distance(startVec, joyDragVec);

            //드래그 표시
            if (distance >= 2)
            {
                distance = 2;
                joyStick.transform.position = joyDragVec + Vector3.forward * 9;
                //joyStick.transform.position = ((joyDragVec + Vector3.forward * 9) - startVec).normalized * 2f
                //                             + startVec;
            }
            else
            {
                joyStick.transform.position = joyDragVec + Vector3.forward * 9;
            }
            //드래그 거리만큼 시간조정
            GameManager.instance.SetTimeScale(distance * 0.5f);

            calPos = joyDragVec - startVec;

            calPos = calPos.normalized;

            //calPos 방향으로 moveSpeed로 이동
            Vector2 movePos = calPos * Time.deltaTime * player.GetComponent<Player>().moveSpeed;

            //이동제한.
            player.transform.Translate(movePos, Space.World);
            player.transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, limitDistance * -1, limitDistance),
                                                     Mathf.Clamp(player.transform.position.y, limitDistance * -1, limitDistance),
                                                     0f);
        }
        else
        {
            GameManager.instance.SetTimeScale(0f);

            distance = 0;
        }

        if (dirDrag)
        {
            dirStick.transform.position = dirDragVec + Vector3.forward * 9;

            calPos2 = dirDragVec - startVec2;
            calPos2 = calPos2.normalized;

            player.GetComponent<Player>().SetPlayerDirection(calPos2);
        }

    }
}