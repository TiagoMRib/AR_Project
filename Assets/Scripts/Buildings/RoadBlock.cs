public class RoadBlock : BaseBuilding
{
    protected override void Start()
    {
        base.Start();
        buildingName = "Road Block";
        maxHealth = 200f;
        isDefensiveBuilding = false;
    }

        protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}