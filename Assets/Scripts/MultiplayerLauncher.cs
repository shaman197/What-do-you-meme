using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerLauncher : Photon.PunBehaviour
{
    public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
    public byte maxPlayersPerRoom = 7;

    public GameObject playerPanel;
    public InputField playerInputfield;
    public Button startGameButton;
    public GameObject roomSettingPanel;
    public InputField scoreInputfield;
    public InputField blankCardsInputField;

    private static string GameVersion = "v4.2";
    private bool isConnecting;

    private string playerKey = "PlayerName";
    private Dictionary<string, GameObject> roomList;
    private Dictionary<string, GameObject> playerList;
    private string currentActiveRoomName;

    private void Start()
    {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.logLevel = logLevel;
        PhotonNetwork.autoCleanUpPlayerObjects = true;

        if (PlayerPrefs.HasKey(playerKey))
        {
            string name = PlayerPrefs.GetString(playerKey);
            playerInputfield.text = name;
        }

        roomList = new Dictionary<string, GameObject>();
        playerList = new Dictionary<string, GameObject>();
    }

    public void FillPlayerName()
    {
        PhotonNetwork.playerName = playerInputfield.text;
        PlayerPrefs.SetString(playerKey, playerInputfield.text);
    }

    public void Connect()
    {
        isConnecting = true;

        if (!PhotonNetwork.connected)
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings(GameVersion);
        }
    }

    public void CreateAndJoinRoom()
    {
        ExitGames.Client.Photon.Hashtable CustomOptions = new ExitGames.Client.Photon.Hashtable();
        CustomOptions.Add("MaxScore", scoreInputfield.text);
        CustomOptions.Add("BlankCards", blankCardsInputField.text);

        PhotonNetwork.CreateRoom("Single room", 
            new RoomOptions() {
                MaxPlayers = maxPlayersPerRoom,
                CustomRoomProperties = CustomOptions
            }, TypedLobby.Default);
    }

    public void JoinRandomRoom()
    {
        // #Critical we need at this point to attempt joining a random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed().
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    public void StartGame()
    {
        StartCoroutine(DelayLoad());
    }

    public IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel("CardGame");
    }

    #region Photon override functions 
    public override void OnConnectedToMaster()
    {
        // If you're in game and leave the scene this function is called
        playerPanel.SetActive(true);
        roomSettingPanel.SetActive(false);
    }

    public override void OnJoinedLobby()
    {
        JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        // If there are no rooms or all rooms are full
        roomSettingPanel.SetActive(true);
    }

    public override void OnDisconnectedFromPhoton()
    {
        // If you have to do something if you disconnect
    }

    public override void OnReceivedRoomListUpdate()
    {
        // If more rooms are available
    }

    public override void OnJoinedRoom()
    {
        StartGame();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        
    }
    #endregion
}