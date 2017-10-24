using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Photon.PunBehaviour
{
    public Transform scoreboardPanel;
    public Dictionary<int, GameObject> playerScoreItemList;
    public GameObject playerScoreItem;
    public Vector2 offsetListPerItem = new Vector2(0f, -85f);

    private PhotonPlayer[] players;
    private Vector2 originalScoreboardSize;

    private void Start()
    {
        PhotonNetwork.player.SetScore(0);

        playerScoreItemList = new Dictionary<int, GameObject>();
        originalScoreboardSize = scoreboardPanel.GetComponent<RectTransform>().sizeDelta;

        LoadAllPlayers();

        UpdateScoreToPlayer(PhotonNetwork.player.ID);
    }

    public void LoadAllPlayers()
    {
        players = PhotonNetwork.playerList;
        foreach (PhotonPlayer player in players)
        {
            AddPlayerToList(player);
        }

        RectTransform listPanelSize = scoreboardPanel.GetComponent<RectTransform>();
        listPanelSize.sizeDelta = listPanelSize.sizeDelta - (offsetListPerItem * playerScoreItemList.Count);
    }

    public void AddPlayerToList(PhotonPlayer player)
    {
        GameObject item = Instantiate(playerScoreItem);
        item = SetPlayerInfo(item, player);
        item.name = player.NickName;
        item.transform.SetParent(scoreboardPanel, false);

        RectTransform position = item.GetComponent<RectTransform>();
        position.anchoredPosition = position.anchoredPosition + (offsetListPerItem * playerScoreItemList.Count);

        playerScoreItemList.Add(player.ID, item);
    }

    public void RemovePlayerFromList(PhotonPlayer player)
    {
        Destroy(playerScoreItemList[player.ID]);
        playerScoreItemList.Remove(player.ID);

        RepositionList();
    }

    public void RepositionList()
    {
        int count = 0;

        foreach (KeyValuePair<int, GameObject> listItem in playerScoreItemList)
        {
            RectTransform position = listItem.Value.GetComponent<RectTransform>();
            position.anchoredPosition = position.anchoredPosition + (offsetListPerItem * count);
            count++;
        }

        count = 0;
    }

    public GameObject SetPlayerInfo(GameObject listItem, PhotonPlayer player)
    {
        Text playerText = listItem.transform.GetChild(0).GetComponent<Text>();
        playerText.text = player.NickName;

        Text scoreText = listItem.transform.GetChild(1).GetComponent<Text>();
        scoreText.text = "Awesome score: " + player.GetScore();

        //Text statusText = listItem.transform.GetChild(2).GetComponent<Text>();
        //statusText.text = player.SetCustomProperties();

        return listItem;
    }

    public void UpdateScoreToPlayerAll(int playerId)
    {
        PhotonPlayer player = PhotonPlayer.Find(playerId);
        player.AddScore(1);
        photonView.RPC("UpdateScoreToPlayer", PhotonTargets.All, playerId);
    }

    [PunRPC]
    public void UpdateScoreToPlayer(int playerId)
    {
        PhotonPlayer player = PhotonPlayer.Find(playerId);
        SetPlayerInfo(playerScoreItemList[playerId], player);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        AddPlayerToList(newPlayer);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        RemovePlayerFromList(otherPlayer);
    }
}