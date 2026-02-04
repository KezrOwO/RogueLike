using System.Collections;
using UnityEngine;

public class EnemyDodgeDash : EnemyBase
{
    [Header("Dodge Dash")]
    [SerializeField] private float dashSpeed = 12f;          // ⬇️ Réduit pour moins de distance
    [SerializeField] private float dashDuration = 0.2f;      // ⬇️ Réduit pour dash plus court
    [SerializeField] private float dashCooldown = 1.5f;      // ✅ OK
    [SerializeField] private float windUp = 0.05f;           // ✅ OK (réaction rapide)
    [SerializeField] private float dodgeChance = 0.5f;

    private float dashTimer;
    private bool isDashing;

    protected override void Start()
    {
        base.Start();

        if (rb != null)
            Debug.Log($"RB Config: BodyType={rb.bodyType}, Simulated={rb.simulated}, GravityScale={rb.gravityScale}, Drag={rb.drag}");
    }


 


    protected override void HandleMovement(Transform target)
    {
        // ✅ Pendant le dash : on NE TOUCHE PAS rb.velocity ici
        if (isDashing) return;

        base.HandleMovement(target);
    }

    protected override void Update()
    {
        if (!isDashing)
        {
            base.Update();
        }

        dashTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDashing) return;
        if (dashTimer < dashCooldown) return;

        if (other.CompareTag("PlayerProjectile"))
        {
            if (Random.value > dodgeChance)
            {

                return;
            }

            Vector2 dir = ((Vector2)transform.position - (Vector2)other.transform.position).normalized;
            Vector2 perp = Random.value > 0.5f
                ? new Vector2(-dir.y, dir.x)
                : new Vector2(dir.y, -dir.x);


            
            StartCoroutine(Dash(perp));
        }
    }

    private IEnumerator Dash(Vector2 dir)
    {
        isDashing = true;
        blockMovement = true;  // ⭐ BLOQUER le mouvement de EnemyBase
        dashTimer = 0f;
        SetInvincible(true);



        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody2D is null in Dash()!");
            isDashing = false;
            blockMovement = false;
            yield break;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.yellow;

        Vector2 startPos = transform.position;

        yield return new WaitForSeconds(windUp);

        Vector2 dashVelocity = dir.normalized * dashSpeed;

        float elapsed = 0f;

        Vector2 lastPos = transform.position;
        
        while (elapsed < dashDuration)
        {
            rb.velocity = dashVelocity;

            elapsed += Time.fixedDeltaTime;
            
            Vector2 currentPos = transform.position;
            float distanceMoved = Vector2.Distance(lastPos, currentPos);

            
           lastPos = currentPos;

            yield return new WaitForFixedUpdate();
           
        }

        Vector2 endPos = transform.position;
        float totalDistance = Vector2.Distance(startPos, endPos);


        if (totalDistance < 0.1f)
        {
            Debug.LogError($"<color=red>⚠️ DASH FAILED! Distance: {totalDistance:F2}</color>");
        }

        rb.velocity = Vector2.zero;
        sr.color = originalColor;
        blockMovement = false;  // ⭐ DÉBLOQUER le mouvement
        isDashing = false;
        SetInvincible(false);
    }
}
