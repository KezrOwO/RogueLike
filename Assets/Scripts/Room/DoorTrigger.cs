using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;

    private void Reset()
    {
        // auto-setup 
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null)
            return;

        if (roomManager != null)
        {
            roomManager.LoadNextRoom();
        }
    }

    private void Update()
    {
        if(GameManager.isGameOver)
        {
            return;
        }
    }

    public void SetManager(RoomManager manager) => roomManager = manager;
}
