using UnityEngine;
using Vuforia;

public class GameInitialization : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    private bool gameStarted = false;
    private bool cardDetected = false;

    public GameObject arenaPrefab; // The arena prefab to enable/disable
    public Canvas startGameCanvas; // The canvas containing the "Start Game" button

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            // Register for status change events
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Initially hide the canvas and arena
        if (startGameCanvas) startGameCanvas.gameObject.SetActive(false);
        if (arenaPrefab) arenaPrefab.SetActive(false);

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

    // This method is called whenever the target's status changes
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            // The Image Target has been detected
            Debug.Log("The Image has been detected");
            cardDetected = true;
        }
        else
        {
            // The Image Target is no longer visible
            cardDetected = false;
            StopGame();
        }

        UpdateUIVisibility();
    }

    // Called when the game is supposed to start
    public void StartGame()
    {
        Debug.Log("Game started!");
        gameStarted = true;

        // Parent the arena to the card to keep it fixed relative to the card
        if (arenaPrefab != null)
        {
            arenaPrefab.transform.SetParent(transform, worldPositionStays: true);
            arenaPrefab.SetActive(true);
        }

        UpdateUIVisibility();
        GameManager.Instance.StartMatch(); // Start the game logic
    }

    // Stops the game
    public void StopGame()
    {
        DestroyAllTroopsInLayer(7);
        DestroyAllTroopsInLayer(8);
        gameStarted = false;
        UpdateUIVisibility();
        GameManager.Instance.EndMatch(); // Change later to pause?
    }
    
    public void DestroyAllTroopsInLayer(int layer)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                Destroy(obj);
            }
        }

        Debug.Log("All troops in layer " + layer + " have been destroyed.");
    }


    // Handles UI visibility based on card detection and game state
    private void UpdateUIVisibility()
    {
        if (arenaPrefab) arenaPrefab.SetActive(cardDetected);
        if (startGameCanvas) startGameCanvas.gameObject.SetActive(!gameStarted && cardDetected);
    }

    private void OnMatchStarted()
    {
        UpdateUIVisibility();
    }

    private void OnMatchEnded()
    {
        UpdateUIVisibility();
    }
}
