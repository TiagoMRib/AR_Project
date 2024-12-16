using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    
    public Button startGameBtn;
    
  

    public void OnStartBtn()
    {
        SceneManager.LoadScene("Virtual_scene");
    }

    public void OnQuitBtn()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;  
        #else
                Application.Quit();  
        #endif
    }

   

    
}    