using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth == 0)
        {
            if (CompareTag("Player")) GameManager.Instance?.PlayerDefeated();
            else Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        if (amount > 0) currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }
}
