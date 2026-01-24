using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    [SerializeField] protected float health = 10f;
    [SerializeField] protected float speed = 1.5f;
    [SerializeField] protected float fireForce = 5f;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float attackTimer = 1.5f;
    [SerializeField] protected GameObject ProjectilEnemy;
    [SerializeField] protected float flashDuration = 0.3f;

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
        if(target == null)
        {
            return; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (target)
        {
            HandleMovement(target);
            HandleAim();

        }

        OnShoot();

        attackTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!GameManager.isGameOver && target)
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
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnShoot()
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

    protected virtual void HandleMovement(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        moveDirecton = direction;
    }

    protected virtual void HandleAim()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 vector = Vector3.left * direction.x + Vector3.down * direction.y;
        Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector);
    }

    private IEnumerator FlashRed()
    {

        enemyRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.color = originalColor;

    }
}
