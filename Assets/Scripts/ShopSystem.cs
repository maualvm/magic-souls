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

    [SerializeField]
    Transform Container;

    [SerializeField]
    Transform ShopItemTemplate;

    private bool bShopIsOpen;

    private void Awake()
    {
        ShopUI.SetActive(false);
        bShopIsOpen = false;
        //ShopItemTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateItemButton(Item.GetSprite(Item.ItemType.FireSpell_2), "Fire Spell Lvl 2", Item.GetCost(Item.ItemType.FireSpell_2), "Fire", 0);
        CreateItemButton(Item.GetSprite(Item.ItemType.WaterSpell_2), "Water Spell Lvl 2", Item.GetCost(Item.ItemType.WaterSpell_2), "Water", 1);
        CreateItemButton(Item.GetSprite(Item.ItemType.EarthSpell_2), "Earth Spell Lvl 2", Item.GetCost(Item.ItemType.EarthSpell_2), "Earth", 2);
        CreateItemButton(Item.GetSprite(Item.ItemType.AirSpell_2), "Air Spell Lvl 2", Item.GetCost(Item.ItemType.AirSpell_2), "Air", 3);
        CreateItemButton(Item.GetSprite(Item.ItemType.FirePotion_Strong), "Fire Potion (Strong)", Item.GetCost(Item.ItemType.FirePotion_Strong), "Fire", 4);
        CreateItemButton(Item.GetSprite(Item.ItemType.WaterPotion_Strong), "Water Potion (Strong)", Item.GetCost(Item.ItemType.WaterPotion_Strong), "Water", 5);
        CreateItemButton(Item.GetSprite(Item.ItemType.EarthPotion_Strong), "Earth Potion (Strong)", Item.GetCost(Item.ItemType.EarthPotion_Strong), "Earth", 6);
        CreateItemButton(Item.GetSprite(Item.ItemType.AirPotion_Strong), "Air Potion (Strong)", Item.GetCost(Item.ItemType.AirPotion_Strong), "Air", 7);
        CreateItemButton(Item.GetSprite(Item.ItemType.FirePotion_Weak), "Fire Potion (Weak)", Item.GetCost(Item.ItemType.FirePotion_Weak), "Fire", 8);
        CreateItemButton(Item.GetSprite(Item.ItemType.WaterPotion_Weak), "Water Potion (Weak)", Item.GetCost(Item.ItemType.WaterPotion_Weak), "Water", 9);
        CreateItemButton(Item.GetSprite(Item.ItemType.EarthPotion_Weak), "Earth Potion (Weak)", Item.GetCost(Item.ItemType.EarthPotion_Weak), "Earth", 10);
        CreateItemButton(Item.GetSprite(Item.ItemType.AirPotion_Weak), "Air Potion (Weak)", Item.GetCost(Item.ItemType.AirPotion_Weak), "Air", 11);

        CreateItemButton(Item.GetSprite(Item.ItemType.HealthPotion), "Health Potion", Item.GetCost(Item.ItemType.HealthPotion), "General", 12);
        CreateItemButton(Item.GetSprite(Item.ItemType.StaminaPotion), "Stamina Potion", Item.GetCost(Item.ItemType.StaminaPotion), "General", 13);
        CreateItemButton(Item.GetSprite(Item.ItemType.Shield), "Shield", Item.GetCost(Item.ItemType.Shield), "General", 14);
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

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, string itemElement, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(ShopItemTemplate, Container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        //Set shop item position
        float shopItemHeight = 90f;
        float shopItemWidth = 370f;
        shopItemRectTransform.anchoredPosition = new Vector2(shopItemWidth * ((positionIndex % 4) - 2), -shopItemHeight * (positionIndex / 4));

        if(positionIndex % 4 != 0)
        {
            shopItemRectTransform.anchoredPosition += new Vector2(100 * (positionIndex % 4), 0);
        }

        if((positionIndex / 4) != 0)
        {
            shopItemRectTransform.anchoredPosition += new Vector2(0,-100 * (positionIndex / 4));
        }

        if(positionIndex >= 12)
        {
            shopItemRectTransform.anchoredPosition += new Vector2((positionIndex / 12) * 250, -50);
        }
        


        //Set shop item texts and icons
        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("cost").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

        if (itemElement == "General")
        {
            shopItemTransform.Find("soul_Image").GetComponent<Image>().sprite = GameAssets.i.GeneralSoul;
        }
        else if (itemElement == "Fire")
        {
            shopItemTransform.Find("soul_Image").GetComponent<Image>().sprite = GameAssets.i.FireSoul;
        }
        else if (itemElement == "Water")
        {
            shopItemTransform.Find("soul_Image").GetComponent<Image>().sprite = GameAssets.i.WaterSoul;
        }
        else if (itemElement == "Earth")
        {
            shopItemTransform.Find("soul_Image").GetComponent<Image>().sprite = GameAssets.i.EarthSoul;
        }
        else if (itemElement == "Air")
        {
            shopItemTransform.Find("soul_Image").GetComponent<Image>().sprite = GameAssets.i.AirSoul;
        }

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
