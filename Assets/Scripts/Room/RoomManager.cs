using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Scene references")]
    [SerializeField] private Transform player;                 // Player transform
    [SerializeField] private List<GameObject> roomsInOrder;    // Room1, Room2, Room3...

    [Header("Options")]
    [SerializeField] private bool requireRoomCleared = true;   

    private int currentRoomIndex = 0;
    private RoomController currentRoomController;
    private int next => currentRoomIndex + 1;
    public static bool isRunCompleted = false;

    [SerializeField] private Camera mainCamera;

    private GameManager gameManager;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (player == null)
        {
            Debug.LogError("RoomManager: Player reference is missing.");
            return;
        }

        if (roomsInOrder == null || roomsInOrder.Count == 0)
        {
            Debug.LogError("RoomManager: roomsInOrder is empty.");
            return;
        }

        
        for (int i = 0; i < roomsInOrder.Count; i++)
            roomsInOrder[i].SetActive(false);

        ActivateRoom(0);

        
        WireAllDoorTriggers();
    }

    public void TryGoToNextRoom()
    {
        if (currentRoomController == null)
            return;

        if (requireRoomCleared && !currentRoomController.IsCleared)
            return;

        

        if (next >= roomsInOrder.Count)
        {
            isRunCompleted = true;
            return;
        }

        ActivateRoom(next);
    }

    private void ActivateRoom(int index)
    {
        // Desactivate current room
        if (index != currentRoomIndex && currentRoomIndex >= 0 && currentRoomIndex < roomsInOrder.Count)
            roomsInOrder[currentRoomIndex].SetActive(false);
            gameManager.DestroyProjectil();


        currentRoomIndex = index;

        GameObject roomGO = roomsInOrder[currentRoomIndex];
        roomGO.SetActive(true);

        // Get RoomController
        currentRoomController = roomGO.GetComponent<RoomController>();
        if (currentRoomController == null)
            Debug.LogWarning($"RoomManager: No RoomController on {roomGO.name}");

        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(
                roomGO.transform.position.x,
                roomGO.transform.position.y,
                mainCamera.transform.position.z
            );
        }


        // Spawn player at spawn point
        Transform spawn = roomGO.transform.Find("PlayerSpawn");
        if (spawn == null)
        {
            Debug.LogWarning($"RoomManager: PlayerSpawn not found in {roomGO.name} (expected child named 'PlayerSpawn').");
        }
        else
        {
            player.position = spawn.position;
        }
    }

    private void WireAllDoorTriggers()
    {
        foreach (var room in roomsInOrder)
        {
            var triggers = room.GetComponentsInChildren<DoorTrigger>(true);
            foreach (var t in triggers)
                t.SetManager(this);
        }
    }


}
