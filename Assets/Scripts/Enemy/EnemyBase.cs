using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float health = 10f;
    [SerializeField] protected float speed = 1.5f;

    [Header("Shoot")]
    [SerializeField] protected float fireForce = 5f;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float attackTimer = 1.5f;
    [SerializeField] protected GameObject ProjectilEnemy;
    [SerializeField] protected bool canShoot = true;

    [Header("Feedback")]
    [SerializeField] protected float flashDuration = 0.3f;

    public bool IsInvincible { get; private set; }
    public void SetInvincible(bool value) => IsInvincible = value;

    public Transform Aim;

    // ✅ IMPORTANT : protected pour les enfants
    protected Rigidbody2D rb;
    protected Transform target;
    protected Vector2 moveDirecton;

    // ✅ IMPORTANT : les enfants peuvent bloquer le mouvement de base
    protected bool blockMovement = false;

    private SpriteRenderer enemyRenderer;
    private Color originalColor;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;
    }

    protected virtual void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) target = player.transform;
    }

    // ✅ virtual pour pouvoir faire base.Update() dans les enfants
    protected virtual void Update()
    {
        if (GameManager.isGameOver)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (target != null && !blockMovement)
        {
            HandleMovement(target);
            HandleAim();
        }

        if (canShoot && !blockMovement)
            OnShoot();

        attackTimer += Time.deltaTime;
    }

    // ✅ virtual + respecte blockMovement
    protected virtual void FixedUpdate()
    {
        if (GameManager.isGameOver || target == null) return;
        if (blockMovement) return;

        rb.velocity = moveDirecton * speed;
    }

    public void TakeDamage(float damage)
    {
        if (IsInvincible) return;

        health -= damage;
        StartCoroutine(FlashRed());

        if (health <= 0) OnDeath();
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
