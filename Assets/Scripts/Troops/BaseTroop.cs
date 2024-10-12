using UnityEngine;
using System.Collections;

public abstract class BaseTroop : MonoBehaviour
{
    public bool flyer = false;

    public string troopType = "Normal";
    public float health = 100f;       // Health of the troop
    public float damage = 10f;        // Damage this troop 
    
    public float attackRange = 1f;   // Attack range

    public float attackCooldown = 1f;
    public float speed = 5f;          // Movement speed
    public string teamTag = "Team1";  // Team identifier (e.g., "Team1" or "Team2")

    public string enemyTeamTag;       // Enemy team identifier

    protected Transform currentTarget;   // The current target this troop is attacking
    protected bool isAttacking = false;    // To check if the troop is already attacking

    protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Initial setup, team assignment
        enemyTeamTag = (teamTag == "Team1") ? "Team2" : "Team1";  // Set enemy team tag
        animator = GetComponentInChildren<Animator>();
        FindTarget();
    }

    protected virtual void Update()
    {
        if (currentTarget != null)
        {
            // Check distance to current target
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            Debug.Log("Attack:" + distanceToTarget + " units away from the target.");
            
            if (distanceToTarget > attackRange) 
            {

                Debug.Log("Attack: Out of range. Moving towards the target.");
                // Move towards the target if out of range
                MoveTowardsTarget();
            }
            else if (!isAttacking)
            {
                // Start attacking if in range and not already attacking
                Attack();
            }
        }
    }

    // Base method for moving towards a target
    protected virtual void MoveTowardsTarget()
    {
        Debug.Log("Movement: Moving towards the target.");
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, step);
    }

    // Method to be called when the troop takes damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle death
    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);  // Destroy the troop on death
    }

    // Find the closest target 
    protected abstract void FindTarget();

    protected abstract void Attack();
}
