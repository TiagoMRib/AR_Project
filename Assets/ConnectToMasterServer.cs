using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class ConnectToMasterServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("CONNECTING TO MASTER SERVER");
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
