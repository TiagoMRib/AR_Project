using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

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

    public NavMeshAgent agent;
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
            agent.SetDestination(currentTarget.position);
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
