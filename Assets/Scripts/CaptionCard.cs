using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptionCard : Draggable
{
    public string captionText = "";
    public int playerId;

    private Text cardText;

    public virtual void OnEnable()
    {
        cardText = transform.GetChild(0).GetComponent<Text>();
    }

    public void HideText()
    {
        cardText.text = "";
    }

    public void ShowText()
    {
        if(captionText != "")
            cardText.text = captionText;
    }

    public void HighlightCard()
    {
        GetComponent<Image>().color = new Color(0f, 192f, 255f);
    }
}
