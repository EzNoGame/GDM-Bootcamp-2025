using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Player Components")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttackComponent playerAttack;
    [SerializeField] private PlayerHealthComponent playerHealth;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackSpeedReduction = 0.2f;
    
    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerAttackComponent PlayerAttack => playerAttack;
    
    private float originalMoveSpeed;
    private float originalRotationSpeed;
    private bool isAttacking = false;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Get PlayerMovement component if not assigned
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
        
        // Get PlayerAttackComponent if not assigned
        if (playerAttack == null)
        {
            playerAttack = GetComponent<PlayerAttackComponent>();
        }
        
        // Store original move speed
        if (playerMovement != null)
        {
            originalMoveSpeed = playerMovement.GetMoveSpeed();
        }

        // Store original rotation speed
        if (playerMovement != null)
        {
            originalRotationSpeed = playerMovement.GetRotationSpeed();
        }
    }
    
    private void OnEnable()
    {
        // Subscribe to attack events
        PlayerAttackComponent.OnPlayerAttack += OnPlayerAttack;
        PlayerAttackComponent.OnAttackEnd += OnAttackEnd;
    }
    
    private void OnDisable()
    {
        // Unsubscribe from attack events
        PlayerAttackComponent.OnPlayerAttack -= OnPlayerAttack;
        PlayerAttackComponent.OnAttackEnd -= OnAttackEnd;
    }
    
    private void OnPlayerAttack()
    {
        if (!isAttacking)
        {
            SlowDownPlayer();
            isAttacking = true;
        }
    }
    
    private void OnAttackEnd()
    {
        if (isAttacking)
        {
            RestorePlayerSpeed();
            isAttacking = false;
        }
    }
    
    private void SlowDownPlayer()
    {
        if (playerMovement != null)
        {
            float slowedSpeed = originalMoveSpeed * attackSpeedReduction;
            float slowedRotationSpeed = originalRotationSpeed * attackSpeedReduction;
            playerMovement.SetMoveSpeed(slowedSpeed);
            playerMovement.SetRotationSpeed(slowedRotationSpeed);
        }
    }
    
    private void RestorePlayerSpeed()
    {
        if (playerMovement != null)
        {
            playerMovement.SetMoveSpeed(originalMoveSpeed);
            playerMovement.SetRotationSpeed(originalRotationSpeed);
        }
    }
    
    public void SetAttackSpeedReduction(float newReduction)
    {
        attackSpeedReduction = newReduction;
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
    }
}
