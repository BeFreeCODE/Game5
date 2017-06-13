using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Vector2 startVec;
    private Vector2 dragVec;
    private Vector2 moveVec;

    public bool isDrag;

    private void OnMouseDown()
    {
        //마우스 위치 변수저장
        startVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragVec = startVec;

        //마우스 위치와 현재 플레이어 위치 차이 값
        moveVec = new Vector2(startVec.x - player.transform.position.x,
                                startVec.y - player.transform.position.y);
    }

    private void OnMouseDrag()
    {
        if (startVec != dragVec)
        {
            //Drag 시작
            isDrag = true;
        }

        //드래그 위치 업데이트
        dragVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isDrag)
        {
            //moveVec 차이값만큼 간격을 두고 이동.
            //Mathf.Clamp(min, max 지정)
            player.transform.position = new Vector3(Mathf.Clamp(dragVec.x - moveVec.x, -5.2f, 5.2f),
                                                     Mathf.Clamp(dragVec.y - moveVec.y, -9.2f, 9.2f),
                                                 0f);
        }
    }

    private void OnMouseUp()
    {
        //드래그 종료
        isDrag = false;
    }
}