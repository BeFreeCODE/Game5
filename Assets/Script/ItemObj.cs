using UnityEngine;
using System.Collections;

public class ItemObj : MonoBehaviour {

    //플레리어와 거리
    private float DistanceToPlayer()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = this.transform.position;

        return Vector3.Distance(myPos, playerPos);

    }

    private void Update()
    {
        if (GameManager.instance.curGameState == GameState.game)
        {
            //거리가 10이상이면 꺼줌.
            if (DistanceToPlayer() >= 10f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
