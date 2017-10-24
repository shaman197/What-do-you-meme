using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeManager : Photon.PunBehaviour
{
    public DropZone tableTop;
    public CaptionCardManager captioncardManager;
    public MemeCardManager memeCardManager;
    public GameObject WinnerPanel;

    private int maxScore = 1;

    public void Start()
    {
        object value = new object();

        if (PhotonNetwork.room.CustomProperties.TryGetValue("MaxScore", out value) && !value.Equals(""))
            maxScore = Convert.ToInt32(value.ToString());
    }

    public void DetermineNextAction()
    {
        if (PhotonNetwork.player.GetScore() == maxScore)
        {
            ShowWinner();
        }

        else
        {
            StartNewRound();
        }
    }

    private void StartNewRound()
    {
        tableTop.canDrop = true;
        captioncardManager.DrawCards(1);

        if (PhotonNetwork.isMasterClient)
            memeCardManager.PickCard();
    }

    private void ShowWinner()
    {
        photonView.RPC("ShowWinnerToAll", PhotonTargets.All);
    }

    [PunRPC]
    private void ShowWinnerToAll()
    {
        WinnerPanel.SetActive(true);
        WinnerPanel.transform.GetChild(0).GetComponent<Text>().text = "The winner is " + PhotonNetwork.playerName;
    }

    public void BackToLobby()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("Launcher");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
