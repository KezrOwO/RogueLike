using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    [SerializeField] protected float health = 10f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float fireForce = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackTimer = 1.5f;
    [SerializeField] private GameObject ProjectilEnemy;
    [SerializeField] private float flashDuration = 0.3f;

    public Transform Aim;
    private Rigidbody2D rb;
    Transform target;
    Vector2 moveDirecton;

    private SpriteRenderer enemyRenderer;
    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver)
        {
            rb.velocity = Vector2.zero;
            speed = 0;
            return;
        }

        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirecton = direction;

            Vector3 vector = Vector3.left * direction.x + Vector3.down * direction.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector);
        }

        OnShoot();

        attackTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (target)
        {
            rb.velocity = new Vector2(moveDirecton.x, moveDirecton.y) * speed;
        }


    }

    public void TakeDamage(float damage)
    {


        health -= damage;

        StartCoroutine(FlashRed());
        if (health <= 0)
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
            intProjectil.tag = "EnemyProjectile";
            intProjectil.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);

            Destroy(intProjectil, 2f);
        }
    }

    private IEnumerator FlashRed()
    {

        enemyRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.color = originalColor;

    }
}
