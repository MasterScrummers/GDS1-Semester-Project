using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject doors;

    public bool empty { get; private set; }
    private int enemyCount;

    void Start()
    {
        foreach(Transform child in DoStatic.GetChildren(enemies.transform))
        {
            Enemy enemy = child.GetComponent<Enemy>();
            if (enemy && child.gameObject.activeInHierarchy)
            {
                enemy.AssignToRoomData(this);
                enemyCount++;
            }
        }
    }

    void Update()
    {
        empty = enemyCount == 0;
        doors.SetActive(!empty);
    }

    public void UpdateEnemyCount()
    {
        enemyCount--;
    }
}
