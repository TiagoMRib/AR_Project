using UnityEngine;

public class Bat : BaseTroop
{
    public string troopType = "Spooky";  // Troop type specific to Bat
    public bool canFly = true;           // Bats can fly

    protected override void Start()
    {
        base.Start();
        // Additional setup specific to BatTroop
        Debug.Log("Bat Troop Created - Type: " + troopType);
    }

    // Bat finds the closest enemy (troops or buildings)
    protected override void FindTarget()
    {
        Debug.Log("Movement: Bat is finding the closest enemy.");
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTeamTag);
        if (enemyObjects.Length == 0)
        {
            Debug.Log("Movement: No enemies found.");
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
    }

    // Bats can override MoveTowardsTarget if they have unique movement logic (e.g., flying)
    protected override void MoveTowardsTarget()
    {
        // If the bat can fly, it can pass the river
        if (canFly)
        {
            Debug.Log("Movement: Bat is flying towards the target.");
        }

        base.MoveTowardsTarget();  // Call base movement
    }
}
