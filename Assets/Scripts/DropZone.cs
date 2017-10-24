using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool tableTopZone;
    public bool canDrop = true;
    public CaptionCardManager captionManager;

    private void Start()
    {
        captionManager = FindObjectOfType<CaptionCardManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        CaptionCard drag = eventData.pointerDrag.GetComponent<CaptionCard>();

        if(drag != null)
        {
            if(canDrop)
            {
                drag.parentToReturn = this.transform;
                drag.canDrag = false;
                captionManager.AddCardToTable(drag.playerId, drag.captionText);
            }

            if (tableTopZone)
            {
                canDrop = false;

                if (captionManager.CheckEveryonePlayedACard())
                {
                    captionManager.ActivateShowTableTopCards();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.pointerDrag)
            return;

        Draggable drag = eventData.pointerDrag.GetComponent<Draggable>();

        if (drag != null && canDrop)
        {
            drag.placeholderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.pointerDrag && canDrop)
            return;

        if(eventData.pointerDrag != null)
        {
            Draggable drag = eventData.pointerDrag.GetComponent<Draggable>();

            if (drag != null && drag.placeholderParent == this.transform)
            {
                drag.placeholderParent = this.transform;
            }
        }
    }
}
