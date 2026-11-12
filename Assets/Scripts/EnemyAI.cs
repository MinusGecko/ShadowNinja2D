using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1.2f;
    private Rigidbody2D rb;
    private Transform player;
    private float nextAttack;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange) { rb.velocity = new Vector2(0, rb.velocity.y); return; }
        if (distance > attackRange)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (Time.time >= nextAttack)
            {
                player.GetComponent<Health>()?.TakeDamage(1);
                nextAttack = Time.time + attackCooldown;
            }
        }
    }
}
