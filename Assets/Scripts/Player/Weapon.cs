using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public static float damagePlayer = 2f;
    [SerializeField] private float damageEnemy = 1f;
    private bool hasHit = false;  // Empêche les dégâts multiples du même projectile

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;  // Évite les dégâts multiples

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
                // Vérifier si c'est un EnemyDodgeDash
                EnemyDodgeDash dodger = enemy.GetComponent<EnemyDodgeDash>();
                if (dodger != null)
                {
                    // ✅ Destruction rapide si l'ennemi est invincible (en train de dasher)
                    if (enemy.IsInvincible)
                    {
                        hasHit = true;
                        Destroy(gameObject);  // Destruction immédiate si dash en cours
                        return;
                    }
                    else
                    {
                        // Attendre un court instant pour voir si le dash se déclenche
                        StartCoroutine(WaitForDodgeResult(dodger, enemy));
                    }
                }
                else
                {
                    // Pour les autres ennemis : dégâts + destruction immédiate
                    hasHit = true;
                    enemy.TakeDamage(damagePlayer);
                    Destroy(gameObject);
                }
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
                hasHit = true;
                player.TakeDamage(damageEnemy);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerController component not found on " + collision.gameObject.name);
            }
        }
    }

    private IEnumerator WaitForDodgeResult(EnemyDodgeDash dodger, EnemyBase enemy)
    {
        hasHit = true;

        // ⏱️ Attendre un court instant pour voir si le dash se déclenche
        yield return new WaitForSeconds(0.1f);

        // ✅ Si l'ennemi est devenu invincible (dash en cours), détruire immédiatement
        if (enemy != null && enemy.IsInvincible)
        {
            Destroy(gameObject);
            yield break;
        }

        // Sinon, infliger les dégâts (le dash a échoué ou n'a pas eu lieu)
        if (enemy != null && this != null && gameObject != null)
        {
            enemy.TakeDamage(damagePlayer);
            Destroy(gameObject);
        }
    }
}

