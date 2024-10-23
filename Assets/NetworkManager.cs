using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    
    RoomInfo[] rooms;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log(PhotonNetwork.CountOfPlayers);
        Debug.Log(PhotonNetwork.PlayerList);
    }
    public void CreateRoom(string roomName)
    {
        Debug.Log("Creating room");
        RoomOptions roomOptions = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = 2};
        PhotonNetwork.CreateRoom(roomName,roomOptions);
    }
    public void JoinRoom(string roomName)
    {
        Debug.Log("Joining room");
        if(PhotonNetwork.PlayerList.Length <= 4)
        {
            Debug.Log("All set to join");
            PhotonNetwork.JoinRoom(roomName);
        }
    }
    
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMasterServer");
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("CreateRoomFailed");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("JoinedRoom");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("JoinRoomFailed");
        Debug.Log(message);
    }
}