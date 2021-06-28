using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TooltipScreenSpaceUI : MonoBehaviour
{
    public static TooltipScreenSpaceUI Instance { get; private set; }

    [SerializeField]
    private RectTransform canvasRectTransform;

    private RectTransform BackgroundRectTransform;
    private TextMeshProUGUI TextMeshPro;
    private RectTransform rectTransform;

    private Func<string> getTooltipTextFunc;

    private void Awake()
    {
        Instance = this;
        BackgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        TextMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        HideTooltip();
        //ShowTooltip("Hello World");
    }

    private void SetText(string tooltipText)
    {
        TextMeshPro.SetText(tooltipText);
        TextMeshPro.ForceMeshUpdate();

        Vector2 textSize = TextMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(TextMeshPro.margin.x * 2, TextMeshPro.margin.y * 2);
        BackgroundRectTransform.sizeDelta = textSize + paddingSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetText(getTooltipTextFunc());
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if(anchoredPosition.x + BackgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            //Tooltip left screen on right side
            anchoredPosition.x = canvasRectTransform.rect.width - BackgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + BackgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            //Tooltip left screen on top side
            anchoredPosition.y = canvasRectTransform.rect.height - BackgroundRectTransform.rect.height;
        }


        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void ShowTooltip(string tooltipText)
    {
        ShowTooltip(() => tooltipText);
    }
    private void ShowTooltip(Func<string> getTooltipTextFunc)
    {
        this.getTooltipTextFunc = getTooltipTextFunc;
        gameObject.SetActive(true);
        SetText(getTooltipTextFunc());
        Update();
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipText)
    {
        Instance.ShowTooltip(tooltipText);
    }

    public static void ShowTooltip_Static(Func<string> getTooltipTextFunc)
    {
        Instance.ShowTooltip(getTooltipTextFunc);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }
}
