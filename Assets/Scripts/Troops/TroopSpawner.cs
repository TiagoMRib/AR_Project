using UnityEngine;
using Vuforia;
using System.Collections;
using TMPro;

public class TroopSpawner : MonoBehaviour
{
    // Public variables for setup
    public GameObject troopPrefab; // The troop prefab to spawn
    public Transform spawnPoint; // The point where the troop spawns
    public Canvas summonCanvas; // UI canvas for displaying mana cost
    public float troopManaCost; // Mana cost to summon a troop
    public float summonCooldown = 3f; // Cooldown between summons

    // Internal variables
    private ObserverBehaviour observerBehaviour;
    private bool cardDetected = false;
    private bool isCooldownActive = false;
    private TextMeshProUGUI manaCostText; // UI text for mana cost display

    void Start()
    {
        // Get the Vuforia ObserverBehaviour component
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Setup the summon canvas and mana cost text
        if (summonCanvas != null)
        {
            summonCanvas.gameObject.SetActive(false);
            manaCostText = summonCanvas.GetComponentInChildren<TextMeshProUGUI>();
            UpdateManaCostDisplay();
        }
    }

    void OnDestroy()
    {
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        // Check if the card is currently being tracked
        cardDetected = (targetStatus.Status == Status.TRACKED);

        // Show or hide the UI based on detection
        summonCanvas.gameObject.SetActive(cardDetected);

        // Disable the spawner if the card is no longer detected
        if (!cardDetected)
        {
            // Instead of destroying, deactivate this GameObject
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (cardDetected && GameManager.Instance.IsMatchRunning && !isCooldownActive)
        {
            if (GameManager.Instance.ManaSystem.currentMana >= troopManaCost)
            {
                SummonTroop();
            }
        }
        
        // Continuously update the mana cost display
        UpdateManaCostDisplay();
    }

    private void SummonTroop()
    {
        // Instantiate the troop at the spawn point
        GameObject spawnedTroop = Instantiate(troopPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedTroop.SetActive(true);

        // Deduct mana for the summon
        GameManager.Instance.ManaSystem.SpendMana(troopManaCost);

        // Start the cooldown
        StartCoroutine(SummonCooldown());
    }

    private IEnumerator SummonCooldown()
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(summonCooldown);
        isCooldownActive = false;
    }

    private void UpdateManaCostDisplay()
    {
        if (manaCostText != null && GameManager.Instance.IsMatchRunning)
        {
            // Update the text and color based on current mana
            manaCostText.text = $"-{troopManaCost} Mana";
            manaCostText.color = GameManager.Instance.ManaSystem.currentMana >= troopManaCost ? Color.blue : Color.red;
        }
    }
}
