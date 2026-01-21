using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementspeed = 5f;
    [SerializeField] private float health = 10f;
    [SerializeField] private float flashDuration = 0.3f;
    private bool isDead = false;
    private Rigidbody2D body;
    private Vector2 input;

    private Vector2 lastMoveDirection;

    private SpriteRenderer playerRenderer;
    private Color originalColor;

    public Transform Aim;
    private bool isWalking = false; 

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerRenderer.color;
    }


    void Update()
    {
        if (!isDead)
        {
            ProccessInput();

            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
        }
        else
        {
            input = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            body.velocity = input * movementspeed;

            if (isWalking)
            {
                Vector3 vector3 = Vector3.left * input.x + Vector3.down * input.y;
                Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
            }
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    void ProccessInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 aimDirection = Vector2.zero;

        

        if (Input.GetKey(KeyCode.UpArrow))
        {
            aimDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            aimDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            aimDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            aimDirection = Vector2.right;
        }

        if (aimDirection != Vector2.zero)
        {
            Vector3 vector3 = Vector3.left * aimDirection.x + Vector3.down * aimDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }

    public void AddItem(ItemData item)
    {
        
        switch (item.itemType)
        {
            case ItemData.ItemType.HpUp:
                health += item.itemValue;
                break;
            case ItemData.ItemType.DamageUp:
                Weapon.damagePlayer += item.itemValue;
                break;
            case ItemData.ItemType.SpeedUp:
                movementspeed += item.itemValue;
                break;
            case ItemData.ItemType.AttackSpeedUp:
                PlayerAttack.attackCooldown -= item.itemValue;
                break;
            default:
                Debug.LogWarning("Unknown item type: " + item.itemType);
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            isDead = true;
            playerRenderer.enabled = false;
            body.velocity = Vector2.zero;
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.TriggerGameOver();
            }
        }
    }

    private IEnumerator FlashRed()
    {
        
        playerRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        playerRenderer.color = originalColor;
        
    }

    void OnGUI()
    {
        if (isDead)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = Color.blue;
            style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Game Over", style);
        }

        if(RoomManager.isRunCompleted)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = Color.green;
            style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 25, 300, 50), "Run Completed!", style);
        }
    }
}
