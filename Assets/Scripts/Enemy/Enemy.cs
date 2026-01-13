using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    float health, maxHealth = 10f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {

        Debug.Log("Enemy took " + damage + " damage.");
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
