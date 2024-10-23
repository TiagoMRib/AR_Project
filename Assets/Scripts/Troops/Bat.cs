using UnityEngine;
using System.Collections;

public class Bat : BaseTroop
{
    protected override void Start()
    {
        troopType = "Spooky";  
        flyer = true;  
        base.Start();
        Debug.Log("Bat Troop Created - Type: " + troopType);
    }

    // Bat finds the closest enemy (troops or buildings)
protected override void FindTarget()
{
    Debug.Log("Movement: Bat is finding the closest enemy.");
    GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTeamTag);
    if (enemyObjects.Length == 0)
    {
        Debug.Log("No enemies found.");
        return;
    }

    float closestDistance = Mathf.Infinity;
    Transform closestTarget = null;

    foreach (GameObject enemy in enemyObjects)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);

        // Check if the enemy is a building and is not dead
        BaseBuilding building = enemy.GetComponent<BaseBuilding>();
        if (building != null && !building.isDead)
        {
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = enemy.transform;
            }
        }
        else
        {
            // Check if the enemy is a troop and is not dead
            BaseTroop troop = enemy.GetComponent<BaseTroop>();
            if (troop != null && troop.health > 0)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = enemy.transform;
                }
            }
        }
    }

    if (closestTarget != null)
    {
        currentTarget = closestTarget;
        Debug.Log("Bat found the closest enemy: " + currentTarget);
    }
    else
    {
        Debug.Log("No valid targets found.");
    }
}


    // Overriding the Attack method for the Bat troop to attack buildings
protected override void Attack()
{
    if (currentTarget != null && !isAttacking)
    {
        // Determine if the target is a building or a troop
        BaseBuilding building = currentTarget.GetComponent<BaseBuilding>();
        BaseTroop troop = currentTarget.GetComponent<BaseTroop>();

        if (building != null && !building.isDead)
        {
            Debug.Log("Attack: Bat attacks the building: " + building.buildingName);
            StartCoroutine(AttackCoroutine(building, null)); // Pass building to the coroutine
        }
        else if (troop != null && troop.health > 0)
        {
            Debug.Log("Attack: Bat attacks the troop: " + troop.troopType);
            StartCoroutine(AttackCoroutine(null, troop)); // Pass troop to the coroutine
        }
        else
        {
            Debug.Log("Attack: Target is already dead or invalid, searching for a new target.");
            FindTarget();
        }
    }
}

// Generalized coroutine for attacking either a building or a troop
private IEnumerator AttackCoroutine(BaseBuilding building, BaseTroop troop)
{
    isAttacking = true;

    while (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
    {
        // If attacking a building, ensure it's not dead
        if (building != null)
        {
            if (building.isDead)
            {
                Debug.Log("Attack: Target building is dead. Stopping attack.");
                break;
            }

            building.TakeDamage(damage);
            Debug.Log("Bat deals " + damage + " damage to " + building.buildingName);
        }
        // If attacking a troop, ensure it has health remaining
        else if (troop != null)
        {
            if (troop.health <= 0)
            {
                Debug.Log("Attack: Target troop is dead. Stopping attack.");
                break;
            }

            troop.TakeDamage(damage);
            Debug.Log("Bat deals " + damage + " damage to " + troop.troopType);
        }

        // Wait for the cooldown before the next attack
        yield return new WaitForSeconds(attackCooldown);

        // Trigger the attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    // Stop the attack animation when the attack loop ends
    if (animator != null)
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Idle"); // Optionally, set the animation state back to "Idle" or a default state
    }

    isAttacking = false;
    FindTarget(); // Search for a new target if the current one is gone
}



}
