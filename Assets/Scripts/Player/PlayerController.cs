using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movementspeed = 5f;
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

        if((moveX == 0 &&  moveY == 0) && (input.x != 0 || input.y != 0))
        {
            isWalking = false;
            lastMoveDirection = input;
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if(moveX !=0 || moveY != 0)
        {
            isWalking=true;
        }
    }
}
