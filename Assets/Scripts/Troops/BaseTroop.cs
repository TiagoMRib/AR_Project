using UnityEngine;

public abstract class BaseTroop : MonoBehaviour
{
    public float health = 100f;       // Health of the troop
    public float damage = 10f;        // Damage this troop deals
    public float speed = 5f;          // Movement speed
    public string teamTag = "Team1";            // Team identifier (e.g., "Team1" or "Team2")

    public string enemyTeamTag;       // Enemy team identifier

    protected Transform currentTarget;   // The current target this troop is attacking

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Initial setup, team assignment, etc.
        // Can be overridden in specific troops if needed

        enemyTeamTag = (teamTag == "Team1") ? "Team2" : "Team1";  // Set enemy team tag
        FindTarget();
    }

    protected virtual void Update()
    {
        if (currentTarget != null)
        {
            MoveTowardsTarget();
        }
    }

    // Base method for moving towards a target
    protected virtual void MoveTowardsTarget()
    {
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

    // Handle death (can be overridden for different death behavior)
    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);  // Destroy the troop on death
    }

    // Find the closest target (can be customized in derived classes)
    protected abstract void FindTarget();
}
