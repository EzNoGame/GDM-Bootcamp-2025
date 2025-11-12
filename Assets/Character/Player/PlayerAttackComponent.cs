using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerAttackComponent : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private string attackAnimationTrigger = "Attack";
    // [SerializeField] private float hitboxActiveDuration = 0.2f;
    
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject hitbox;
    
    private InputAction attackAction;
    private float lastAttackTime;
    private bool canAttack = true;
    private bool isHitboxActive = false;
    private float hitboxActiveTime;
    
    // Events
    public static event Action OnPlayerAttack;
    public static event Action OnAttackStart;
    public static event Action OnAttackEnd;
    public static event Action OnHitboxActivated;
    public static event Action OnHitboxDeactivated;
    
    private void Awake()
    {
        // Get components
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        
        // Get attack action from input system
        if (playerInput != null)
        {
            attackAction = playerInput.actions["Attack"];
        }
        
        // Ensure hitbox starts disabled
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        if (attackAction != null)
        {
            attackAction.performed += OnAttackInput;
        }
    }
    
    private void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.performed -= OnAttackInput;
        }
    }
    
    private void Update()
    {
        // Check cooldown
        if (!canAttack && Time.time - lastAttackTime >= attackCooldown)
        {
            canAttack = true;
        }
    }
    
    private void OnAttackInput(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            PerformAttack();
        }
    }
    
    private void PerformAttack()
    {
        // Update timing
        lastAttackTime = Time.time;
        canAttack = false;
        
        // Trigger events
        OnPlayerAttack?.Invoke();
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger(attackAnimationTrigger);
        }
        
        Debug.Log("Player is attacking!");

        AudioManager.Instance.PlayAttackSFX();
    }
    
    // Animation event methods (call these from animation events)
    public void OnAttackAnimationStart()
    {
        OnAttackStart?.Invoke();
    }
    
    public void OnAttackAnimationEnd()
    {
        OnAttackEnd?.Invoke();
        DeactivateHitbox(); // Ensure hitbox is deactivated when attack ends
    }
    
    // Call this method from animation event at the point you want hitbox to activate
    public void ActivateHitbox()
    {
        if (hitbox != null && !isHitboxActive)
        {
            hitbox.SetActive(true);
            isHitboxActive = true;
            hitboxActiveTime = Time.time;
            OnHitboxActivated?.Invoke();
            hitbox.gameObject.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("Hitbox activated!");
        }
    }
    
    public void DeactivateHitbox()
    {
        if (hitbox != null && isHitboxActive)
        {
            hitbox.SetActive(false);
            isHitboxActive = false;
            OnHitboxDeactivated?.Invoke();
            hitbox.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log("Hitbox deactivated!");
        }
    }
    
    // Method to check if hitbox is active (for external components)
    public bool IsHitboxActive()
    {
        return isHitboxActive;
    }
    
    // Public methods
    public void SetAttackCooldown(float newCooldown)
    {
        attackCooldown = newCooldown;
    }
    
    public void SetAttackAnimationTrigger(string newTrigger)
    {
        attackAnimationTrigger = newTrigger;
    }

    
    public void SetHitbox(GameObject newHitbox)
    {
        hitbox = newHitbox;
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }
    
    public bool IsAttacking()
    {
        return !canAttack;
    }
    
    public float GetAttackCooldownRemaining()
    {
        if (canAttack) return 0f;
        return attackCooldown - (Time.time - lastAttackTime);
    }
    
    public float GetAttackCooldownProgress()
    {
        if (canAttack) return 1f;
        return (Time.time - lastAttackTime) / attackCooldown;
    }
    
}