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
            if (distance < closestDistance && !enemy.GetComponent<BaseBuilding>().isDead)
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

            if (building != null && !building.isDead)
            {
                Debug.Log("Attack: Bat attacks the building: " + building.buildingName);
                
                // Trigger the attack animation
                /*if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }*/

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

        while (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            if (building.isDead)
            {
                Debug.Log("Attack: Target is dead. Stopping attack.");
                break;
            }

            building.TakeDamage(damage);
            Debug.Log("Bat deals " + damage + " damage to " + building.buildingName);
            yield return new WaitForSeconds(attackCooldown);
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
