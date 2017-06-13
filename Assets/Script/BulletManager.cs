using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private Player player;

    private List<GameObject> bulletList = new List<GameObject>();
    
    private int maxBulletNum = 200;
    
    //총알 생성.
    public void MakeBullets()
    {
        for (int i = 0; i < maxBulletNum; i++)
        {
            GameObject newDot = Instantiate(bulletObj);
            newDot.transform.parent = this.transform;
            newDot.SetActive(false);

            bulletList.Add(newDot);
        }
    }

    //총알 가져오기.
    public GameObject GetBullets()
    {
        foreach(GameObject _bullet in bulletList)
        {
            if(!_bullet.activeInHierarchy)
            {
                return _bullet;
            }
        }
        return null;
    }

    //총알 발사.
    public void FireBullets(Vector3 _pos)
    {
        //총알하나 불러와서
        GameObject fireBullet = GetBullets();
        fireBullet.transform.position = _pos;
        fireBullet.SetActive(true);

        //발사
        fireBullet.GetComponent<Bullet>().SetFireDirection(player.dirNum);
        fireBullet.GetComponent<Bullet>().isFire = true;

        Debug.Log("fire!");
    }
}
