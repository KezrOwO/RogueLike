using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private List<ItemData> possiblesDrops;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                if (possiblesDrops != null && possiblesDrops.Count > 0)
                {
                    // Sélectionne un item aléatoire dans la liste (25% de chance pour chaque item si vous en avez 4)
                    int randomIndex = Random.Range(0, possiblesDrops.Count);
                    ItemData selectedItem = possiblesDrops[randomIndex];
                    
                    player.AddItem(selectedItem);
                    Debug.Log("Picked up item: " + selectedItem.itemName);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("possiblesDrops list is empty or null!");
                }
            }
            else
            {
                Debug.LogWarning("PlayerController component not found on " + collision.gameObject.name);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
