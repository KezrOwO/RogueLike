using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Scene references")]
    [SerializeField] private Transform player;                 // Player transform

    [Header("Options")]
    [SerializeField] private bool requireRoomCleared = true;   

    private int currentRoomIndex = 0;
    private RoomController currentRoomController;
    public static bool isRunCompleted = false;

    [SerializeField] private Camera mainCamera;

    private GameManager gameManager;

    [SerializeField] private RunGenerator runGenerator;
    private List<GameObject> runSequence;
    private GameObject currentRoomInstance;
    private bool isTransitioning = false;

    private void Awake()
    {

        gameManager = FindObjectOfType<GameManager>();
        if (player == null)
        {
            Debug.LogError("RoomManager: Player reference is missing.");
            return;
        }

        if(runGenerator == null)
        {
                Debug.LogError("RoomManager: RunGenerator not found in the scene.");
                return;
        }
        runSequence = runGenerator.GenerateRun();

        if (runSequence != null && runSequence.Count >0)
        {
            Debug.Log("RoomManager: Run sequence generated with " + runSequence.Count + " rooms.");
        }

        
        currentRoomIndex = 0;
        SpawnRoom(runSequence[0]);
        TeleportPlayerToSpawn();    
        WireDoorTriggers();
    }

    public void LoadNextRoom()
    {
        if (currentRoomController == null )
            return;

        if (requireRoomCleared && !currentRoomController.IsCleared)
            return;

        if(isTransitioning)
            return;
            
        isTransitioning = true;

        currentRoomIndex++;

        if (currentRoomIndex >= runSequence.Count)
        {
            isRunCompleted = true;
            isTransitioning = false;
            Debug.Log("RoomManager: Run completed!");
            return;
        }

        if(currentRoomInstance != null)
        {
            Destroy(currentRoomInstance.gameObject);
        }       

        if(gameManager != null)
        {
            gameManager.DestroyProjectil();
        }
        SpawnRoom(runSequence[currentRoomIndex]); 
        TeleportPlayerToSpawn();
        WireDoorTriggers();
        isTransitioning = false;
    }

    private void SpawnRoom(GameObject roomPrefab)
    {
        if (roomPrefab == null)
        {
            Debug.LogError("RoomManager: Cannot spawn null room prefab.");
            return;
        }

        currentRoomInstance = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        currentRoomController = currentRoomInstance.GetComponent<RoomController>();
        if (currentRoomController == null)
        {
            Debug.LogWarning($"RoomManager: No RoomController on {roomPrefab.name}");
        }

        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(
                currentRoomInstance.transform.position.x,
                currentRoomInstance.transform.position.y,
                mainCamera.transform.position.z
            );
        }
    }

    private void TeleportPlayerToSpawn()
    {
        if (currentRoomInstance == null || player == null)
            return;

        Transform spawn = currentRoomInstance.transform.Find("PlayerSpawn");
        if (spawn == null)
        {
            Debug.LogWarning($"RoomManager: PlayerSpawn not found in {currentRoomInstance.name}");
            return;
        }

        player.position = spawn.position;
    }

    private void WireDoorTriggers()
    {
        if (currentRoomInstance == null)
            return;

        var triggers = currentRoomInstance.GetComponentsInChildren<DoorTrigger>(true);
        foreach (var t in triggers)
            t.SetManager(this);
    }
}
