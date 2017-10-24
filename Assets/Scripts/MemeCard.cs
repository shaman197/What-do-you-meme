using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MemeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string url = "";
    private float ScaleSizeOnHover = 2f;

    private Image meme;

	private void Start()
    {
        meme = transform.GetChild(0).GetComponent<Image>();
    }

    private IEnumerator RealLoadImage()
    {
        WWW www = new WWW(url);

        yield return www;

        if (www.texture != null)
        {


            Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
            meme.sprite = sprite;
            meme.color = new Color(meme.color.r, meme.color.b, meme.color.g, 1);
        }

        else
        {
            Debug.Log(url + " DOES NOT WORK!");
        }

        yield return null;
    }

    public void LoadImage()
    {
        StartCoroutine(RealLoadImage());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = new Vector3(ScaleSizeOnHover, ScaleSizeOnHover, ScaleSizeOnHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }
}
