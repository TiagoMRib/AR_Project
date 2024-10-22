using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // The target the projectile will hit
    private float speed; // Speed of the projectile

    public void SetTarget(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
        Destroy(gameObject, 5f); // Destroy the projectile after 5 seconds if it doesn't hit anything
    }

    private void Update()
    {
        if (target != null)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Check if the projectile has reached the target
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Deal damage to the target
                BaseBuilding building = target.GetComponent<BaseBuilding>();
                if (building != null && !building.isDead)
                {
                    building.TakeDamage(10); // Deal 10 damage (modify as needed)
                }

                // Destroy the projectile after hitting
                Destroy(gameObject);
            }
        }
    }
}
