using UnityEngine;
using System.Collections;

public class Golem : BaseTroop
{
    protected override void Start()
    {
        troopType = "Golem";  
        flyer = false;  // Golem is not a flyer
        base.Start();
        Debug.Log("Golem Troop Created - Type: " + troopType);
    }

    // Golem finds the closest enemy (troops or buildings)
    protected override void FindTarget()
    {
        Debug.Log("Movement: Golem is finding the closest enemy.");
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTeamTag);
        if (enemyObjects.Length == 0)
        {
            Debug.Log("No enemies found.");
            return;
        }

        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemyObjects)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && !enemy.GetComponent<BaseBuilding>().isDead)
            {
                closestDistance = distance;
                currentTarget = enemy.transform;
            }
        }

        Debug.Log("Golem found the closest enemy: " + currentTarget);
    }

    protected override void Update()
    {
        
        // Check if Golem is moving towards the target
        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            // Move towards the target
            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget();
                animator.SetBool("isWalking", true);
            }
            else if (!isAttacking)
            {
                Attack();
                animator.SetBool("isWalking", false);
            }
            
        }
        else
        {
            // If no target, stop the walking animation
            animator.SetBool("isWalking", false);
        }
    }

    // Golem moves towards the current target
    protected override void MoveTowardsTarget()
    {
        if (currentTarget != null)
        {
            base.MoveTowardsTarget();
            // When Golem reaches the target, stop walking
            animator.SetBool("isWalking", false);
        }
            
        
    }

    // Overriding the Attack method for the Golem troop to attack buildings
    protected override void Attack()
    {
        if (currentTarget != null && !isAttacking)
        {
            BaseBuilding building = currentTarget.GetComponent<BaseBuilding>();

            if (building != null && !building.isDead)
            {
                Debug.Log("Attack: Golem attacks the building: " + building.buildingName);
                StartCoroutine(AttackBuilding(building)); // Start attacking the building
            }
            else
            {
                Debug.Log("Attack: Target is already dead, searching for a new target.");
                FindTarget();
            }
        }
    }

    // Coroutine for continuous attack with cooldown
    private IEnumerator AttackBuilding(BaseBuilding building)
    {
        isAttacking = true;
        int hits = 0; // Counter for hits

        while (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            if (building.isDead)
            {
                Debug.Log("Attack: Target is dead. Stopping attack.");
                break;
            }

            // Alternate between Hit and Hammer Attack animations
            if (hits < 2)
            {
                // Trigger Hit animation
                if (animator != null)
                {
                    animator.SetTrigger("Hit");
                }

                // Deal damage
                building.TakeDamage(damage);
                Debug.Log("Golem deals " + damage + " damage to " + building.buildingName);
                hits++;
            }
            else
            {
                // Trigger Hammer Attack animation
                if (animator != null)
                {
                    animator.SetTrigger("Hammer");
                }

                // Deal damage
                building.TakeDamage(damage * 2); // Assuming Hammer Attack does more damage
                Debug.Log("Golem performs a hammer attack, dealing " + (damage * 2) + " damage to " + building.buildingName);
                hits = 0; // Reset hits after hammer attack
            }

            yield return new WaitForSeconds(attackCooldown); // Wait before the next attack
        }

        // Stop the attack animation when the attack loop ends
        if (animator != null)
        {
            animator.SetTrigger("Idle"); // Set back to Idle state after attack ends
        }

        isAttacking = false;
        FindTarget(); // Search for a new target if the current one is gone
    }
}
