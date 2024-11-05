using System.Collections;
using System.Linq;
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    public Transform leftBridge;           // Reference to the left bridge position
    public Transform rightBridge;          // Reference to the right bridge position
    public GameObject[] cardPrefabs;       // Array of cards the AI can play
    public float playInterval = 20f;        // Time interval between AI plays
    public Transform enemyCastle;          // Reference to the player's castle

    private int currentEnergy;
    private bool gameStarted = false;      // Flag to check if the game has started

    

    void Update()
    {
        // Start the AI routine only if the game has started
        if (gameStarted && !IsInvoking(nameof(PlayCardRoutine)))
        {
            InvokeRepeating(nameof(PlayCardRoutine), 0f, playInterval);
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        Debug.Log("Game started. AI is now active.");
    }
    
    public void FinishGame()
    {
        gameStarted = false;
        Debug.Log("Game finished. AI is now inactive.");
    }

    void PlayCardRoutine()
    {
        
        PlayCard();
        
    }

    void PlayCard()
    {
       
        GameObject cardToPlay = cardPrefabs[Random.Range(0, cardPrefabs.Length)];

  
        Transform targetSide = FindClosestEnemySide();

      
        if (targetSide != null)
        {
            Instantiate(cardToPlay, targetSide.position, Quaternion.identity);
            Debug.Log("AI played a card at " + targetSide.name);
        }
        else
        {
            // Default to a random side if no enemies are detected
            targetSide = Random.Range(0, 2) == 0 ? leftBridge : rightBridge;
            Instantiate(cardToPlay, targetSide.position, Quaternion.identity);
            Debug.Log("AI played a card at a random location: " + targetSide.name);
        }
        
    }

    Transform FindClosestEnemySide()
    {
       
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Team1");

        if (enemyUnits.Length == 0)
            return null;

        // Find the closest enemy to the AI’s castle
        GameObject closestEnemy = enemyUnits
            .OrderBy(unit => Vector3.Distance(unit.transform.position, enemyCastle.position))
            .FirstOrDefault();

        
        if (closestEnemy != null)
        {
            float enemyXPosition = closestEnemy.transform.position.x;
            float midpointX = (leftBridge.position.x + rightBridge.position.x) / 2;

            
            return enemyXPosition < midpointX ? leftBridge : rightBridge;
        }

        return null;
    }

    
}
