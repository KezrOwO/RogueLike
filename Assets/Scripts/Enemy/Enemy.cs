using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float health, maxHealth = 10f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float fireForce = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackTimer = 1.5f;
    [SerializeField] private GameObject ProjectilEnemy;
    public Transform Aim;
    private Rigidbody2D rb;
    Transform target;
    Vector2 moveDirecton;
    private Vector2 lastAimDirection = Vector2.zero;


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
            lastAimDirection = direction;

            Vector3 vector = Vector3.left * direction.x + Vector3.down * direction.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector);
        }

        OnShoot();

        attackTimer += Time.deltaTime;
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

    private void OnShoot()
    {
        if (attackTimer > attackCooldown)
        {
            attackTimer = 0;
            GameObject intProjectil = Instantiate(ProjectilEnemy, Aim.position, Aim.rotation);
            intProjectil.GetComponent<Renderer>().material.color = Color.red;
            intProjectil.tag = "Enemy";
            intProjectil.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);
            Debug.Log("Création d'un projectile  ennemiy");
            Destroy(intProjectil, 2f);
        }
    }
}
