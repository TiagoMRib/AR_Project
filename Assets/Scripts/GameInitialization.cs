using UnityEngine;
using Vuforia;

public class GameInitialization : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    private bool gameStarted = false;

    public GameObject arenaPrefab; // The arena prefab to show when the game starts

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            // Register for status change events
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    void OnDestroy()
    {
        if (observerBehaviour)
        {
            // Unregister from status change events
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    // This method is called whenever the target's status changes
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED ||
            targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            // The Image Target has been detected
            if (!gameStarted)
            {
                StartGame();
            }
        }
        else
        {
            // The Image Target is no longer visible
            StopGame();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        arenaPrefab.SetActive(true); // Enable the arena prefab
        GameManager.Instance.StartGame(); // Start the game logic
    }

    private void StopGame()
    {
        gameStarted = false;
        arenaPrefab.SetActive(false); // Disable the arena prefab
        GameManager.Instance.EndGame(); // Stop the game logic
    }
}
