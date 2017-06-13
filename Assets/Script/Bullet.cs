using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public bool isFire = false;

    private Vector2 fireDirection = Vector2.up;

    //발사속도
    private float bulletSpeed = 3f;

	void Update ()
    {
	    if(isFire)
        {
            FireBullet();
        }
	}

    private void FireBullet()
    {
        this.transform.Translate(fireDirection.normalized * Time.deltaTime * bulletSpeed, Space.Self);
    }

    public void SetFireDirection(int _num)
    {
        switch (_num)
        {
            case 1:
                fireDirection = Vector3.up;
                break;
            case 2:
                fireDirection = Vector3.up + Vector3.left;
                break;
            case 3:
                fireDirection = Vector3.left;
                break;
            case 4:
                fireDirection = Vector3.left + Vector3.down;
                break;
            case 5:
                fireDirection = Vector3.down;
                break;
            case 6:
                fireDirection = Vector3.down + Vector3.right;
                break;
            case 7:
                fireDirection = Vector3.right;
                break;
            case 8:
                fireDirection = Vector3.up + Vector3.right;
                break;
        }
    }
}
