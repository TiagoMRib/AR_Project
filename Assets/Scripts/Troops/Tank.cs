using UnityEngine;

public class TankTroop : BaseTroop
{

    protected override void Start()
    {
        base.Start();
        // Additional setup specific to TankTroop
        Debug.Log("Tank Troop Created");
    }

    // Tank targets only enemy buildings
    protected override void FindTarget()
    {
        GameObject[] enemyBuildings = GameObject.FindGameObjectsWithTag(enemyTeamTag + "_Building");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemyBuildings)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy.transform;
            }
        }
    }

    // Tanks might have special movement logic, like slower movement over rough terrain
    protected override void MoveTowardsTarget()
    {
    
        Debug.Log("Tank is moving towards the target.");

        base.MoveTowardsTarget();  // Call base movement
    }

    // Overriding the Attack method for the Tank troop to
    protected override void Attack()
    {
        //Tank attack 
    }
}
