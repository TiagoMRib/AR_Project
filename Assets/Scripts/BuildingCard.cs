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
    private GameObject spawnedBuilding = null; // To keep track of the spawned building
    private bool isBuilding = false; // Flag to indicate if a building is under construction

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        GameManager.Instance.OnMatchStarted += OnMatchStarted;
        GameManager.Instance.OnMatchEnded += OnMatchEnded;

        // Initialize UI elements
        if (manaCostText != null)
        {
            manaCostText.text = "Cost: " + manaCost;
        }
        if (statusText != null)
        {
            statusText.text = ""; // Clear the status text initially
        }

        // Ensure the UI is hidden at the start
        if (cardCanvas != null)
        {
            cardCanvas.enabled = false;
        }
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
        TrySpawnBuilding();
    }

    private void OnMatchStarted()
    {
        gameRunning = true;

        // Show the UI when the match starts
        if (cardCanvas != null)
        {
            cardCanvas.enabled = true;
        }

        TrySpawnBuilding();
    }

    private void OnMatchEnded()
    {
        gameRunning = false;

        // Hide the UI when the match ends
        if (cardCanvas != null)
        {
            cardCanvas.enabled = false;
        }

        if (spawnedBuilding != null)
        {
            Destroy(spawnedBuilding);
            spawnedBuilding = null;
        }
    }

    private void TrySpawnBuilding()
    {
        if (cardDetected && gameRunning && buildingPrefab != null && spawnedBuilding == null && !isBuilding)
        {
            if (GameManager.Instance.CanSpendMana(manaCost))
            {
                StartCoroutine(BuildBuilding()); // Start the building process
            }
            else
            {
                // Display insufficient mana message
                if (statusText != null)
                {
                    statusText.text = "Not enough mana!";
                }
            }
        }
        else if (!cardDetected || !gameRunning)
        {
            if (spawnedBuilding != null)
            {
                Destroy(spawnedBuilding);
                spawnedBuilding = null;
            }
        }
    }

    private IEnumerator BuildBuilding()
    {
        isBuilding = true;
        GameManager.Instance.ManaSystem.SpendMana(manaCost); // Spend the mana

        if (statusText != null)
        {
            statusText.text = "Building..."; // Show the building status
        }

        yield return new WaitForSeconds(buildDelay); // Wait for the build delay

        // Instantiate and set up the building
        spawnedBuilding = Instantiate(buildingPrefab, transform.position, transform.rotation);
        spawnedBuilding.transform.SetParent(transform); // Parent the building to the card
        spawnedBuilding.transform.localPosition = new Vector3(0, 0.1f, 0); // Adjust Y position if needed
        spawnedBuilding.SetActive(true);

        if (statusText != null)
        {
            statusText.text = ""; // Clear the status text
        }

        isBuilding = false;
    }
}
