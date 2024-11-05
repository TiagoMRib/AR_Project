using System.Collections;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    [Header("Building Attributes")]
    public string buildingName;
    public float maxHealth = 100f;
    protected float currentHealth;
    public bool isDefensiveBuilding;  // Determines if this building attacks enemies
    public float attackRange = 10f;
    public float attackCooldown = 1f;

    public string teamTag;  
    public string enemyTeamTag;
    public bool isDead = false;

    [Header("Mana Reward Settings")]
    public float manaReward = 50f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemyTeamTag = (teamTag == "Team1") ? "Team2" : "Team1";

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // You can add common update logic for all buildings here if needed
    }

    // Method to take damage
    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Attack: Building " + buildingName + " took " + amount + " damage. Current health: " + currentHealth);
        
        if (currentHealth <= 0f)
        {
            
            Die();
        }
    }

    // Abstract method for attacking (must be implemented by derived classes)
    public virtual void Attack(){
        Debug.Log("Attack: Building " + buildingName + " is attacking.");
    }

    // Method to check for and attack enemies, implemented by child classes
    protected IEnumerator AttackRoutine()
    {
        while (!isDead && isDefensiveBuilding)
        {
            yield return new WaitForSeconds(attackCooldown);
            Attack();  // Calls the specific attack logic from the derived class
        }
    }

    // Method when the building dies
    protected virtual void Die()
    {
        isDead = true;

        if (buildingName != null && buildingName.ToLower().Contains("castle"))
        {
            //gameInitialization.StopGame();
            Debug.Log("Castle died");
        }
        // Check if this building belongs to the enemy team
        if (teamTag == "Team2")
        {
            // Award mana to the player
            GameManager.Instance?.ManaSystem.GainMana(manaReward);
        }

        Destroy(gameObject);  // Remove the building from the scene
    }
}

