using System.Collections;
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

    private void Start()
    {

        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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


        if(runSequence == null || runSequence.Count == 0)
            {
            Debug.LogError("RoomManager: Run sequence is null or empty.");
            return;
        }

        currentRoomIndex = 0;
        SpawnRoom(runSequence[0]);
        InitializeCurrentRoom();
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
            
            //  Appeler la victoire
            if (gameManager != null)
            {
                gameManager.TriggerVictory();
            }
            
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
        InitializeCurrentRoom();
        TeleportPlayerToSpawn();
        WireDoorTriggers();
        isTransitioning = false;

    }

    private void InitializeCurrentRoom()
    {
        if (currentRoomController == null)
        {
            Debug.LogWarning("RoomManager: Cannot initialize room - RoomController is null.");
            return;
        }

        int lastIndex = runSequence.Count - 1;

        if (currentRoomIndex == 0)
        {
            currentRoomController.InitStartRoom();
        }
        else if (currentRoomIndex == lastIndex)
        {
            currentRoomController.InitEndRoom();
        }
        else
        {
            currentRoomController.SetRoomIndex(currentRoomIndex, runSequence.Count);
            currentRoomController.InitRoom();
        }
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
        
        // SOLUTION : Utiliser le Rigidbody2D pour téléporter proprement
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Réinitialiser la vélocité pour éviter tout mouvement résiduel
            playerRb.velocity = Vector2.zero;
            // Utiliser MovePosition pour déplacer via le Rigidbody2D
            playerRb.position = spawn.position;
        }
        else
        {
            Debug.LogWarning("RoomManager: Rigidbody2D manquant sur le joueur.");
            // Fallback si pas de Rigidbody2D (ne devrait pas arriver)
            player.position = spawn.position;
        }
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
