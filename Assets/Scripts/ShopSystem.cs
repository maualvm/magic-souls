using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    GameObject ShopUI;

    private bool bShopIsOpen;

    private void Awake()
    {
        ShopUI.SetActive(false);
        bShopIsOpen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Player.TriggeredShop += HandleShopTrigger;
    }

    private void OnDisable()
    {
        Player.TriggeredShop -= HandleShopTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleShopTrigger(bool bOpenStore)
    {
        if(bOpenStore)
        {
            ShopUI.SetActive(true);
            bShopIsOpen = true;
        }
        else
        {
            ShopUI.SetActive(false);
            bShopIsOpen = false;
        }
    }
}
