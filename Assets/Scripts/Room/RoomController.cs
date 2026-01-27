using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesInRoom;
    [SerializeField] private int enemyAliveCount;
    
    [SerializeField] private GameObject itemPickUpPrefab;
    [SerializeField] private float roomWidth = 28;
    [SerializeField] private float roomHeight = 15f;
   
    
    [SerializeField] private int minEnemies = 3;
    [SerializeField] private int maxEnemies = 6;
    private Transform enemySpawnParent;
    private Transform[] spawnEnemies;
    [SerializeField] private GameObject[] enemiesPrefab;

    private enum States {Entering, Fighting, Cleared}
    private States currentState;

    private Transform doors;
    private Collider2D[] doorBlockers;
    private Collider2D[] doorTriggers;

    private bool hasSpawnedEnemies = false; 

    public bool IsCleared => currentState == States.Cleared;

    private void Awake()
    {
        doors = transform.Find("Doors");
        enemySpawnParent = transform.Find("EnemySpawns");

        spawnEnemies = new Transform[enemySpawnParent.childCount];
        for (int i = 0; i < enemySpawnParent.childCount; i++)
        {
            spawnEnemies[i] = enemySpawnParent.GetChild(i);
        }

        if (doors == null)
        {
            Debug.LogError("Doors not found!");
            return;
        }

        SetupDoorColliders();
        currentState = States.Entering;

    }

    private void OnEnable()
    {
        CloseDoors();
        
        

        // Block multiple spawns
        if (hasSpawnedEnemies)
        {
            Debug.Log("Enemies already spawned, skipping OnEnable spawn.");
            return;
        }
        if (enemiesInRoom == null)
        {
            enemiesInRoom = new List<GameObject>();
        }

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

        if(enemyCount > spawnEnemies.Length)
        {
            enemyCount = Mathf.Min(spawnEnemies.Length, enemyCount);
        }

        for (int i = spawnEnemies.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i+1);
            Swap(spawnEnemies, i, j);
        }

        if(enemiesPrefab.Length == 0)
        {
            Debug.LogError("Enemies Prefab array is empty! Please assign enemy prefabs in the Inspector.");
            return;
        }
        Debug.Log("nombre de prefab " + enemiesPrefab.Length );
        for (int i = 0 ; i < enemyCount; i++)
        {
            Transform spawnPoint = spawnEnemies[i];
            GameObject enemyPrefab = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyInstance.transform.SetParent(transform);
            enemiesInRoom.Add(enemyInstance);

        }

      /*  for (int i = 0; i < enemySpawnCount; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemies.Length);
            Vector3 randomPosition = GetRandomPositionInRoom();
            GameObject enemyInstance = Instantiate(enemies[randomEnemyIndex], randomPosition, Quaternion.identity);
            enemyInstance.transform.parent = transform;
            enemiesInRoom.Add(enemyInstance);
        }*/

        hasSpawnedEnemies = true; 
        currentState = States.Fighting;
    }

    private void Swap(Transform[] array, int indexA, int indexB)
    {
        Transform temp = array[indexA];
        array[indexA] = array[indexB];
        array[indexB] = temp;
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
    }

    private Vector3 GetRandomPositionInRoom()
    {
        float randomX = Random.Range(-roomWidth / 2f, roomWidth / 2f);
        float randomY = Random.Range(-roomHeight / 2f, roomHeight / 2f);
        return transform.position + new Vector3(randomX, randomY, 0f);
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
            changeStateCleared(); 
            enemiesInRoom.Clear();
            spawnItemPickUp();
        }

    }

    void spawnItemPickUp()
    {
        Transform itemSpawn = transform.Find("ItemSpawn");
        if (itemSpawn != null && itemPickUpPrefab != null)
        {
            GameObject item = Instantiate(itemPickUpPrefab, itemSpawn.position, Quaternion.identity);
            item.transform.parent = transform;
        }
        else
        {
            if (itemSpawn == null)
                Debug.LogError("ItemSpawn transform not found in room!");
            if (itemPickUpPrefab == null)
                Debug.LogError("ItemPickUp prefab not assigned in Inspector!");
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
