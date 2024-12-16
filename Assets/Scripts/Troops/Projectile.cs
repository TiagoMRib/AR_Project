using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // The target the projectile will hit
    private float speed; // Speed of the projectile
    private float damage = 10; // Damage dealt by the projectile

    public void SetTarget(Transform target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
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
                    building.TakeDamage(damage); 
                }

                // Destroy the projectile after hitting
                Destroy(gameObject);
            }
        }
    }
}
