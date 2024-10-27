using UnityEngine;
using Vuforia;

public class BuildingCard : MonoBehaviour
{
    public GameObject buildingPrefab; // The building prefab to spawn

    private ObserverBehaviour observerBehaviour;
    private bool cardDetected = false;
    private bool gameRunning = false;
    private GameObject spawnedBuilding = null; // To keep track of the spawned building

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            // Register for status change events
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Subscribe to game state changes
        GameManager.Instance.OnMatchStarted += OnMatchStarted;
        GameManager.Instance.OnMatchEnded += OnMatchEnded;
    }

    void OnDestroy()
    {
        if (observerBehaviour)
        {
            // Unregister from status change events
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }

        // Unsubscribe from game state changes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMatchStarted -= OnMatchStarted;
            GameManager.Instance.OnMatchEnded -= OnMatchEnded;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        cardDetected = (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED);

        // Try to spawn the building when the card is detected
        TrySpawnBuilding();
    }

    private void OnMatchStarted()
    {
        Debug.Log("Game: Match Started");
        gameRunning = true;

        // Try to spawn the building if the card is detected
        TrySpawnBuilding();
    }

    private void OnMatchEnded()
    {
        Debug.Log("Game: Match Ended");
        gameRunning = false;

        // Destroy the spawned building when the game ends
        if (spawnedBuilding != null)
        {
            Destroy(spawnedBuilding);
            spawnedBuilding = null;
        }
    }

    private void TrySpawnBuilding()
    {
        // Spawn the building only if the card is detected, the game is running, and no building is currently spawned
        if (cardDetected && gameRunning && buildingPrefab != null && spawnedBuilding == null)
        {
            // Instantiate the building at the card's position
            spawnedBuilding = Instantiate(buildingPrefab, transform.position, transform.rotation);
            spawnedBuilding.SetActive(true);
            Debug.Log("Building spawned at " + transform.position);
        }
        else if (!cardDetected || !gameRunning)
        {
            // If the conditions are not met, ensure the building is destroyed
            if (spawnedBuilding != null)
            {
                Destroy(spawnedBuilding);
                spawnedBuilding = null;
                Debug.Log("Building removed");
            }
        }
    }
}
