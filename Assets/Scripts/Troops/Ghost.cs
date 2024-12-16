using UnityEngine;
using System.Collections;

public class Ghost : BaseTroop
{
    public float lifetime = 10f; // Time before the ghost vanishes
    private bool isVanishing = false;

    protected override void Start()
    {
        // Set up the ghost-specific settings
        flyer = true; // Ghosts are flying troops
        troopType = "Ghost";
        health = Mathf.Infinity; // Ghosts cannot take damage
        damage = 0f; // Ghosts do not deal damage
        speed = 2f; // Movement speed for the ghost

        base.Start(); // Call the base class Start method

        // Find and set the target castle based on the team
        SetTargetCastle();

        // Start the vanishing process after a certain time
        StartCoroutine(VanishAfterTime());
    }

    protected override void Update()
    {
        // Move towards the target (enemy castle)
        if (currentTarget != null && !isVanishing)
        {
            MoveTowardsTarget();
        }
    }

    // Ghosts do not attack, override the Attack method to do nothing
    protected override void Attack()
    {
        // No attack behavior for ghosts
    }

    // Ghosts do not take damage
    public override void TakeDamage(float amount)
    {
        // Ignore any damage
    }

    // Ghosts have a predefined target, so FindTarget is not needed
    protected override void FindTarget()
    {
        // No need to find any target for the ghost
    }

    // Find the enemy castle based on the ghost's team and set it as the target
    private void SetTargetCastle()
    {
        // Determine the enemy team's tag based on the ghost's team
        string enemyTeamTag = (teamTag == "Team1") ? "Team2" : "Team1";

        // Find all objects tagged with the enemy team tag
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(enemyTeamTag);

        foreach (GameObject obj in potentialTargets)
        {
            // Check if the object has the MainCastle component
            MainCastle enemyCastle = obj.GetComponent<MainCastle>();
            if (enemyCastle != null)
            {
                // Set the enemy castle as the ghost's target
                currentTarget = enemyCastle.transform;
                Debug.Log($"{gameObject.name} is targeting {enemyCastle.name} at position {currentTarget.position}");
                return;
            }
        }

        // If no enemy castle is found, log a warning
        Debug.LogWarning("Enemy castle not found. Make sure the castle has the correct tag and component.");
    }

    // Coroutine to handle the vanishing after a set time
    private IEnumerator VanishAfterTime()
    {
        yield return new WaitForSeconds(lifetime - 1f); // Wait for most of the lifetime before playing animation

        isVanishing = true;

        // Trigger the scared animation before vanishing
        if (animator != null)
        {
            animator.SetTrigger("Scared");
        }

        yield return new WaitForSeconds(1f); // Wait for the scared animation to finish

        // Start the vanishing process
        if (animator != null)
        {
            animator.SetTrigger("Vanish");
        }

        yield return new WaitForSeconds(2f); // Wait for the vanish animation

        Die(); // Destroy the ghost after vanishing
    }

    // Override the Die method to properly clean up
    protected override void Die()
    {
        Debug.Log(gameObject.name + " vanished.");
        Destroy(gameObject);
    }
}
