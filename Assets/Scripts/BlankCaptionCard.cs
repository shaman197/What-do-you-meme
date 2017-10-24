using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlankCaptionCard : CaptionCard
{
    private InputField inputField;

    public override void OnEnable()
    {
        base.OnEnable();

        inputField = transform.GetChild(1).GetComponent<InputField>();
    }

    public void ChangeCaptionText()
    {
        captionText = inputField.text;
    }

    public void ConfirmChangedText()
    {
        Destroy(transform.GetChild(2).gameObject);
        Destroy(inputField.gameObject);
        ShowText();
    }
}
