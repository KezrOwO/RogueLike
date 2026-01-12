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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnShoot();
        }
    }

    void OnShoot()
    {
        if(attackTimer > attackCooldown)
        {
            attackTimer = 0;
            GameObject intProjectil = Instantiate(Projectil, Aim.position, Aim.rotation); 
            intProjectil.GetComponent<Rigidbody2D>().AddForce(-Aim.up *  fireForce, ForceMode2D.Impulse);
            Debug.Log("Création d'un projectile");
            Destroy(intProjectil, 2f);
        }
    }
}
