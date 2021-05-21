using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TooltipWarningScreenSpaceUI : MonoBehaviour
{
    public static TooltipWarningScreenSpaceUI Instance { get; private set; }

    [SerializeField]
    private RectTransform canvasRectTransform;

    private RectTransform BackgroundRectTransform;
    private Image BackgroundImage;
    private TextMeshProUGUI TextMeshPro;
    private RectTransform rectTransform;

    private Func<string> getTooltipTextFunc;

    private float showTimer;
    private float flashTimer;
    private int flashState;
    float flashTimerMax;

    private void Awake()
    {
        Instance = this;
        flashTimerMax = 0.033f;
        BackgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        BackgroundImage = transform.Find("background").GetComponent<Image>();
        TextMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        HideTooltip();
    }

    private void SetText(string tooltipText)
    {
        TextMeshPro.SetText(tooltipText);
        TextMeshPro.ForceMeshUpdate();

        Vector2 textSize = TextMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(TextMeshPro.margin.x * 2, TextMeshPro.margin.y * 2);
        BackgroundRectTransform.sizeDelta = textSize + paddingSize;
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

        flashTimer += Time.deltaTime;
        
        if(flashTimer > flashTimerMax)
        {
            flashState++;
            switch(flashState)
            {
                case 1:
                case 3:
                case 5:
                    TextMeshPro.color = new Color(1, 1, 1, 1);
                    BackgroundImage.color = new Color(178f / 255f, 0 / 255f, 0 / 255f, 1);
                    break;
                case 2:
                case 4:
                    TextMeshPro.color = new Color(178f / 255f, 0 / 255f, 0 / 255f, 1);
                    BackgroundImage.color = new Color(1, 1, 1, 1);
                    break;
            }
        }
        showTimer -= Time.deltaTime;

        if(showTimer <= 0)
        {
            HideTooltip();
        }
    }

    private void ShowTooltip(string tooltipText, float showTimerMax = 2f)
    {
        ShowTooltip(() => tooltipText, showTimerMax);
    }
    private void ShowTooltip(Func<string> getTooltipTextFunc, float showTimerMax = 2f)
    {
        this.getTooltipTextFunc = getTooltipTextFunc;
        gameObject.SetActive(true);
        SetText(getTooltipTextFunc());
        showTimer = showTimerMax;
        flashTimer = 0f;
        flashState = 0;
        Update();
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipText, float showTimerMax = 2f)
    {
        Instance.ShowTooltip(tooltipText, showTimerMax);
    }

    public static void ShowTooltip_Static(Func<string> getTooltipTextFunc, float showTimerMax = 2f)
    {
        Instance.ShowTooltip(getTooltipTextFunc, showTimerMax);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }
}
