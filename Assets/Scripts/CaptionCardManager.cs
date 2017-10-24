using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptionCardManager : Photon.PunBehaviour
{
    public CaptionCard cardPrefab;
    public BlankCaptionCard blankCardPrefab;
    public Transform hand;
    public Transform tableTop;
    public GameModeManager gameModeManager;
    public float waitTimeForCleaningTable = 1f;

    private List<string> deck;
    private int currentDrawPosition = 0;

    void Start()
    {
        deck = new List<string>();

        deck.Add("When ur food sound like WWII in the microwave, but comes out cold");
        deck.Add("When you get to the party and immediatlely have to shit");
        deck.Add("You can't be late for your job, if you don't have a job in the first place");
        deck.Add("When the red light just turned green and someone's already beeping at you");
        deck.Add("When you get to the party and immediatlely have to shit");
        deck.Add("You can't get fired, if you don't have a job");
        deck.Add("When someone you don't like makes a joke");
        deck.Add("Whe fangirling in front of people");

        object value;

        if(PhotonNetwork.room.CustomProperties.TryGetValue("BlankCards", out value) && !value.Equals(""))
            AddBlankCards(System.Convert.ToInt32(value.ToString()));

        deck.Shuffle();
 
        DrawCards(7);
    }

    public void DrawCards(int amount)
    {
        for (int x = currentDrawPosition; x < (currentDrawPosition + amount); x++)
        {
            CaptionCard card = (deck[x] != "") ? Instantiate(cardPrefab, hand) : Instantiate(blankCardPrefab, hand);
            card.captionText = deck[x];
            card.ShowText();
            card.playerId = PhotonNetwork.player.ID;
        }

        currentDrawPosition = currentDrawPosition + amount;
    }

    public void AddBlankCards(int amount)
    {
        for (int x = 0; x < amount; x++)
        {
            deck.Add("");
        }
    }

    public void ActivateShowTableTopCards()
    {
        StartCoroutine(ShowTableTopCards());
    }

    private IEnumerator ShowTableTopCards()
    {
        yield return new WaitForEndOfFrame();

        foreach (Transform gameobject in tableTop)
        {
            CaptionCard card = gameobject.GetComponent<CaptionCard>();

            if(card != null)
            {
                card.ShowText();

                //if(card.playerId != PhotonNetwork.player.ID)
                    card.addVoteButton();
            }
        }

        yield return null;
    }

    public void ActivateCleanTableTop()
    {
        StartCoroutine(CleanTableTop());
    }

    private IEnumerator CleanTableTop()
    {
        yield return new WaitForSecondsRealtime(waitTimeForCleaningTable);

        foreach (Transform card in tableTop)
        {
            Destroy(card.gameObject);
        }

        gameModeManager.DetermineNextAction();

        yield return null;
    }

    public bool CheckEveryonePlayedACard()
    {
        if (tableTop.childCount == PhotonNetwork.playerList.Length)
            return true;
        else
            return false;
    }

    [PunRPC]
    public void AddHiddenCardToTable(int playerId, string text)
    {
        CaptionCard card = Instantiate(cardPrefab, tableTop);
        card.playerId = playerId;
        card.captionText = text;

        if (CheckEveryonePlayedACard())
        {
            ActivateShowTableTopCards();
        }
    }

    public void AddCardToTable(int playerId, string text)
    {
        photonView.RPC("AddHiddenCardToTable", PhotonTargets.Others, playerId, text);
    }

    public void DeleteCardsFromDisconnectedPlayer(int playerId)
    {
        foreach (Transform gameobject in tableTop)
        {
            CaptionCard card = gameobject.GetComponent<CaptionCard>();

            if (card != null)
            {
                if(card.playerId == playerId)
                {
                    Destroy(gameobject.gameObject);
                }
            }
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        DeleteCardsFromDisconnectedPlayer(otherPlayer.ID);
    }
}

static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
