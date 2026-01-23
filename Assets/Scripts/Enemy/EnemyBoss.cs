using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    [SerializeField] private GameObject itemPickUpPrefab;
    protected override void HandleAim()
    {
        base.HandleAim();
    }

    protected override void HandleMovement(Transform target)
    {
        base.HandleMovement(target);
    }

    protected override void OnDeath()
    {
                
            if (itemPickUpPrefab != null)
            {
                Instantiate(itemPickUpPrefab, transform.position, Quaternion.identity);
            }
            
            base.OnDeath(); 
    }

    protected override void OnShoot()
    {
        if(attackTimer >= attackCooldown)
        {
            // Shoot three projectiles in a spread pattern
            for (int i = -1; i <= 1; i++)
            {
                GameObject projectile = Instantiate(ProjectilEnemy, Aim.position, Aim.rotation);
                projectile.GetComponent<Renderer>().material.color = Color.red;
                projectile.tag = "EnemyProjectile";
                
                Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
                float angle = 15f * i; // Spread angle (-15°, 0°, +15°)
                Vector2 shootDirection = Quaternion.Euler(0, 0, angle) * (-Aim.up);
                rbProjectile.AddForce(shootDirection * fireForce, ForceMode2D.Impulse);
                
                Destroy(projectile, 2f);
            }
            attackTimer = 0f;
        }


    }
}
