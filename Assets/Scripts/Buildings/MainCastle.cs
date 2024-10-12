using UnityEngine;
using System.Collections;

public class MainCastle : BaseBuilding
{
    //public GameManager gameManager;  // Reference to GameManager to handle win/loss conditions

    protected override void Start()
    {
        base.Start();
        buildingName = "Main Castle";
        maxHealth = 500f;  
        isDefensiveBuilding = true;  
    }

    protected override void Attack()
    {
        // MainCastle does not have an attack function
    }

    protected override void Die()
    {
        base.Die();
        // Trigger the game-over condition (see later)
        Debug.Log("Game Over! Main Castle destroyed.");
    }
}
