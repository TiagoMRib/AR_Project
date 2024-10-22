using UnityEngine;
using Vuforia;
using Photon.Pun;  

public class VuforiaCardHandler : MonoBehaviour {
    public GameObject batCardPrefab;


    private void Start() {
        

        if (VuforiaBehaviour.Instance == null) {
            Debug.LogError("VuforiaBehaviour.Instance is null!");
        }

        if (batCardPrefab == null) {
            Debug.LogError("batCardPrefab is not assigned!");
            
        } else {
            Debug.Log("batCardPrefab is assigned: " + batCardPrefab.name);
        }





    }



    public void SpawnCardPrefab() {

       
            Debug.Log("Vuforia Card Handler -- Bat detected");

            Instantiate(batCardPrefab, transform.position, transform.rotation);
            
            GameObject tempMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempMarker.transform.position = transform.position;
            tempMarker.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Smaller size for visibility
            Debug.Log("Instantiating batCardPrefab at position: " + transform.position + " and rotation: " + transform.rotation);

            // Notify other players 
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncCardSpawn", RpcTarget.Others, transform.position, transform.rotation);
        
        
    }

    [PunRPC]
    private void SyncCardSpawn(Vector3 position, Quaternion rotation) {
        Instantiate(batCardPrefab, position, rotation);
    }
}