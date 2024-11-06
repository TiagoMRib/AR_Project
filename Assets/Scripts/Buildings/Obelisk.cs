using System.Collections;
using UnityEngine;

public class Obelisk : BaseBuilding
{
    [Header("Obelisk Attributes")]
    public LineRenderer laserBeam; // Reference to the LineRenderer for the laser beam
    public float laserDamage = 20f; // Damage dealt by the laser per second

    private Transform currentTarget; // The current target being attacked by the laser

   
    
    /*
    [Header("Attack Range Visualization")]
    public GameObject attackRangePrefab; // Reference to the prefab for the attack range visualization
    private GameObject attackRangeIndicator;
    */

    protected override void Start()
    {
        base.Start();

        if (laserBeam != null)
        {
            laserBeam.enabled = false; // Initially hide the laser beam
        }
        

        FindTarget();
    }

    protected override void Update()
    {
        base.Update();

        if (!isDead)
        {
          
            SendMessageUpwards("ActivateRangeIndicator", SendMessageOptions.DontRequireReceiver);
            
            if (currentTarget == null) FindTarget();
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
        Debug.Log("Obelisk found " + enemyTroops.Length + " enemies.");
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

        Debug.Log("Obelisk found the closest enemy: " + currentTarget);
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
        SendMessageUpwards("DeactivateRangeIndicator", SendMessageOptions.DontRequireReceiver);
        /*
        if (attackRangeIndicator != null)
        {
            Destroy(attackRangeIndicator);
        }*/
        Debug.Log("Obelisk has been destroyed.");
    }
    
    
}

