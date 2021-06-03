using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField]
    private string _description = "";

    public void SetDescription(string description)
    {
        _description = description;
    }

    public void ShowMyText()
    {
        TooltipScreenSpaceUI.ShowTooltip_Static(_description);
    }

    public void HideMyText()
    {
        TooltipScreenSpaceUI.HideTooltip_Static();
    }
}
