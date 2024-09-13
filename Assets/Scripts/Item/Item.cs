using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
public interface IUseableItem { public void UseItem(); }
public abstract class Item
{
    public ItemData Data;
    public abstract Item Clone();
    public abstract bool IsEmpty();
}

[System.Serializable]
public class CountableItem : Item
{
    public int MaxAmount;
    public int Amount;


    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }
    public int AddAmountAndGetExcess(int amount)
    {
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);
        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }
    public CountableItem Seperate(int amount)
    {
        // ������ �Ѱ� ������ ���, ���� �Ұ�
        if (Amount <= 1) return null;

        if (amount > Amount - 1)
            amount = Amount - 1;

        Amount -= amount;

        var newItem = new CountableItem();
        newItem.Amount = amount;
        return newItem;
    }

    public override Item Clone()
    {
        var item = new CountableItem();
        item.Data = this.Data;
        item.Amount = this.Amount;
        item.MaxAmount = this.MaxAmount;
        return item;
    }

    public override bool IsEmpty() { return Amount <= 0; }
}


public abstract class EquipmentData
{
    public abstract EquipmentData Clone();
}

[System.Serializable]
public class RangedWeaponData : EquipmentData
{
    public int MaxAmmo;
    public int cunAmmo;
    public ItemData AmmoData;
    public GameObject weaponPrefab;
    public override EquipmentData Clone() => new RangedWeaponData { cunAmmo = cunAmmo, AmmoData = AmmoData };
}

[System.Serializable]
public class MeleeWeaponData : EquipmentData
{
    public GameObject weaponPrefab;
    public override EquipmentData Clone() => new MeleeWeaponData { weaponPrefab = this.weaponPrefab};
}

[System.Serializable]
public class ArmorData : EquipmentData
{
    public Avatar avatar;
    public override EquipmentData Clone() => new ArmorData { };
}

[System.Serializable]
public class EquipmentItem : Item, IUseableItem
{
    public EquipmentData EquipmentData;
    public int Durability;
    public override bool IsEmpty() { return Durability <= 0; }
    public override Item Clone()
    {
        var item = new EquipmentItem();
        item.EquipmentData = this.EquipmentData.Clone();
        item.Data = this.Data;
        item.Durability = Durability;

        return item;
    }
    public void UseItem()
    {
        if (EquipmentData is ArmorData armorData)
        {

        }
        else
        {
            GameManager.Instance.Player.EquipmentWeapon(this);
        }
    }
}