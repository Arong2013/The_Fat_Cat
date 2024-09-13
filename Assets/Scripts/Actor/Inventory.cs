using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    EquipmentItem weaponItem, armorItem;
    List<Item> items = new List<Item>();

    public EquipmentItem WeaponItem
    {
        get => weaponItem;
        set
        {
            weaponItem = value;
            GameManager.Instance.Player.EquipmentWeapon(value);
        }
    }

    public EquipmentItem ArmorItem { get => armorItem; set { } }

    public List<Item> Items
    {
        get => items;
        set { items = value; Utils.GetUI<InventoryUI>().UpdateUI(); }
    }

    public int MaxAmount => (int)GameManager.Instance.Player.survivalStats.inventoryCapacity.Value;

    public bool AddItem(Item _item)
    {
        if (_item is CountableItem countItem)
        {
            var matchingItem = Items.OfType<CountableItem>().FirstOrDefault(i => i.Data == _item.Data);
            if (matchingItem != null)
            {
                countItem.SetAmount(-matchingItem.AddAmountAndGetExcess(countItem.Amount));
                if (countItem.Amount <= 0) return true;
            }
        }
        if (Items.Count >= MaxAmount)
            return false;
        Items.Add(_item);
        return true;
    }
}
