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
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && !enemy.GetComponent<BaseBuilding>().isDead)
            {
                closestDistance = distance;
                currentTarget = enemy.transform;
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
                Attack(); // Attack if within range
            }
            else
            {
                if (currentTarget != null)
        {
            // Move towards the target
            MoveTowardsTarget();

            // Trigger walking animation if moving
            if (!isAttacking)
            {
                animator.SetBool("isWalking", true); // Set walking animation
            }
        }
        else
        {
            // If no target, stop the walking animation
            animator.SetBool("isWalking", false);
        }
            }
        }
    }


    // Overriding the Attack method for the Mushroom troop to attack with a projectile
    protected override void Attack()
    {
        if (currentTarget != null && !isAttacking)
        {
            Debug.Log("Attack: Mushroom attacks the building: " + currentTarget.name);
            StartCoroutine(FireProjectile(currentTarget)); // Start the projectile firing coroutine
        }
    }

    private IEnumerator FireProjectile(Transform target)
    {
        isAttacking = true;

        // Wait for the cooldown before firing the next projectile
        yield return new WaitForSeconds(attackCooldown);

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>(); // Assuming the projectile has a script

        if (projectileScript != null)
        {
            projectileScript.SetTarget(target, projectileSpeed); // Set the target and speed for the projectile
            Debug.Log("Mushroom fires a projectile towards: " + target.name);
        }

        isAttacking = false; // Reset attacking state
        FindTarget(); // Find a new target if the current one is gone
    }
}
