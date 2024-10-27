using System.Collections;
using UnityEngine;

public class Obelisk : BaseBuilding
{
    [Header("Obelisk Attributes")]
    public LineRenderer laserBeam; // Reference to the LineRenderer for the laser beam
    public float laserDamage = 20f; // Damage dealt by the laser per second

    private Transform currentTarget; // The current target being attacked by the laser

    protected override void Start()
    {
        base.Start();

        if (laserBeam != null)
        {
            laserBeam.enabled = false; // Initially hide the laser beam
        }
    }

    protected override void Update()
    {
        base.Update();

        if (isDefensiveBuilding && !isDead)
        {
            FindTarget();
            if (currentTarget != null)
            {
                Debug.Log("Obelisk is attacking " + currentTarget.name);
                ShootLaser();
            }
            else
            {
                DisableLaser();
            }
        }
    }

    // Finds the closest enemy within range
    private void FindTarget()
    {
        GameObject[] enemyTroops = GameObject.FindGameObjectsWithTag(enemyTeamTag);
        float closestDistance = attackRange;
        currentTarget = null;

        foreach (GameObject enemy in enemyTroops)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy.transform;
            }
        }
    }
    

    // Shoots a laser beam at the current target
    private void ShootLaser()
    {
        if (currentTarget == null) return;

        // Enable the laser beam and set its positions
        if (laserBeam != null)
        {
            laserBeam.enabled = true;
            laserBeam.SetPosition(0, transform.position + new Vector3(0, 0.85f, 0));
            laserBeam.SetPosition(1, currentTarget.position + new Vector3(0, 0.3f, 0));
        }

        // Deal damage to the target
        BaseTroop targetTroop = currentTarget.GetComponent<BaseTroop>();
        if (targetTroop != null)
        {
            targetTroop.TakeDamage(laserDamage * Time.deltaTime);
        }
    }

    // Disables the laser beam when not shooting
    private void DisableLaser()
    {
        if (laserBeam != null)
        {
            laserBeam.enabled = false;
        }
    }

    protected override void Die()
    {
        base.Die();
        DisableLaser(); // Disable the laser beam when the obelisk is destroyed
        Debug.Log("Obelisk has been destroyed.");
    }
}

