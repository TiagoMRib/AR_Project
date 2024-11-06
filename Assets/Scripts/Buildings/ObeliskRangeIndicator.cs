using Unity.VisualScripting;
using UnityEngine;

public class ObeliskRangeIndicator : MonoBehaviour
{
    public float range = 10f;  // Set this to the range of your turret
    public int segments = 50; // Number of points in the circle
    private LineRenderer lineRenderer;
    public Vector3 offset = Vector3.zero; // Offset for the center of the circle
    private Obelisk obelisk;
  
    void Start()
    {
        obelisk = GetInactiveChild<Obelisk>() ;

        GameManager.Instance.OnMatchStarted += OnMatchStarted;
        if (obelisk != null)
        {
            range = obelisk.attackRange;
        }
        else
        {
            Debug.Log("ObeliskRangeIndicator: Obelisk not found");

        }
        
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.startWidth = 0.05f; 
        lineRenderer.endWidth = 0.05f;
        lineRenderer.enabled = false;
        
    }

    private void OnMatchStarted()
    {
        DrawRangeCircle();
        lineRenderer.enabled = false;
    }

    public void ActivateRangeIndicator()
    {
        if (lineRenderer != null)
        {
            Debug.Log("Activate range indicator");
            lineRenderer.enabled = true;
            
        }
    }

    public void DeactivateRangeIndicator()
    {
        if (lineRenderer != null)
        {
            Debug.Log("Deactivate range indicator");
            lineRenderer.enabled = false;
        }
    }

    

    void DrawRangeCircle()
    {
        Vector3 center = transform.position;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * (range) + center.x;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * (range) + center.z;

            lineRenderer.SetPosition(i, new Vector3(x, center.y, z));
            angle += 360f / segments;
        }
    }
    
    private T GetInactiveChild<T>() where T : Component
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                T component = child.GetComponent<T>();
                child.gameObject.SetActive(false); // Reset to inactive
                if (component != null) return component;
            }
        }
        return null;
    }

    

    
}
