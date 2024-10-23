using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject batCard;
    private bool hasPlayedCard = false;
    // Start is called before the first frame update
    void Start()
    {
        hasPlayedCard = false;
        

    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0) && !hasPlayedCard) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log("Hit detected");
                PlayCard(hit.point);
                hasPlayedCard = true;
            }
        }
    }
    
    void PlayCard(Vector3 spawnPosition) {
        // Spawn the card prefab at the clicked position or detected Vuforia target position
        Instantiate(batCard, spawnPosition, Quaternion.identity);

        // If you're using Photon, notify other players about the card placement
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SyncCardSpawn", RpcTarget.Others, spawnPosition, Quaternion.identity);

        // Mark that the player has played their card (so they can't play again)
        hasPlayedCard = true;
    }


}
