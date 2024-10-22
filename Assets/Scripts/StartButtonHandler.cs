using UnityEngine;

public class StartGameButtonHandler : MonoBehaviour
{
    public GameInitialization gameInitialization; // Reference to the GameInitialization script

    public void OnStartGameButtonClick()
    {
        if (gameInitialization != null)
        {
            gameInitialization.StartGame();
        }
    }
}
