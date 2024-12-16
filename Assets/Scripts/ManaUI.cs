using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaUI : MonoBehaviour
{
    public Slider manaSlider;  // UI Slider to represent the mana bar
    public TextMeshProUGUI manaText;      // UI Text to display current mana

    private ManaSystem manaSystem;

    private void Start()
    {
        // Get reference to the ManaSystem from the GameManager
        if (GameManager.Instance != null)
        {
            manaSystem = GameManager.Instance.ManaSystem;
        }

        // Check if the UI components are set
        if (manaSlider == null || manaText == null)
        {
            Debug.LogError("ManaUI: Please assign the Mana Slider and Mana Text in the inspector.");
        }
    }

    private void Update()
    {
        if (manaSystem != null)
        {
            // Update the mana bar and text
            manaSlider.value = manaSystem.currentMana / manaSystem.maxMana; // Update slider (0 to 1)
            manaText.text = $"{Mathf.RoundToInt(manaSystem.currentMana)} / {manaSystem.maxMana}"; // Display current and max mana
        }
    }
}
