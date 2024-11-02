using UnityEngine;
using Vuforia;
using UnityEngine.AI; 
using System.Collections;
using TMPro;

public class RoadBlockCard : MonoBehaviour
{
    public GameObject roadBlockPrefab;
    public float health = 100f; // Initial health of the roadblock
    public float healthDecayRate = 5f; // Health decay per second
    public Canvas cardCanvas;
    public TextMeshProUGUI statusText; // UI for messages

    private ObserverBehaviour observerBehaviour;
    private bool cardDetected = false;
    private bool gameRunning = false;
    private GameObject spawnedRoadBlock = null;
    private bool isPlacedOnBridge = false;

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
        TryPlaceRoadBlock();
    }

    private void OnMatchStarted()
    {
        gameRunning = true;
        if (cardCanvas != null)
        {
            cardCanvas.enabled = true;
        }
        TryPlaceRoadBlock();
    }

    private void OnMatchEnded()
    {
        gameRunning = false;
        if (cardCanvas != null)
        {
            cardCanvas.enabled = false;
        }
        if (spawnedRoadBlock != null)
        {
            Destroy(spawnedRoadBlock);
            spawnedRoadBlock = null;
        }
    }

    private void TryPlaceRoadBlock()
    {
        if (cardDetected && gameRunning && roadBlockPrefab != null && spawnedRoadBlock == null)
        {
            Vector3 placementPosition = transform.position;
            if (IsAboveBridge(placementPosition))
            {
                isPlacedOnBridge = true;
                statusText.text = ""; // Clear any warning message
                spawnedRoadBlock = Instantiate(roadBlockPrefab, placementPosition, transform.rotation);
                spawnedRoadBlock.transform.SetParent(transform); // Parent to the card
                spawnedRoadBlock.transform.localPosition = new Vector3(0, 0.1f, 0); // Adjust Y position
                spawnedRoadBlock.SetActive(true);

                // Add NavMeshObstacle component to the roadblock
                var navObstacle = spawnedRoadBlock.AddComponent<NavMeshObstacle>();
                navObstacle.carving = true; // Enable carving to update NavMesh dynamically
                navObstacle.size = new Vector3(1f, 1f, 1f); // Adjust size as needed

                //StartCoroutine(HealthDecay());
            }
            else
            {
                statusText.text = "";
            }
        }
    }

    private bool IsAboveBridge(Vector3 position)
    {
        // Perform a raycast downward to check if the object is above a bridge
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Bridge"))
            {
                Debug.Log("Roadblock placed above bridge");
                return true;
            }
        }
        Debug.Log("Roadblock not placed above bridge");
        return false;
    }

void Update()
{
    
    if (spawnedRoadBlock != null && gameRunning) // isPlacedOnBridge
    {
        health -= healthDecayRate * Time.deltaTime;
        if (health <= 0)
        {
            Debug.Log("RoadBlock Died");
            Destroy(spawnedRoadBlock);
            spawnedRoadBlock = null;
        }
        else
        {
            Debug.Log("RoadBlock Health: " + health);
        }
    }
}
/*


    private IEnumerator HealthDecay()
    {
        while (health > 0)
        {
            health -= healthDecayRate * Time.deltaTime;
            Debug.Log("RoadBlock Health: " + health);
            yield return null;
        }

        // When health reaches zero, destroy the roadblock
        if (spawnedRoadBlock != null)
        {
            
            Debug.Log("RoadBlock Died");
            Destroy(spawnedRoadBlock);
            spawnedRoadBlock = null;
        }
    }*/
}

