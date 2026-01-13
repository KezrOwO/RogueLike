using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private float damagePlayer = 2f;
    [SerializeField] private float damageEnemy = 1f;

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Récupère le tag du projectile
        // Permet de différencier les projectiles ennemis et joueurs

        // Si CE projectile est du joueur et touche un ennemi
        if (gameObject.CompareTag("Player") && collision.CompareTag("Enemy")) 
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePlayer);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Enemy component not found on " + collision.gameObject.name);
            }
        }

        // Si CE projectile est ennemi et touche le joueur
        if (gameObject.CompareTag("Enemy") && collision.CompareTag("Player"))
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
