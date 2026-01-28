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
   
    
    [SerializeField] private int baseMinEnemies = 2;
    [SerializeField] private int baseMaxEnemies = 4;
    [SerializeField] private int minEnemyScaling = 4;
    [SerializeField] private int maxEnemyScaling = 7;
    [SerializeField] private int startEnemyCount = 2;
    [SerializeField] private int bossEnemyCount = 8;
    private Transform enemySpawnParent;
    private Transform[] spawnEnemies;
    [SerializeField] private GameObject[] enemiesPrefab;

    private enum States {Entering, Fighting, Cleared}
    private States currentState;

    private Transform doors;
    private Collider2D[] doorBlockers;
    private Collider2D[] doorTriggers;

    private bool hasSpawnedEnemies = false; 

    private int normalIndex;
    private int normalTotalRooms;

    [SerializeField] private AnimationCurve difficultyCurve;

    public bool IsCleared => currentState == States.Cleared;

    private void Awake()
    {
        doors = transform.Find("Doors");
        enemySpawnParent = transform.Find("EnemySpawns");

        if(enemySpawnParent == null)
        {
            Debug.LogError("EnemySpawns parent not found!");
            return;
        }

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

    // Set the room index and total normal rooms for difficulty scaling
    // Start and end rooms are excluded from normal room count
    // index is 1-based for normal rooms
    // Example: For a run with 5 normal rooms, index 1-5, totalRooms = 7 (including start and end)
    public void SetRoomIndex(int index, int totalRooms)
    {
        normalIndex = index - 1;
        normalTotalRooms = totalRooms - 2;
    }

    // Initialise la room et spawn les ennemis
    // Doit être appelé par RoomManager après SetRoomIndex
    public void InitRoom()
    {
        
        // Block multiple spawns
        if (hasSpawnedEnemies)
        {
            Debug.Log("Enemies already spawned, skipping InitRoom spawn.");
            return;
        }

        CloseDoors();

        if (enemiesInRoom == null)
        {
            enemiesInRoom = new List<GameObject>();
        }

        difficultyCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.6f, 0.55f),  new Keyframe(1.0f, 1.0f));
        for (int i = 0; i < difficultyCurve.keys.Length; i++)
        {
            difficultyCurve.SmoothTangents(i, 0f);
        }

        float progress = (float)normalIndex / Mathf.Max(1, normalTotalRooms - 1);
        float difficultyMultiplier = difficultyCurve.Evaluate(progress);

        float minEnemiesFloat = baseMinEnemies + difficultyMultiplier * minEnemyScaling;

        float maxEnemiesFloat = baseMaxEnemies + difficultyMultiplier * maxEnemyScaling;


        int minEnemiesInt = Mathf.Clamp(Mathf.RoundToInt(minEnemiesFloat), 1, spawnEnemies.Length);

        int maxEnemiesInt = Mathf.Clamp(Mathf.RoundToInt(maxEnemiesFloat), minEnemiesInt, spawnEnemies.Length);

        int enemyCount = Random.Range(minEnemiesInt, maxEnemiesInt + 1);
  


        for (int i = spawnEnemies.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Swap(spawnEnemies, i, j);
        }

        if (enemiesPrefab.Length == 0)
        {
            Debug.LogError("Enemies Prefab array is empty! Please assign enemy prefabs in the Inspector.");
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = spawnEnemies[i];
            GameObject enemyPrefab = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyInstance.transform.SetParent(transform);
            enemiesInRoom.Add(enemyInstance);
        }
        Debug.Log("Spawned " + enemyCount + " enemies at room index." + normalIndex);
        hasSpawnedEnemies = true;
        currentState = States.Fighting;
    }

    public void InitStartRoom()
    {
        if (hasSpawnedEnemies)
        {
            Debug.Log("Enemies already spawned, skipping InitRoom spawn.");
            return;
        }

        CloseDoors();

        if (enemiesInRoom == null)
        {
            enemiesInRoom = new List<GameObject>();
        }
       

        int enemyCount = startEnemyCount;

        for (int i = spawnEnemies.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Swap(spawnEnemies, i, j);
        }

        if (enemiesPrefab.Length == 0)
        {
            Debug.LogError("Enemies Prefab array is empty! Please assign enemy prefabs in the Inspector.");
            return;
        }

        Debug.Log("nombre de prefab " + enemiesPrefab.Length);
        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = spawnEnemies[i];
            GameObject enemyPrefab = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyInstance.transform.SetParent(transform);
            enemiesInRoom.Add(enemyInstance);
        }

        hasSpawnedEnemies = true;
        currentState = States.Fighting;
    }

    public void InitEndRoom()
    {
        Debug.Log("Initializing End Room with Boss Enemies.");
        if (hasSpawnedEnemies)
        {
            Debug.Log("Enemies already spawned, skipping InitRoom spawn.");
            return;
        }
        CloseDoors();
        if (enemiesInRoom == null)
        {
            enemiesInRoom = new List<GameObject>();
        }

        int enemyCount = bossEnemyCount;
        for (int i = spawnEnemies.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Swap(spawnEnemies, i, j);
        }
        if (enemiesPrefab.Length == 0)
        {
            Debug.LogError("Enemies Prefab array is empty! Please assign enemy prefabs in the Inspector.");
            return;
        }
        Debug.Log("nombre de prefab " + enemiesPrefab.Length);
        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = spawnEnemies[i];
            GameObject enemyPrefab = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyInstance.transform.SetParent(transform);
            enemiesInRoom.Add(enemyInstance);
        }
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
