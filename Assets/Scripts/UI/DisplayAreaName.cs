using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayAreaName : MonoBehaviour
{
    [SerializeField]
    private GameObject OldlynTxt;

    [SerializeField]
    private GameObject ByburnTxt;

    [SerializeField]
    private GameObject EarthshadeTxt;

    [SerializeField]
    private GameObject LakehavenTxt;

    [SerializeField]
    private GameObject NorthfalconTxt;

    [SerializeField]
    private float showTime = 3f;

    private void OnEnable()
    {
        AreaTrigger.EnteredArea += AreaName;
    }

    private void OnDisable()
    {
        AreaTrigger.EnteredArea -= AreaName;
    }

    private void Start()
    {
        AreaName("Town");
    }

    private void DeactivateAll()
    {
        OldlynTxt.SetActive(false);
        ByburnTxt.SetActive(false);
        EarthshadeTxt.SetActive(false);
        LakehavenTxt.SetActive(false);
        NorthfalconTxt.SetActive(false);
    }

    private void AreaName(string area)
    {
        DeactivateAll();
        StartCoroutine(ShowText(area));
    }

    IEnumerator ShowText(string area)
    {
        if (area == "Town")
        {
            OldlynTxt.SetActive(true);
        }
        else if (area == "Fire")
        {
            ByburnTxt.SetActive(true);
        }
        else if (area == "Earth")
        {
            EarthshadeTxt.SetActive(true);
        }
        else if (area == "Water")
        {
            LakehavenTxt.SetActive(true);
        }
        else if (area == "Air")
        {
            NorthfalconTxt.SetActive(true);
        }

        yield return new WaitForSeconds(showTime);

        DeactivateAll();
    }
}
