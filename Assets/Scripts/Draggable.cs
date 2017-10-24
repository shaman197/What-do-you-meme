using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject placeHolderPrefab;
    public Button voteButtonPrefab;
    public Transform parentToReturn = null;
    public Transform placeholderParent;
    public bool canDrag = true;

    private Vector2 mouseOffset;
    private CanvasGroup canvasGroup;
    private GameObject placeholder;
    private Button voteButton;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        parentToReturn = transform.parent;
        placeholderParent = parentToReturn;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            placeholder = (GameObject)Instantiate(placeHolderPrefab);
            placeholder.transform.SetParent(transform.parent);
            placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

            mouseOffset = transform.position - (Vector3)eventData.position;

            transform.SetParent(transform.parent.parent);

            canvasGroup.blocksRaycasts = false;

            RemoveVoteButton();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            transform.position = eventData.position + mouseOffset;

            if (placeholder.transform.parent != placeholderParent)
                placeholder.transform.SetParent(placeholderParent);

            int newSiblingIndex = placeholderParent.childCount;

            for (int x = 0; x < placeholderParent.childCount; x++)
            {
                if (transform.position.x < placeholderParent.GetChild(x).position.x)
                {
                    newSiblingIndex = x;

                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                        newSiblingIndex--;

                    break;
                }
            }

            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(placeholder != null)
        {
            transform.SetParent(parentToReturn);
            transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

            canvasGroup.blocksRaycasts = true;

            Destroy(placeholder);
            //placeholder = null;
        }
    }

    public void addVoteButton()
    {
        if (voteButton == null)
        {
            voteButton = Instantiate(voteButtonPrefab, transform);
        }
    }

    public void RemoveVoteButton()
    {
        if (voteButton != null)
        {
            Destroy(voteButton.gameObject);
        }
    }
}
