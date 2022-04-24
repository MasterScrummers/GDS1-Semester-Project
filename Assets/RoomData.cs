using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public int enemyCount;
    public Transform enemiesGroup;
    public bool empty;

    // Start is called before the first frame update
    void Start()
    {
        empty = true;
        enemiesGroup = this.gameObject.transform.GetChild(0);
        enemyCount = enemiesGroup.childCount;
        if(enemyCount > 0)
        {
            empty = false;
        }
    }

    public void CheckEnemyCount()
    {
        enemyCount -= 1;
        if(enemyCount <= 0)
        {
            empty = true;
        }
    }
}
