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
        Debug.Log("Cursor Entering " + name + " GameObject");
        TooltipScreenSpaceUI.ShowTooltip_Static(_description);
    }

    public void HideMyText()
    {
        Debug.Log("Cursor Exiting " + name + " GameObject");
        TooltipScreenSpaceUI.HideTooltip_Static();
    }
}
