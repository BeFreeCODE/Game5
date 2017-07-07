using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Grid"))
            return;

        switch (other.transform.name)
        {
            case "1":
                GameManager.instance.SetPlayerType(0);          
                break;
            case "2":
                GameManager.instance.SetPlayerType(1);
                break;
            case "3":
                GameManager.instance.SetPlayerType(2);
                break;
            case "4":
                GameManager.instance.SetPlayerType(3);
                break;
            case "5":
                GameManager.instance.SetPlayerType(4);
                break;
            case "6":
                GameManager.instance.SetPlayerType(5);
                break;
            case "7":
                GameManager.instance.SetPlayerType(6);
                 break;
        }
        other.transform.localScale = new Vector3(1280f,1280f,1f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Grid"))
            return;
        other.transform.localScale = new Vector3(640f, 640f, 1f);
    }
}
