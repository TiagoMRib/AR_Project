using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsMatchRunning { get; private set; } = false;

    public event Action OnMatchStarted;
    public event Action OnMatchEnded;

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
        OnMatchStarted?.Invoke();
        Debug.Log("Match started!");
    }

    public void EndMatch()
    {
        IsMatchRunning = false;
        OnMatchEnded?.Invoke();
        Debug.Log("Match ended.");
    }
}
