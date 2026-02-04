using UnityEngine;

public class EnemyFastMelee : EnemyBase
{
    [Header("Melee")]
    [SerializeField] private float contactDamage = 1f;
    [SerializeField] private float damageTick = 0.35f; 

    private float damageTimer;

    protected override void OnShoot()
    {
        // no shooting behavior for melee enemy
    }

    protected override void Update()
    {
        base.Update();
        damageTimer += Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (damageTimer < damageTick) return;

        if (collision.collider.CompareTag("Player"))
        {
            var player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
                Debug.Log("EnemyFastMelee dealt " + contactDamage + " damage to Player.");
                damageTimer = 0f;
            }
        }
    }
}
