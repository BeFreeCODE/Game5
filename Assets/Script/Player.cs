using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{


    //충돌
    private void OnTriggerEnter(Collider other)
    {
        //방향전환 아이템.
        if (other.transform.tag.Equals("DirItem"))
        {
            int dirNum = other.GetComponent<DirItem>().DIRNUM;
            SetPlayerDirection(dirNum);

            other.gameObject.SetActive(false);
        }
    }

    //플레이어 방향 전환
    private void SetPlayerDirection(int _dirNum)
    {
        Vector3 newDirection = Vector3.zero;

        switch (_dirNum)
        {
            case 1:
                newDirection = this.transform.position + Vector3.up;
                break;
            case 2:
                newDirection = this.transform.position + Vector3.up + Vector3.left;
                break;
            case 3:
                newDirection = this.transform.position + Vector3.left;
                break;
            case 4:
                newDirection = this.transform.position + Vector3.left + Vector3.down;
                break;
            case 5:
                newDirection = this.transform.position + Vector3.down;
                break;
            case 6:
                newDirection = this.transform.position + Vector3.down + Vector3.right;
                break;
            case 7:
                newDirection = this.transform.position + Vector3.right;
                break;
            case 8:
                newDirection = this.transform.position + Vector3.up + Vector3.right;
                break;
        }

        if (newDirection != Vector3.zero)
        {
            this.transform.LookAt(newDirection);
        }
    }
}