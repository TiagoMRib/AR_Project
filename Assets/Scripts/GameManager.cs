using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameInitialization gameInitialization;
    public bool IsMatchRunning { get; private set; } = false;

    public ManaSystem ManaSystem;

    public event Action OnMatchStarted;
    public event Action OnMatchEnded;
    
    public AIOpponent AIOpponent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public void StartMatch()
    {
        IsMatchRunning = true;
        Debug.Log("Summon: StartMatch called, IsMatchRunning set to true.");

        AIOpponent.StartGame();

        if (ManaSystem != null)
        {
            ManaSystem.Initialize(); // Reset and initialize mana
        }

        OnMatchStarted?.Invoke(); // Trigger the event
        Debug.Log("Summon: OnMatchStarted event invoked.");
    }

    public void EndMatch()
    {
        IsMatchRunning = false;
        Debug.Log("Summon: EndMatch called, IsMatchRunning set to false.");

        OnMatchEnded?.Invoke(); // Trigger the event
        AIOpponent.FinishGame();
        Debug.Log("Summon: OnMatchEnded event invoked.");
    }

    public bool CanSpendMana(float amount)
    {
        return ManaSystem != null && ManaSystem.SpendMana(amount);
    }
}
