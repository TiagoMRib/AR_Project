using UnityEngine;
using System.Collections;

public class MainCastle : BaseBuilding
{
    //public GameManager gameManager;  

    public GameObject intactModel;    // The model for the intact castle
    public GameObject brokenModel;    // The model for the broken castle (below 50% health)
    public GameObject destroyedModel;

    protected override void Start()
    {
        base.Start();
        maxHealth = 500f;  
        isDefensiveBuilding = true;  
        UpdateModel(); 
    }

    protected override void Update()
    {
        base.Update();
        UpdateModel(); 
    }


    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        UpdateModel(); // Update the model based on the new health
    }


    protected override void Die()
    {
        base.Die();
        // Trigger the game-over condition (see later)
        Debug.Log("Game Over! Castle destroyed.");
    }

    private void UpdateModel()
    {
        // Enable/disable models based on health percentage
        if (currentHealth > 0.5*maxHealth)
        {
            // Castle is in good condition
            intactModel.SetActive(true);
            brokenModel.SetActive(false);
            destroyedModel.SetActive(false);
        }
        else if (currentHealth > 0.25*maxHealth)
        {
            // Castle is damaged
            intactModel.SetActive(false);
            brokenModel.SetActive(true);
            destroyedModel.SetActive(false);
        }
        else
        {
            // Castle is destroyed
            intactModel.SetActive(false);
            brokenModel.SetActive(false);
            destroyedModel.SetActive(true);
        }
    }
}
