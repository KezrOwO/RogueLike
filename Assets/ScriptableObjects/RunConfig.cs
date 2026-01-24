using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunConfig", menuName = "ScriptableObjects/RunConfig")]
public class RunConfig : ScriptableObject
{
    [SerializeField] private GameObject roomStart;
    [SerializeField] private GameObject roomBoss;
    [SerializeField] private List<GameObject> roomPrefabs;
    [SerializeField] private int runLength = 5;
    [SerializeField] private bool allowDuplicateRooms = true;

    public GameObject RoomStart => roomStart;
    public GameObject RoomBoss => roomBoss;
    public List<GameObject> RoomPrefabs => roomPrefabs;
    public int RunLength => runLength;
    public bool AllowDuplicateRooms => allowDuplicateRooms;
}
