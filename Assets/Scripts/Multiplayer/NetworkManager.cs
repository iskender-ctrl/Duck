using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private MultiplayerPropertyCashe propertyCache;
    private bool isMatchmaking = false;

    #region Unity Callbacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        SetUp();
        AddButtonListeners();
    }
    #endregion

    #region SetUp
    private void SetUp()
    {
        propertyCache = GetComponent<MultiplayerPropertyCashe>();
    }
    #endregion

    #region Button Listeners
    private void AddButtonListeners()
    {
        propertyCache.playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        if (!isMatchmaking)
        {
            isMatchmaking = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion

    #region Matchmaking
    private IEnumerator MatchmakingTimeout()
    {
        Debug.Log("NetworkManager.MatchmakingTimeout - Started");
        yield return new WaitForSeconds(propertyCache.matchmakingTimeout);

        if (PhotonNetwork.InRoom)
        {
            Debug.Log("NetworkManager.MatchmakingTimeout - Room is full. Transitioning to GameScene");
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            isMatchmaking = false;
            Debug.Log("NetworkManager.MatchmakingTimeout - Matchmaking timeout exceeded. Please try again later.");
        }

        Debug.Log("NetworkManager.MatchmakingTimeout - Finished");
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        QuickMatch();
    }
    private void QuickMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateNewRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    private void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        string roomName = "Room_" + Random.Range(1000, 9999);

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(MatchmakingTimeout());
    }
    #endregion
}
