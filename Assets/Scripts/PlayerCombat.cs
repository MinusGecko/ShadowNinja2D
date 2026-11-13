using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackCooldown = 0.35f;
    private float nextAttackTime;

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || MobileControlButton.ConsumeAttack()) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
    {
        Vector2 center = attackPoint != null ? attackPoint.position :
            (Vector2)transform.position + Vector2.right * transform.localScale.x * 0.7f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, attackRange, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null) health.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = attackPoint != null ? attackPoint.position :
            transform.position + Vector3.right * 0.7f;
        Gizmos.DrawWireSphere(center, attackRange);
    }
}
