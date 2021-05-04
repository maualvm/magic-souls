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

    [SerializeField]
    Transform MaxedOutTemplate;

    private bool bShopIsOpen;

    public static event Action<Item.ItemType, int, string> TryBuyItem;
    public static event Action<string, int> SpellLevelUp;
    public static event Action<string> StrongPotionBought, WeakPotionBought;
    public static event Action HealthPotionBought, StaminaPotionBought, ShieldBought;

    private void Awake()
    {
        ShopUI.SetActive(false);
        bShopIsOpen = false;
        ShopItemTemplate.gameObject.SetActive(false);
        MaxedOutTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateItemButton(Item.ItemType.FireSpell_2, Item.GetSprite(Item.ItemType.FireSpell_2), "Fire Spell Lvl 2",
            Item.GetCost(Item.ItemType.FireSpell_2), "Fire", 0);
        CreateItemButton(Item.ItemType.WaterSpell_2, Item.GetSprite(Item.ItemType.WaterSpell_2), "Water Spell Lvl 2",
            Item.GetCost(Item.ItemType.WaterSpell_2), "Water", 1);
        CreateItemButton(Item.ItemType.EarthSpell_2, Item.GetSprite(Item.ItemType.EarthSpell_2), "Earth Spell Lvl 2",
            Item.GetCost(Item.ItemType.EarthSpell_2), "Earth", 2);
        CreateItemButton(Item.ItemType.AirSpell_2, Item.GetSprite(Item.ItemType.AirSpell_2), "Air Spell Lvl 2",
            Item.GetCost(Item.ItemType.AirSpell_2), "Air", 3);
        CreateItemButton(Item.ItemType.FirePotion_Strong, Item.GetSprite(Item.ItemType.FirePotion_Strong), "Fire Potion (Strong)",
            Item.GetCost(Item.ItemType.FirePotion_Strong), "Fire", 4);
        CreateItemButton(Item.ItemType.WaterPotion_Strong, Item.GetSprite(Item.ItemType.WaterPotion_Strong), "Water Potion (Strong)",
            Item.GetCost(Item.ItemType.WaterPotion_Strong), "Water", 5);
        CreateItemButton(Item.ItemType.EarthPotion_Strong, Item.GetSprite(Item.ItemType.EarthPotion_Strong), "Earth Potion (Strong)",
            Item.GetCost(Item.ItemType.EarthPotion_Strong), "Earth", 6);
        CreateItemButton(Item.ItemType.AirPotion_Strong, Item.GetSprite(Item.ItemType.AirPotion_Strong), "Air Potion (Strong)",
            Item.GetCost(Item.ItemType.AirPotion_Strong), "Air", 7);
        CreateItemButton(Item.ItemType.FirePotion_Weak, Item.GetSprite(Item.ItemType.FirePotion_Weak), "Fire Potion (Weak)",
            Item.GetCost(Item.ItemType.FirePotion_Weak), "Fire", 8);
        CreateItemButton(Item.ItemType.WaterPotion_Weak, Item.GetSprite(Item.ItemType.WaterPotion_Weak), "Water Potion (Weak)",
            Item.GetCost(Item.ItemType.WaterPotion_Weak), "Water", 9);
        CreateItemButton(Item.ItemType.EarthPotion_Weak, Item.GetSprite(Item.ItemType.EarthPotion_Weak), "Earth Potion (Weak)",
            Item.GetCost(Item.ItemType.EarthPotion_Weak), "Earth", 10);
        CreateItemButton(Item.ItemType.AirPotion_Weak, Item.GetSprite(Item.ItemType.AirPotion_Weak), "Air Potion (Weak)",
            Item.GetCost(Item.ItemType.AirPotion_Weak), "Air", 11);

        CreateItemButton(Item.ItemType.HealthPotion, Item.GetSprite(Item.ItemType.HealthPotion), "Health Potion",
            Item.GetCost(Item.ItemType.HealthPotion), "General", 12);
        CreateItemButton(Item.ItemType.StaminaPotion, Item.GetSprite(Item.ItemType.StaminaPotion), "Stamina Potion",
            Item.GetCost(Item.ItemType.StaminaPotion), "General", 13);
        CreateItemButton(Item.ItemType.Shield, Item.GetSprite(Item.ItemType.Shield), "Shield",
            Item.GetCost(Item.ItemType.Shield), "General", 14);
    }

    private void OnEnable()
    {
        Player.TriggeredShop += HandleShopTrigger;
        InventorySystem.BoughtItem += HandleItemBought;
    }

    private void OnDisable()
    {
        Player.TriggeredShop -= HandleShopTrigger;
        InventorySystem.BoughtItem -= HandleItemBought;
    }

    private void CreateItemButton(Item.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, string itemElement, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(ShopItemTemplate, Container);
        shopItemTransform.gameObject.SetActive(true);
        shopItemTransform.gameObject.name = itemType.ToString();
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        PositionButton(shopItemRectTransform, positionIndex);

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

        Button shopBtn = shopItemTransform.GetComponent<Button>();
        shopBtn.onClick.AddListener(() => TryBuyItem?.Invoke(itemType, itemCost, itemElement));
    }

    private void CreateMaxedOutLabel(int positionIndex)
    {
        Transform maxedOutTransform = Instantiate(MaxedOutTemplate, Container);
        maxedOutTransform.gameObject.SetActive(true);
        RectTransform maxedOutRectTransform = maxedOutTransform.GetComponent<RectTransform>();

        PositionButton(maxedOutRectTransform, positionIndex);
    }

    private void PositionButton(RectTransform rect, int positionIndex)
    {
        //Set shop item position
        float shopItemHeight = 90f;
        float shopItemWidth = 370f;
        rect.anchoredPosition = new Vector2(shopItemWidth * ((positionIndex % 4) - 2), -shopItemHeight * (positionIndex / 4));

        if (positionIndex % 4 != 0)
        {
            rect.anchoredPosition += new Vector2(100 * (positionIndex % 4), 0);
        }

        if ((positionIndex / 4) != 0)
        {
            rect.anchoredPosition += new Vector2(0, -100 * (positionIndex / 4));
        }

        if (positionIndex >= 12)
        {
            rect.anchoredPosition += new Vector2((positionIndex / 12) * 250, -50);
        }
    }

    private void HandleItemBought(Item.ItemType itemType)
    {
        Debug.Log("Bought item : " + itemType);
        switch (itemType)
        {
            default:
            case Item.ItemType.FireSpell_2:
                SpellLevelUp?.Invoke("Fire", 2);
                Destroy(Container.Find("FireSpell_2").gameObject);
                CreateItemButton(Item.ItemType.FireSpell_3, Item.GetSprite(Item.ItemType.FireSpell_3), "Fire Spell Lvl 3",
                    Item.GetCost(Item.ItemType.FireSpell_3), "Fire", 0);
                break;
            case Item.ItemType.FireSpell_3:
                SpellLevelUp?.Invoke("Fire", 3);
                Destroy(Container.Find("FireSpell_3").gameObject);
                CreateItemButton(Item.ItemType.FireSpell_4, Item.GetSprite(Item.ItemType.FireSpell_4), "Fire Spell Lvl 4",
                    Item.GetCost(Item.ItemType.FireSpell_4), "Fire", 0);
                break;
            case Item.ItemType.FireSpell_4:
                SpellLevelUp?.Invoke("Fire", 4);
                Container.Find("FireSpell_4").gameObject.GetComponent<Button>().interactable = false;
                CreateMaxedOutLabel(0);
                break;
            case Item.ItemType.WaterSpell_2:
                SpellLevelUp?.Invoke("Water", 2);
                Destroy(Container.Find("WaterSpell_2").gameObject);
                CreateItemButton(Item.ItemType.WaterSpell_3, Item.GetSprite(Item.ItemType.WaterSpell_3), "Water Spell Lvl 3",
                    Item.GetCost(Item.ItemType.WaterSpell_3), "Water", 1);
                break;
            case Item.ItemType.WaterSpell_3:
                SpellLevelUp?.Invoke("Water", 3);
                Destroy(Container.Find("WaterSpell_3").gameObject);
                CreateItemButton(Item.ItemType.WaterSpell_4, Item.GetSprite(Item.ItemType.WaterSpell_4), "Water Spell Lvl 4",
                    Item.GetCost(Item.ItemType.WaterSpell_4), "Water", 1);
                break;
            case Item.ItemType.WaterSpell_4:
                SpellLevelUp?.Invoke("Water", 4);
                Container.Find("WaterSpell_4").gameObject.GetComponent<Button>().interactable = false;
                CreateMaxedOutLabel(1);
                break;
            case Item.ItemType.EarthSpell_2:
                SpellLevelUp?.Invoke("Earth", 2);
                Destroy(Container.Find("EarthSpell_2").gameObject);
                CreateItemButton(Item.ItemType.EarthSpell_3, Item.GetSprite(Item.ItemType.EarthSpell_3), "Earth Spell Lvl 3",
                    Item.GetCost(Item.ItemType.EarthSpell_3), "Earth", 2);
                break;
            case Item.ItemType.EarthSpell_3:
                SpellLevelUp?.Invoke("Earth", 3);
                Destroy(Container.Find("EarthSpell_3").gameObject);
                CreateItemButton(Item.ItemType.EarthSpell_4, Item.GetSprite(Item.ItemType.EarthSpell_4), "Earth Spell Lvl 4",
                    Item.GetCost(Item.ItemType.EarthSpell_4), "Earth", 2);
                break;
            case Item.ItemType.EarthSpell_4:
                SpellLevelUp?.Invoke("Earth", 4);
                Container.Find("EarthSpell_4").gameObject.GetComponent<Button>().interactable = false;
                CreateMaxedOutLabel(2);
                break;
            case Item.ItemType.AirSpell_2:
                SpellLevelUp?.Invoke("Air", 2);
                Destroy(Container.Find("AirSpell_2").gameObject);
                CreateItemButton(Item.ItemType.AirSpell_3, Item.GetSprite(Item.ItemType.AirSpell_3), "Air Spell Lvl 3",
                    Item.GetCost(Item.ItemType.AirSpell_3), "Air", 3);
                break;
            case Item.ItemType.AirSpell_3:
                SpellLevelUp?.Invoke("Air", 3);
                Destroy(Container.Find("AirSpell_3").gameObject);
                CreateItemButton(Item.ItemType.AirSpell_4, Item.GetSprite(Item.ItemType.AirSpell_4), "Air Spell Lvl 4",
                    Item.GetCost(Item.ItemType.AirSpell_4), "Air", 3);
                break;
            case Item.ItemType.AirSpell_4:
                SpellLevelUp?.Invoke("Air", 4);
                Container.Find("AirSpell_4").gameObject.GetComponent<Button>().interactable = false;
                CreateMaxedOutLabel(3);
                break;
            case Item.ItemType.FirePotion_Strong:
                StrongPotionBought?.Invoke("Fire");
                break;
            case Item.ItemType.WaterPotion_Strong:
                StrongPotionBought?.Invoke("Water");
                break;
            case Item.ItemType.EarthPotion_Strong:
                StrongPotionBought?.Invoke("Earth");
                break;
            case Item.ItemType.AirPotion_Strong:
                StrongPotionBought?.Invoke("Air");
                break;
            case Item.ItemType.FirePotion_Weak:
                WeakPotionBought?.Invoke("Fire");
                break;
            case Item.ItemType.WaterPotion_Weak:
                WeakPotionBought?.Invoke("Water");
                break;
            case Item.ItemType.EarthPotion_Weak:
                WeakPotionBought?.Invoke("Earth");
                break;
            case Item.ItemType.AirPotion_Weak:
                WeakPotionBought?.Invoke("Air");
                break;
            case Item.ItemType.HealthPotion:
                HealthPotionBought?.Invoke();
                break;
            case Item.ItemType.StaminaPotion:
                StaminaPotionBought?.Invoke();
                break;
            case Item.ItemType.Shield:
                ShieldBought?.Invoke();
                break;
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
