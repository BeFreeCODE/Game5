using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour {

    void Update () {
	    if(Input.GetMouseButton(0))
        {
            Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray.origin,ray.direction * 10f,out hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.transform.tag.Equals("PopUp"))
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
	}
}
