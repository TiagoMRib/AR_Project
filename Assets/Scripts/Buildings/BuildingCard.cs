using UnityEngine;
using Vuforia;
using System.Collections;
using TMPro; // Import for TextMeshPro elements

public class BuildingCard : MonoBehaviour
{
    [Header("Building Setup")]
    public GameObject buildingPrefab; // The building prefab to spawn
    public float manaCost = 50f; // Mana cost to build the building
    public float buildDelay = 3f; // Delay before the building is constructed

    [Header("UI Elements")]
    public Canvas cardCanvas; // Canvas attached to the card
    public TextMeshProUGUI manaCostText; // Text to display mana cost
    public TextMeshProUGUI statusText; // Text to show build status ("Building..." or empty)

    private ObserverBehaviour observerBehaviour;
    private bool cardDetected = false;
    private bool gameRunning = false;
    private GameObject spawnedBuilding = null; // Track the spawned building
    private bool isBuilding = false; // Check if a building is under construction

    void Start()
    {
        // Set up Vuforia observer
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Subscribe to game state events
        GameManager.Instance.OnMatchStarted += OnMatchStarted;
        GameManager.Instance.OnMatchEnded += OnMatchEnded;

        // Initialize UI
        InitializeUI();
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
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

    private void InitializeUI()
    {
        UpdateManaCostTextColor();
        if (statusText != null)
        {
            statusText.text = ""; // Clear the status text
        }
        if (cardCanvas != null)
        {
            cardCanvas.enabled = false; // Hide the UI initially
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        // Update card detection status
        cardDetected = (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED);
        TrySpawnBuilding();
    }

    private void OnMatchStarted()
    {
        gameRunning = true;
        if (cardCanvas != null)
        {
            cardCanvas.enabled = true; // Show UI when the game starts
        }
        TrySpawnBuilding();
    }

    private void OnMatchEnded()
    {
        gameRunning = false;
        if (cardCanvas != null)
        {
            cardCanvas.enabled = false; // Hide UI when the game ends
        }
        DestroyCurrentBuilding();
    }

    private void TrySpawnBuilding()
    {
        UpdateManaCostTextColor();

        if (cardDetected && gameRunning && !isBuilding && spawnedBuilding == null)
        {
            if (GameManager.Instance.ManaSystem.currentMana >= manaCost)
            {
                StartCoroutine(BuildBuilding());
            }
            else
            {
                ShowStatusMessage(""); // Clear status message
            }
        }
        else if (!cardDetected || !gameRunning)
        {
            DestroyCurrentBuilding();
        }
    }

    private IEnumerator BuildBuilding()
    {
        isBuilding = true;
        GameManager.Instance.ManaSystem.SpendMana(manaCost); // Deduct mana cost

        ShowStatusMessage("Building..."); // Show building status
        manaCostText.gameObject.SetActive(false); // Hide mana cost text during construction

        yield return new WaitForSeconds(buildDelay); // Wait for the build delay

        // Instantiate and activate the building
        spawnedBuilding = Instantiate(buildingPrefab, transform.position, transform.rotation);
        spawnedBuilding.transform.SetParent(transform); // Attach to the card
        spawnedBuilding.transform.localPosition = new Vector3(0, 0.1f, 0); // Adjust position
        spawnedBuilding.SetActive(true);

        ShowStatusMessage(""); // Clear status text
        manaCostText.gameObject.SetActive(true); // Show mana cost text after building
        isBuilding = false;
    }

    private void DestroyCurrentBuilding()
    {
        if (spawnedBuilding != null)
        {
            Destroy(spawnedBuilding);
            spawnedBuilding = null;
        }
        isBuilding = false;
        manaCostText.gameObject.SetActive(true); // Show mana cost text when no building is present
    }

    private void UpdateManaCostTextColor()
    {
        if (manaCostText != null)
        {
            if (GameManager.Instance.ManaSystem.currentMana >= manaCost)
            {
                manaCostText.color = Color.blue; // Sufficient mana
            }
            else
            {
                manaCostText.text = $"-{manaCost} Mana";
                manaCostText.color = Color.red; // Insufficient mana
            }
        }
    }

    private void ShowStatusMessage(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.gameObject.SetActive(!string.IsNullOrEmpty(message)); // Only show status text if there's a message
        }
    }
}
