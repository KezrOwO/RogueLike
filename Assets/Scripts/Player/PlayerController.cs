using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementspeed = 5f;
    [SerializeField] private float health = 10f;
    private Rigidbody2D body;
    private Vector2 input;

    private Vector2 lastMoveDirection;


    public Transform Aim;
    private bool isWalking = false; 

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        ProccessInput();

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        
    }

    private void FixedUpdate()
    {
        body.velocity = input  * movementspeed;

        if(isWalking)
        {
            Vector3 vector3 = Vector3.left * input.x + Vector3.down * input.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
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

        public void TakeDamage(float damage)
    {
        Debug.Log("Player took " + damage + " damage.");
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Player has been destroyed.");
        }
    }
}
