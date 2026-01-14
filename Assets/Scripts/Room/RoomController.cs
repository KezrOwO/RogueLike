using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemiesInRoom;
    [SerializeField] private int enemySpawnCount = 2;
    [SerializeField] private int enemyAliveCount;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float roomWidth = 28;
    [SerializeField] private float roomHeight = 15f;


    private enum States {Entering, Fighting, Cleared}
    private States currentState;

    private Transform doors;
    private Collider2D[] doorBlockers;  // Colliders solides qui bloquent
    private Collider2D[] doorTriggers;  // Triggers pour les transitions

    public bool IsCleared => currentState == States.Cleared; 

    private void Awake()
    {

        doors = transform.Find("Doors");

        if (doors == null)
        {
            Debug.LogError("Doors not found! Make sure there is a child GameObject named 'Doors'.");
        }
        else
        {
            SetupDoorColliders();
        }

        currentState = States.Entering; // Set initial state to "Entering"

        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogError("Enemies array is empty! Please assign enemy prefabs in the Inspector.");
            return;
        }

        if (enemiesInRoom == null)
        {
            enemiesInRoom = new List<GameObject>();
        }

        for (int i = 0; i < enemySpawnCount; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemies.Length);
            Vector3 randomPosition = GetRandomPositionInRoom();
            GameObject enemyInstance = Instantiate(enemies[randomEnemyIndex], randomPosition, Quaternion.identity);
            enemyInstance.transform.parent = transform;
            enemiesInRoom.Add(enemyInstance);
        }

        

        CloseDoors(); // Close doors when entering the room

        // Change state to Fighting after spawning enemies
        currentState = States.Fighting;


    }

    private void SetupDoorColliders()
    {
        List<Collider2D> blockers = new List<Collider2D>();
        List<Collider2D> triggers = new List<Collider2D>();

        // Parcourir chaque porte (DoorN, DoorS, DoorE, DoorW)
        foreach (Transform door in doors)
        {
            Collider2D[] colliders = door.GetComponents<Collider2D>();
            
            foreach (Collider2D col in colliders)
            {
                if (col.isTrigger)
                {
                    triggers.Add(col);
                }
                else
                {
                    blockers.Add(col);
                }
            }
        }

        doorBlockers = blockers.ToArray();
        doorTriggers = triggers.ToArray();

        Debug.Log($"Found {doorBlockers.Length} door blockers and {doorTriggers.Length} door triggers.");
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

        for (int i = enemiesInRoom.Count - 1; i >= 0; i--)
        {
            if (enemiesInRoom[i] == null)
            {
                enemiesInRoom.RemoveAt(i);
            }
        }

        enemyAliveCount = enemiesInRoom.Count;

        if (enemyAliveCount == 0 && currentState == States.Fighting)
        {
            Debug.Log("All enemies defeated!");
            changeStateCleared();
            
        }

    }

    void changeStateCleared()
    {
        
        currentState = States.Cleared;
        openDoor();
    }

    void CloseDoors()
    {
        // Activer les colliders solides (bloquer le passage)
        if (doorBlockers != null)
        {
            foreach (Collider2D blocker in doorBlockers)
            {
                blocker.enabled = true;
            }
        }

        // Désactiver les triggers (pas de transition possible)
        if (doorTriggers != null)
        {
            foreach (Collider2D trigger in doorTriggers)
            {
                trigger.enabled = false;
            }
        }

        
    }

    void openDoor()
    {
        
        
        // Désactiver les colliders solides (ouvrir le passage)
        if (doorBlockers != null)
        {
            foreach (Collider2D blocker in doorBlockers)
            {
                blocker.enabled = false;
            }
        }

        // Activer les triggers (autoriser les transitions)
        if (doorTriggers != null)
        {
            foreach (Collider2D trigger in doorTriggers)
            {
                trigger.enabled = true;
            }
        }

        
    }

}
