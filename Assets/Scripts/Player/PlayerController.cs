using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movementspeed = 5f;
    private Rigidbody2D body;
    private Vector2 input;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        body.velocity = input  * movementspeed;
    }
}
