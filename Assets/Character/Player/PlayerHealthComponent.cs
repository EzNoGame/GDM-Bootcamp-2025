using UnityEngine;

public class PlayerHealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Clamp(currentHealth - Mathf.Max(0f, amount), 0f, maxHealth);
        Debug.Log($"Player took {amount} damage. Health: {currentHealth}/{maxHealth}");
        if (IsDead)
        {
            HandleDeath();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Clamp(currentHealth + Mathf.Max(0f, amount), 0f, maxHealth);
    }

    private void HandleDeath()
    {
        Debug.Log("Player died.");
        // TODO: Add death handling (respawn, UI, etc.)
        gameObject.SetActive(false);
    }

    public float GetHealth() { return currentHealth; }
    public float GetMaxHealth() { return maxHealth; }
}


