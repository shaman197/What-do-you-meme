using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotePlayer : MonoBehaviour
{
    private Button voteButton;
    private CaptionCard card;
    private PlayerManager playerManager;
    private CaptionCardManager captionCardManager;

    public void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        captionCardManager = FindObjectOfType<CaptionCardManager>();

        card = GetComponentInParent<CaptionCard>();

        voteButton = GetComponent<Button>();
        voteButton.onClick.AddListener(VoteActions);
    }

    private void VoteActions()
    {
        playerManager.UpdateScoreToPlayerAll(card.playerId);
        MakeAllCardsUninteractable();
        card.HighlightCard();
        captionCardManager.ActivateCleanTableTop();
    }

    private void MakeAllCardsUninteractable()
    {
        VotePlayer[] voteButtons = FindObjectsOfType<VotePlayer>();

        foreach (VotePlayer voteButton in voteButtons)
        {
            Button button = voteButton.GetComponent<Button>();
            button.interactable = false;
        }
    }
}
