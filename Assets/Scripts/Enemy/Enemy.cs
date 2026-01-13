using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float health, maxHealth = 10f;
    [SerializeField] private float speed = 1.5f;
    private Rigidbody2D rb;
    Transform target;
    Vector2 moveDirecton;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirecton = direction;
        }
    }

    private void FixedUpdate()
    {
        if (target) { 
            rb.velocity = new Vector2(moveDirecton.x, moveDirecton.y) * speed;
        }
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
