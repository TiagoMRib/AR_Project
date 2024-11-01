using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public float maxMana = 250f; // Maximum mana the player can hold
    public float initialMana = 50f; // Starting mana
    public float manaRegenRate = 1f; // Mana regeneration rate per second

    public float currentMana { get; private set; } // Current mana, accessible but not directly settable

    private bool isMatchRunning = false; // Track if the match is running

    private void Start()
    {
        // Subscribe to the GameManager events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMatchStarted += OnMatchStarted;
            GameManager.Instance.OnMatchEnded += OnMatchEnded;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the GameManager events to avoid memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMatchStarted -= OnMatchStarted;
            GameManager.Instance.OnMatchEnded -= OnMatchEnded;
        }
    }

    // Called when the match starts
    private void OnMatchStarted()
    {
        isMatchRunning = true;
        Initialize(); // Reset mana to initial amount when match starts
        InvokeRepeating("RegenerateMana", 1f, 1f); // Start mana regeneration
    }

    // Called when the match ends
    private void OnMatchEnded()
    {
        isMatchRunning = false;
        CancelInvoke("RegenerateMana"); // Stop mana regeneration
    }

    // Initialize or reset the mana system
    public void Initialize()
    {
        Debug.Log("Mana System Initialized");
        currentMana = initialMana;
    }

    // Regenerates mana at the specified rate up to the max mana limit
    private void RegenerateMana()
    {
        if (isMatchRunning && currentMana < maxMana)
        {
            currentMana = Mathf.Min(currentMana + manaRegenRate, maxMana);
        }
    }

    // Checks if there is enough mana and spends the specified amount if possible
    public bool SpendMana(float amount)
    {
        if (isMatchRunning && currentMana >= amount)
        {
            currentMana -= amount;
            return true; // Mana spent successfully
        }
        return false; // Not enough mana
    }

    // Adds mana when an enemy is killed, ensuring it doesn’t exceed max mana
    public void GainMana(float amount)
    {
        if (isMatchRunning)
        {
            currentMana = Mathf.Min(currentMana + amount, maxMana);
        }
    }
}
