using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeCardManager : Photon.PunBehaviour {

    public MemeCard card;
    private List<string> deck;
    private int cardCount = 0;
    private string currentCardUrl;

    void Start()
    {
        deck = new List<string>();

        deck.Add("http://vinithtrolls.com/wp-content/uploads/2016/10/Thani-Oruvan-Tamil-Meme-Templates-21.jpg");
        deck.Add("https://pbs.twimg.com/profile_images/560104957261512704/2g0XY6O9.jpeg");
        deck.Add("https://i.imgflip.com/1s1wyq.jpg");
        deck.Add("https://i.imgflip.com/x41yq.jpg");
        deck.Add("https://i.imgflip.com/1551tg.jpg");
        deck.Add("https://i.imgflip.com/gioi5.jpg");

        deck.Shuffle();
        PickCard();
    }

    public void PickCard()
    {
        SendPickedCardToEveryone(deck[cardCount]);

        if (cardCount == (deck.Count - 1))
        {
            deck.Shuffle();
            cardCount = -1;
        }

        cardCount++;
    }

    public void SendPickedCardToEveryone(string url)
    {
        photonView.RPC("ShowCurrentCard", PhotonTargets.All, url);
    }

    [PunRPC]
    public void ShowCurrentCard(string url)
    {
        card.url = url;
        card.LoadImage();
    }
}