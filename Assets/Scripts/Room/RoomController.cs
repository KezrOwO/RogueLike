using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemiesInRoom;
    [SerializeField] private int numberOfEnemies = 2;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float roomWidth = 10f;
    [SerializeField] private float roomHeight = 10f;

    private string[] states= {"Entering", "Fighting", "Cleared"};
    private string currentState;

    private void Awake()
    {

        currentState = states[0]; // Set initial state to "Entering"

        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogError("Enemies array is empty! Please assign enemy prefabs in the Inspector.");
            return;
        }

        if (enemiesInRoom == null)
        {
            enemiesInRoom = new List<GameObject>();
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemies.Length);
            Vector3 randomPosition = GetRandomPositionInRoom();
            GameObject enemyInstance = Instantiate(enemies[randomEnemyIndex], randomPosition, Quaternion.identity);
            enemyInstance.transform.parent = transform;
            enemiesInRoom.Add(enemyInstance);
        }

        if(numberOfEnemies > 0)
        {
            currentState = states[1]; // Change state to "Fighting" if there are enemies
        }
    }

    private Vector3 GetRandomPositionInRoom()
    {
        float randomX = Random.Range(-roomWidth / 2f, roomWidth / 2f);
        float randomY = Random.Range(-roomHeight / 2f, roomHeight / 2f);
        return transform.position + new Vector3(randomX, randomY, 0f);
    }


    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = enemiesInRoom.Count - 1; i >= 0; i--)
        {
            if (enemiesInRoom[i] == null)
            {
                enemiesInRoom.RemoveAt(i);
            }
        }

        numberOfEnemies = enemiesInRoom.Count;
        
        if(numberOfEnemies == 0 && currentState == states[1])
        {
            currentState = states[2]; // Change state to "Cleared" when all enemies are defeated
            
        }

    }
}
