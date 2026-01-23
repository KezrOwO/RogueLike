using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public static float damagePlayer = 2f;
    [SerializeField] private float damageEnemy = 1f;

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Récupère le tag du projectile
        // Permet de différencier les projectiles ennemis et joueurs

        // Si CE projectile est du joueur et touche un ennemi
        if (gameObject.CompareTag("PlayerProjectile") && collision.CompareTag("Enemy")) 
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy == null)
            {
                enemy = collision.GetComponentInParent<EnemyBase>();
            }
            
            if (enemy != null)
            {
                enemy.TakeDamage(damagePlayer);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("EnemyBase component not found on " + collision.gameObject.name);
            }
        }

        // Si CE projectile est ennemi et touche le joueur
        if (gameObject.CompareTag("EnemyProjectile") && collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageEnemy);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerController component not found on " + collision.gameObject.name);
            }
        }
    }
}
