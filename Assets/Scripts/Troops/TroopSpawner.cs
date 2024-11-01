using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Collections;
using TMPro;

public class TroopSpawner : MonoBehaviour
{
    public GameObject troopPrefab; // The troop prefab to spawn
    public Transform spawnPoint; // Where the troop will spawn relative to the card
    public Canvas summonCanvas; // Reference to the canvas containing the summon text UI

    public float troopManaCost; // Mana cost for summoning this troop
    public float summonCooldown = 3f; // Cooldown time after summoning

    private ObserverBehaviour observerBehaviour;
    private bool cardDetected = false;
    private bool gameRunning = false;
    private bool isCooldownActive = false; // Track cooldown status
    private TextMeshProUGUI manaCostText; // Text element to display mana cost

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Initially hide the summon canvas
        if (summonCanvas != null)
        {
            summonCanvas.gameObject.SetActive(false);
            manaCostText = summonCanvas.GetComponentInChildren<TextMeshProUGUI>(); // Find Text component within summonCanvas
        }

        // Subscribe to game state changes
        GameManager.Instance.OnMatchStarted += OnMatchStarted;
        GameManager.Instance.OnMatchEnded += OnMatchEnded;

        // Display mana cost on the UI
        UpdateManaCostDisplay();
    }

    void OnDestroy()
    {
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }

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
        gameRunning = true;
        UpdateSummonCanvasVisibility();
    }

    private void OnMatchEnded()
    {
        gameRunning = false;
        UpdateSummonCanvasVisibility();
    }

    private void UpdateSummonCanvasVisibility()
    {
        // Show the summon canvas only if the game is running, the card is detected
        if (summonCanvas != null)
        {
            summonCanvas.gameObject.SetActive(gameRunning && cardDetected);
            UpdateManaCostDisplay();
        }
    }

    private void UpdateManaCostDisplay()
    {
        if (manaCostText != null)
        {
            // Display the mana cost and change color based on affordability
            manaCostText.text = $"-{troopManaCost} Mana";
            if (GameManager.Instance.ManaSystem.currentMana >= troopManaCost)
            {
                manaCostText.color = Color.blue; // Sufficient mana
            }
            else
            {
                manaCostText.color = Color.red; // Insufficient mana
            }
        }
    }

    private void SummonTroop()
    {
        if (cardDetected && gameRunning && !isCooldownActive && troopPrefab != null && spawnPoint != null)
        {
            // Check if the player has enough mana
            if (GameManager.Instance.CanSpendMana(troopManaCost))
            {
                // Instantiate the troop at the specified spawn point
                GameObject spawnedTroop = Instantiate(troopPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedTroop.SetActive(true);

                Debug.Log("Troop summoned!");

                // Start the cooldown after summoning
                StartCoroutine(SummonCooldown());
            }
            else
            {
                Debug.Log("Not enough mana to summon troop.");
            }
        }
    }

    private IEnumerator SummonCooldown()
    {
        isCooldownActive = true;
        UpdateSummonCanvasVisibility(); // Hide canvas during cooldown

        yield return new WaitForSeconds(summonCooldown);

        isCooldownActive = false;
        UpdateSummonCanvasVisibility(); // Show canvas again after cooldown
    }

    void Update()
    {
        // Continuously update the mana cost color in case mana changes
        UpdateManaCostDisplay();

        // Summon troop when the card is detected and conditions are met
        if (cardDetected && gameRunning && !isCooldownActive)
        {
            SummonTroop();
        }
    }
}
