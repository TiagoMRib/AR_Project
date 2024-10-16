using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        Debug.Log("GameManager: Game has officially started.");
        // Enable other game systems, like troop spawning, etc.
    }

    public void EndGame()
    {
        gameStarted = false;
        Debug.Log("GameManager: Game has ended.");
        // Handle end-of-game logic here
    }
}
