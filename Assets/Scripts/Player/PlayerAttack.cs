using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform Aim;
    public GameObject Projectil;
    [SerializeField] private float fireForce = 10f;
    [SerializeField] private float attackCooldown = 0.2f; 
    [SerializeField] private float attackTimer = 0.5f;
    [SerializeField] private float minAttackCooldown = 0.1f;
    [SerializeField] private float maxAttackCooldown = 1f;

    private Vector2 lastAimDirection = Vector2.zero;

    // Méthode publique pour modifier attackCooldown
    public void ModifyAttackCooldown(float value)
    {
        attackCooldown -= value;
        attackCooldown = Mathf.Clamp(attackCooldown, minAttackCooldown, maxAttackCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldown = Mathf.Clamp(attackCooldown, minAttackCooldown, maxAttackCooldown);

        if (GameManager.isGameOver)
        {
            return;
        }

        attackTimer += Time.deltaTime;

        // Mettre à jour la direction AVANT de tirer
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

        // Mettre à jour la rotation de l'Aim immédiatement
        if (lastAimDirection != Vector2.zero)
        {
            Vector3 vector3 = Vector3.left * lastAimDirection.x + Vector3.down * lastAimDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }

        // Tirer APRÈS avoir mis à jour la direction ET la rotation
        if (lastAimDirection != Vector2.zero && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            OnShoot();
    }

    void OnShoot()
    {
        
        if(attackTimer > attackCooldown )
        {
            attackTimer = 0;
            GameObject intProjectil = Instantiate(Projectil, Aim.position, Aim.rotation); 
            intProjectil.GetComponent<Renderer>().material.color = Color.blue;
            intProjectil.tag = "PlayerProjectile";
            intProjectil.GetComponent<Rigidbody2D>().AddForce(-Aim.up *  fireForce, ForceMode2D.Impulse);
            
            Destroy(intProjectil, 2f);
        }
    }
}
