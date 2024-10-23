using System.Collections;
using UnityEngine;

public class Graveyard : BaseBuilding
{
    [Header("Graveyard Attributes")]
    public GameObject ghostPrefab; // Prefab for the ghost unit
    public float spawnInterval = 5f; // Time interval between spawning ghosts

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnGhostRoutine()); // Start the spawning routine
    }

    // Coroutine to spawn ghosts at regular intervals
    private IEnumerator SpawnGhostRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (isDead) yield break; // Stop spawning if the graveyard is destroyed

            SpawnGhost();
        }
    }

    // Method to spawn a ghost
    private void SpawnGhost()
    {
        if (ghostPrefab == null)
        {
            Debug.LogWarning("No ghost prefab assigned to Graveyard.");
            return;
        }

        // Instantiate the ghost near the Graveyard
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        GameObject ghost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);

        // Set the ghost's team tag
        BaseTroop ghostTroop = ghost.GetComponent<BaseTroop>();
        if (ghostTroop != null)
        {
            ghostTroop.teamTag = teamTag;
            ghostTroop.enemyTeamTag = enemyTeamTag;

            ghost.tag = teamTag;
        }

        Debug.Log("Graveyard spawned a ghost at " + spawnPosition);
    }

    // Override Die method to stop spawning ghosts
    protected override void Die()
    {
        base.Die();
        Debug.Log("Graveyard has been destroyed and will no longer spawn ghosts.");
    }
}

