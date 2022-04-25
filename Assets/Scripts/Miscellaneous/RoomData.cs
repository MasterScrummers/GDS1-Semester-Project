using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public int enemyCount;
    public Transform enemiesGroup;
    public bool empty;
    private GameObject doors;

    // Start is called before the first frame update
    void Start()
    {
        empty = false;
        enemiesGroup = this.gameObject.transform.GetChild(0);
        enemyCount = enemiesGroup.childCount;
        doors = this.gameObject.transform.GetChild(1).gameObject;
        if(enemyCount <= 0)
        {
            empty = true;
            OpenDoors();
        }
    }

    public void CheckEnemyCount()
    {
        enemyCount -= 1;
        if(enemyCount <= 0)
        {
            empty = true;
            OpenDoors();
        }
    }

    private void OpenDoors()
    {
        Destroy(doors);
    }
}