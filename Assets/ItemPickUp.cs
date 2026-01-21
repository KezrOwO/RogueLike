using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private List<ItemData> possiblesDrops;
    [SerializeField] private Vector3 itemOffset = new Vector3(0, 0.5f, 0); // Offset pour afficher l'item au-dessus
    private ItemData selectedItem; // Stocke l'item sélectionné
    private GameObject itemVisual; // Référence à l'objet visuel de l'item


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                if (selectedItem != null)
                {
                    player.AddItem(selectedItem);
                    
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("No item was selected!");
                }
            }
            else
            {
                Debug.LogWarning("PlayerController component not found on " + collision.gameObject.name);
            }
        }
    }

    void Awake()
    {
        if (possiblesDrops == null || possiblesDrops.Count == 0)
        {
            Debug.LogWarning("possiblesDrops list is empty or null on Awake!");
            return;
        }
        selectedItem = selectRandomItem();
        showItem(selectedItem);
    }

    void showItem(ItemData item)
    {
        // Récupérer l'objet "Item" existant dans les enfants
        itemVisual = transform.Find("Item")?.gameObject;
        
        if (itemVisual == null)
        {
            Debug.LogError("Aucun objet enfant nommé 'Item' trouvé dans le prefab!");
            return;
        }
        
        // Configurer le sprite sur le SpriteRenderer existant
        SpriteRenderer spriteRenderer = itemVisual.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = item.itemIcon;
            spriteRenderer.sortingOrder = 1;
            itemVisual.transform.localScale = new Vector3(7.5f, 7.5f, 1f);
        }
        
       
    }

    ItemData selectRandomItem()
    {
        int randomIndex = Random.Range(0, possiblesDrops.Count);
        return possiblesDrops[randomIndex];
    }
}
