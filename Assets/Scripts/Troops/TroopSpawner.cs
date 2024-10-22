using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TroopSpawner : MonoBehaviour
{
    public GameObject troopPrefab; // The troop prefab to spawn
    public Transform spawnPoint; // Where the troop will spawn relative to the card
    public Canvas summonCanvas; // Reference to the canvas containing the summon button

    private ObserverBehaviour observerBehaviour;
    private bool cardDetected = false;
    private bool gameRunning = false;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            // Register for status change events
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Initially hide the summon canvas
        if (summonCanvas != null)
        {
            summonCanvas.gameObject.SetActive(false);
            Button summonButton = summonCanvas.GetComponentInChildren<Button>();
            if (summonButton != null)
            {
                summonButton.onClick.AddListener(SummonTroop); // Add listener to the summon button
            }
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
        UpdateSummonCanvasVisibility();
    }

    private void OnMatchStarted()
    {
        Debug.Log("UI: Match Started");
        gameRunning = true;
        UpdateSummonCanvasVisibility();
    }

    private void OnMatchEnded()
    {
        gameRunning = false;
        UpdateSummonCanvasVisibility();
    }

    public void OnTargetFound()
    {
        Debug.Log("UI: Card Detected");
        cardDetected = true;
        UpdateSummonCanvasVisibility();
    }

    public void OnTargetLost()
    {
        Debug.Log("UI: Card Lost");
        cardDetected = false;
        UpdateSummonCanvasVisibility();
    }

    private void UpdateSummonCanvasVisibility()
    {
        // Show the summon canvas only if the game is running and the card is detected
        if (summonCanvas != null)
        {
            Debug.Log("UI: Canvas visible? " + (gameRunning && cardDetected));
            summonCanvas.gameObject.SetActive(gameRunning && cardDetected);
        }
    }

    private void SummonTroop()
    {
        if (cardDetected && gameRunning && troopPrefab != null && spawnPoint != null)
        {
            // Instantiate the troop at the specified spawn point
            GameObject summonedTroop = Instantiate(troopPrefab, spawnPoint.position, spawnPoint.rotation);
            summonedTroop.SetActive(true);
            Debug.Log("Troop summoned!");
        }
    }
}
