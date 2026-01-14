using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform Aim;
    public GameObject Projectil;
    public float fireForce = 10f;
    private float attackCooldown = 0.2f;
    private float attackTimer = 0.5f;

    private Vector2 lastAimDirection = Vector2.zero;


    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            lastAimDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            lastAimDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            lastAimDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            lastAimDirection = Vector2.right;
        }

        if (lastAimDirection != Vector2.zero && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            OnShoot();


    }

    void OnShoot()
    {
        if(attackTimer > attackCooldown)
        {
            attackTimer = 0;
            GameObject intProjectil = Instantiate(Projectil, Aim.position, Aim.rotation); 
            intProjectil.GetComponent<Renderer>().material.color = Color.blue;
            intProjectil.tag = "PlayerProjectil";
            intProjectil.GetComponent<Rigidbody2D>().AddForce(-Aim.up *  fireForce, ForceMode2D.Impulse);
            
            Destroy(intProjectil, 2f);
        }
    }
}
