using UnityEngine;

public class EnemyAttackComponent : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float attackDamage = 10.0f;
    [SerializeField] private string attackAnimationTrigger = "Attack";

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovementComponent movementComponent;

    private Transform playerTransform;
    private float lastAttackTime;
    private bool isAttacking;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (movementComponent == null)
            movementComponent = GetComponent<EnemyMovementComponent>();
    }

    private void Start()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerMovement != null)
        {
            playerTransform = PlayerManager.Instance.PlayerMovement.transform;
        }
        lastAttackTime = -attackCooldown;
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerMovement != null)
            {
                playerTransform = PlayerManager.Instance.PlayerMovement.transform;
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown && distanceToPlayer <= attackRange)
        {
            if(PlayerManager.Instance.gameObject.activeSelf)
            {
                BeginAttack();
            }
        }
    }

    private void BeginAttack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;

        if (movementComponent != null)
            movementComponent.enabled = false; // pause movement during attack

        if (animator != null && !string.IsNullOrEmpty(attackAnimationTrigger))
            animator.SetTrigger(attackAnimationTrigger);
    }

    // Animation Events - hook these up on the attack animation
    public void OnAttackAnimationStart()
    {
        isAttacking = true;
    }

    // Call at the hit frame from the animation event
    public void OnAttackAnimationHit()
    {
        TryDealDamageToPlayer();
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        if (movementComponent != null)
            movementComponent.enabled = true;
    }

    private void TryDealDamageToPlayer()
    {
        if (playerTransform == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > attackRange)
            return; // out of range at hit frame

        // Attempt to find a health/damage receiver on the player
        PlayerHealthComponent playerHealth = null;
        if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerMovement != null)
        {
            playerHealth = PlayerManager.Instance.gameObject.GetComponent<PlayerHealthComponent>();
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogWarning("EnemyAttackComponent: No PlayerHealthComponent found on player to receive damage.");
        }
    }

    // Public API
    public void SetAttackRange(float newRange)
    {
        attackRange = Mathf.Max(0f, newRange);
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public void SetAttackDamage(float newDamage)
    {
        attackDamage = Mathf.Max(0f, newDamage);
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public void SetAttackCooldown(float newCooldown)
    {
        attackCooldown = Mathf.Max(0f, newCooldown);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


