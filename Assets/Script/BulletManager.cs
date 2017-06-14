using UnityEngine;
using System.Collections;

public class BulletManager : Object
{
    //총알 발사.
    public void FireBullets(Vector3 _pos)
    {
        //총알하나 불러와서
        GameObject fireBullet = GetObj();
        fireBullet.transform.position = _pos;
        fireBullet.SetActive(true);

        //발사
        fireBullet.GetComponent<Bullet>().SetFireDirection(player.dirNum);
        fireBullet.GetComponent<Bullet>().isFire = true;
    }
}
