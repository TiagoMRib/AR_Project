using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseTroop : MonoBehaviour
{
    public bool flyer = false;

    public string troopType = "Normal";
    public float health = 100f;
    public float damage = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public float speed = 5f;
    public string teamTag = "Team1";
    public string enemyTeamTag;

    protected Transform currentTarget;
    protected bool isAttacking = false;

    protected Animator animator;

    public List<Transform> pathPoints = new List<Transform>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemyTeamTag = (teamTag == "Team1") ? "Team2" : "Team1";
        animator = GetComponentInChildren<Animator>();
        Debug.Log("Animator found: " + animator);
        FindTarget();
    }

    protected virtual void Update()
    {
        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget();
            }
            else if (!isAttacking)
            {
                Attack();
            }
        }
    }

    private void BaseMovement()
    {
        if (currentTarget == null) return;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, step);

        Vector3 direction = currentTarget.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step * 100f);
        }
    }

    protected virtual void MoveTowardsTarget()
    {
        if (flyer)
        {
            BaseMovement();
        }
        else
        {
            if (IsTargetAcrossRiver())
            {
                if (pathPoints.Count == 0)
                {
                    FindBridgePath();
                }

                if (pathPoints.Count > 0)
                {
                    Transform nextPoint = pathPoints[0];
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, nextPoint.position, step);

                    if (Vector3.Distance(transform.position, nextPoint.position) < 0.1f)
                    {
                        pathPoints.RemoveAt(0);
                    }
                }
            }
            else
            {
                BaseMovement();
            }
        }
    }

    private bool IsTargetAcrossRiver()
    {
        return Mathf.Abs(transform.position.z) < 3 && Mathf.Abs(currentTarget.position.z) > 3 || 
               Mathf.Abs(transform.position.z) > 3 && Mathf.Abs(currentTarget.position.z) < 3;
    }

    private void FindBridgePath()
{
    GameObject[] bridges = GameObject.FindGameObjectsWithTag("Bridge");

    if (bridges.Length == 0)
    {
        Debug.LogWarning("No bridges found in the scene.");
        return;
    }

    float closestDistance = Mathf.Infinity;
    GameObject closestBridge = null;

    foreach (GameObject bridge in bridges)
    {
        float distance = Vector3.Distance(transform.position, bridge.transform.position);
        if (distance < closestDistance)
        {
            closestDistance = distance;
            closestBridge = bridge;
        }
    }

    // If a closest bridge is found, add its start and end points to path points
    if (closestBridge != null)
    {
        // Assuming the bridge has a script that defines its start and end points
        Bridge bridgeScript = closestBridge.GetComponent<Bridge>(); // Replace Bridge with your actual bridge script

        if (bridgeScript != null)
        {
            // Move towards the start of the bridge first
            pathPoints.Add(bridgeScript.startPoint); // Assumes startPoint is a Transform
            pathPoints.Add(bridgeScript.endPoint);   // Assumes endPoint is a Transform
            pathPoints.Add(currentTarget);            // Move towards the original target after crossing
        }
        else
        {
            Debug.LogWarning("Bridge script not found on the closest bridge.");
        }
    }
}

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);
    }

    protected abstract void FindTarget();
    protected abstract void Attack();
}
