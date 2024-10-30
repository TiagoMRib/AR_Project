using UnityEngine;
using System.Collections;

public class Mushroom : BaseTroop
{
    public GameObject projectilePrefab; // Prefab for the projectile to be instantiated
    public float projectileSpeed = 10f; // Speed of the projectile

    protected override void Start()
    {
        troopType = "Mushroom";  
        flyer = false; // Set to true if the mushroom can fly
        base.Start();
        Debug.Log("Mushroom Troop Created - Type: " + troopType);
    }

    // Find the closest enemy to attack
    protected override void FindTarget()
    {
        Debug.Log("Movement: Mushroom is finding the closest enemy.");
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTeamTag);
        if (enemyObjects.Length == 0)
        {
            Debug.Log("No enemies found.");
            return;
        }

        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemyObjects)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance && !enemy.GetComponent<BaseBuilding>().isDead)
                {
                    closestDistance = distance;
                    currentTarget = enemy.transform;
                }
            }
            
        }

        Debug.Log("Mushroom found the closest enemy: " + currentTarget);
    }

    protected override void Update()
    {
        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                animator.SetBool("isWalking",false);
                Attack(); // Attack if within range
            }
            else
            {
               
                // Move towards the target
                MoveTowardsTarget();

                // Trigger walking animation if moving
                if (!isAttacking)
                {
                    animator.SetBool("isWalking", true); // Set walking animation
                }
            
            }
        }
        else
        {
            // If no target, stop the walking animation
            animator.SetBool("isWalking", false);
            FindTarget();
        }
            
        
    }

    protected override void Attack()
{
    if (currentTarget != null && !isAttacking)
    {
        // Determine if the target is a building or a troop
        BaseBuilding building = currentTarget.GetComponent<BaseBuilding>();
        BaseTroop troop = currentTarget.GetComponent<BaseTroop>();

        if (building != null && !building.isDead)
        {
            Debug.Log("Attack: Shroom attacks the building: " + building.buildingName);
            StartCoroutine(FireProjectile(building, null)); // Pass building to the coroutine
        }
        else if (troop != null && troop.health > 0)
        {
            Debug.Log("Attack: Shroom attacks the troop: " + troop.troopType);
            StartCoroutine(FireProjectile(null, troop)); // Pass troop to the coroutine
        }
        else
        {
            Debug.Log("Attack: Target is already dead or invalid, searching for a new target.");
            FindTarget();
        }
    }
}

// Generalized coroutine for attacking either a building or a troop
private IEnumerator FireProjectile(BaseBuilding building, BaseTroop troop)
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

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            yield return new WaitForSeconds(1f);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>(); 

            if (projectileScript != null)
            {
                projectileScript.SetTarget(building.transform, projectileSpeed, damage); // Set the target and speed for the projectile
            }
        }
        // If attacking a troop, ensure it has health remaining
        else if (troop != null)
        {
            if (troop.health <= 0)
            {
                Debug.Log("Attack: Target troop is dead. Stopping attack.");
                break;
            }

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            yield return new WaitForSeconds(1f);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>(); 

            if (projectileScript != null)
            {
                projectileScript.SetTarget(troop.transform, projectileSpeed, damage); // Set the target and speed for the projectile
            }
        }

        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Idle"); 
        }

        // Wait for the cooldown before the next attack
        yield return new WaitForSeconds(attackCooldown);
        
    }

    // Stop the attack animation when the attack loop ends
    

    isAttacking = false;
    FindTarget(); // Search for a new target if the current one is gone
}


}
