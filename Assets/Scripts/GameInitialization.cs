using UnityEngine;
using Vuforia;

public class GameInitialization : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    private bool gameStarted = false;

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
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            // The Image Target has been detected
            Debug.Log("The Image has been detected");
            if (!gameStarted && startGameCanvas != null)
            {
                Debug.Log("Turning on canvas");
                startGameCanvas.gameObject.SetActive(true); // Show the canvas
            }
        }
        else
        {
            // The Image Target is no longer visible
            StopGame();
        }
    }

    // Called when the game is supposed to start
    public void StartGame()
    {
        Debug.Log("Game started!");
        gameStarted = true;
        if (arenaPrefab) arenaPrefab.SetActive(true); // Show the arena
        if (startGameCanvas) startGameCanvas.gameObject.SetActive(false); // Hide the canvas
        GameManager.Instance.StartGame(); // Start the game logic
    }

    // Stops the game
    private void StopGame()
    {
        gameStarted = false;
        if (arenaPrefab) arenaPrefab.SetActive(false); // Hide the arena
        if (startGameCanvas) startGameCanvas.gameObject.SetActive(false); // Hide the canvas
        GameManager.Instance.EndGame(); // Stop the game logic
    }
}
