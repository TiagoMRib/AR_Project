using UnityEngine;
using Vuforia;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class RoadBlockCard : MonoBehaviour
{
    public GameObject roadBlockPrefab;
    public float health = 100f; // Initial health of the roadblock

    public float manaCost = 30f;
    public float healthDecayRate = 5f; // Health decay per second
    public Canvas cardCanvas;
    public TextMeshProUGUI statusText; // UI for messages

    private ObserverBehaviour observerBehaviour;
    private bool cardDetected = false;
    private bool gameRunning = false;
    private GameObject spawnedRoadBlock = null;

    // Y Position for roadblock alignment (set this to match the height of the river and bridges)
    public float yAlignmentPosition = 0.5f;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        GameManager.Instance.OnMatchStarted += OnMatchStarted;
        GameManager.Instance.OnMatchEnded += OnMatchEnded;

        // Initialize UI
        if (statusText != null)
        {
            statusText.text = ""; // Clear initial status text
        }

        if (cardCanvas != null)
        {
            cardCanvas.gameObject.SetActive(false);
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
        TryPlaceRoadBlock();
    }

    private void OnMatchStarted()
    {
        gameRunning = true;
        if (cardCanvas != null)
        {
            cardCanvas.gameObject.SetActive(true);
        }
        TryPlaceRoadBlock();
    }

    private void OnMatchEnded()
    {
        gameRunning = false;
        if (cardCanvas != null)
        {
            cardCanvas.gameObject.SetActive(false);
        }
        if (spawnedRoadBlock != null)
        {
            Destroy(spawnedRoadBlock);
            spawnedRoadBlock = null;
        }
    }

    private void TryPlaceRoadBlock()
    {
        Debug.Log("Trying to place RoadBlock");
        if (cardDetected && gameRunning && roadBlockPrefab != null && spawnedRoadBlock == null)
        {
            // Check if the player has enough mana
            if (GameManager.Instance.CanSpendMana(manaCost))
            {
                Vector3 placementPosition = transform.position;
                
                // Adjust Y position to match river and bridges
                placementPosition.y = yAlignmentPosition;

                // Deduct the mana
                GameManager.Instance.ManaSystem.SpendMana(manaCost);

                // Instantiate the roadblock
                spawnedRoadBlock = Instantiate(roadBlockPrefab, placementPosition, transform.rotation);
                spawnedRoadBlock.transform.SetParent(transform); // Parent to the card
                spawnedRoadBlock.SetActive(true);

                // Add NavMeshObstacle component to the roadblock
                var navObstacle = spawnedRoadBlock.AddComponent<NavMeshObstacle>();
                navObstacle.carving = true; // Enable carving to update NavMesh dynamically
                navObstacle.size = new Vector3(1f, 1f, 1f); // Adjust size as needed

                StartCoroutine(HealthDecay());
            }
            else
            {
                // Display a message if there is not enough mana
                if (statusText != null)
                {
                    statusText.text = $"-{manaCost} Mana";
                    statusText.color = Color.red;
                }
                Debug.Log("Not enough mana to place the RoadBlock.");
            }
        }
    }

    private IEnumerator HealthDecay()
    {
        while (health > 0)
        {
            health -= healthDecayRate * Time.deltaTime;
            yield return null;
        }

        // When health reaches zero, destroy the roadblock
        if (spawnedRoadBlock != null)
        {
            Debug.Log("RoadBlock Died");
            Destroy(spawnedRoadBlock);
            spawnedRoadBlock = null;
        }
    }
}
