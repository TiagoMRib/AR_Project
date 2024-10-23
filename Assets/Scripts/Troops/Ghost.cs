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

        // Set the target to the opposite side
        currentTarget = new GameObject("GhostTarget").transform;
        currentTarget.position = (teamTag == "Team1") ? new Vector3(-100, transform.position.y, transform.position.z)
                                                     : new Vector3(100, transform.position.y, transform.position.z);

        // Start the vanishing process after a certain time
        StartCoroutine(VanishAfterTime());
    }

    protected override void Update()
    {
        // Move towards the target (opposite side)
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

    // FindTarget is not needed, as ghosts have a predefined target
    protected override void FindTarget()
    {
        // No need to find any target for the ghost
    }

    // Coroutine to handle the vanishing after a set time
    private IEnumerator VanishAfterTime()
    {
        yield return new WaitForSeconds(lifetime - 2f); // Wait for most of the lifetime before playing animation

        // Trigger the scared animation before vanishing
        if (animator != null)
        {
            animator.SetTrigger("Scared");
        }

        yield return new WaitForSeconds(2f); // Wait for the scared animation to finish

        // Start the vanishing process
        isVanishing = true;
        if (animator != null)
        {
            animator.SetTrigger("Vanish");
        }

        yield return new WaitForSeconds(1f); // Wait for the vanish animation

        Die(); // Destroy the ghost after vanishing
    }

    // Override the Die method to properly clean up
    protected override void Die()
    {
        Debug.Log(gameObject.name + " vanished.");
        if (currentTarget != null)
        {
            Destroy(currentTarget.gameObject); // Clean up the temporary target
        }
        Destroy(gameObject);
    }
}
