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

        foreach (GameObject enemy in enemyObjects)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy.transform;
            }
        }

        Debug.Log("Bat found the closest enemy: " + currentTarget);
    }

    // Overriding the Attack method for the Bat troop to attack buildings
    protected override void Attack()
    {
        if (currentTarget != null && !isAttacking)
        {
            BaseBuilding building = currentTarget.GetComponent<BaseBuilding>();

            if (building != null)
            {
                Debug.Log("Attack: Bat attacks the building: " + building.buildingName);
                
                // Trigger the attack animation
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }

                StartCoroutine(AttackBuilding(building)); // Start attacking the building
            }
        }
    }

    // Coroutine for continuous attack with cooldown
        private IEnumerator AttackBuilding(BaseBuilding building)
    {
        isAttacking = true;

        while (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            if (building != null)
            {
                // Deal damage to the building
                building.TakeDamage(damage);
                Debug.Log("Bat deals " + damage + " damage to " + building.buildingName);

                // Wait for attack cooldown before the next attack
                yield return new WaitForSeconds(attackCooldown);

                // Trigger the attack animation again for each attack cycle
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                Debug.Log("Attack: Target building destroyed.");
                currentTarget = null;
                break;
            }
        }

        // Stop the attack animation when the attack loop ends
        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Idle"); // Optionally, set the animation state back to "Idle" or a default state
        }

        isAttacking = false; // Reset isAttacking when out of range or target destroyed
    }
}
