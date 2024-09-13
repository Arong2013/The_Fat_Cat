using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory => GameManager.Instance.Player.inventory;
    [SerializeField] Transform slotsParent;
    [SerializeField] ItemSlot weaponSlot, armorSlot, itemSlot;
    [SerializeField] TextMeshProUGUI infoUI;
    [SerializeField] Button OpenBtn;
    public void OpenUI()
    {
        if (GameManager.Instance.Player != null)
            gameObject.SetActive(!gameObject.activeSelf);
    }
    void OnEnable()
    {
        PopulateInventorySlots();
    }

    void OnDisable()
    {
        ClearInventorySlots();
    }

    void PopulateInventorySlots()
    {
        foreach (var item in inventory.Items)
        {
            var slot = Instantiate(itemSlot, slotsParent);
            var itemSlotComponent = slot.GetComponent<ItemSlot>();
            itemSlotComponent.SetItem(item);
           // itemSlotComponent.AddPointer<IPointerDownHandler>(() => TrySetShortCut(item));
            itemSlotComponent.uiEventActions[UIEventType.PointerUp] += (PointerEventData) => {TryUseItem(item); StopAllCoroutines();};
        }
    }

    void ClearInventorySlots()
    {
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }
    }
    public void UpdateUI()
    {
        ClearEmptySlots();
      //  UpdateInfoUI();

    }

    void ClearEmptySlots()
    {
        foreach (Transform child in slotsParent)
        {
            if (child.TryGetComponent<ItemSlot>(out ItemSlot component) && component.item != null && component.item.IsEmpty())
            {
                Instantiate(itemSlot.gameObject, slotsParent);
                Destroy(child.gameObject);
            }
        }
    }
    // void UpdateInfoUI()
    // {
    //     if (infoUI == null) return;

    //     infoUI.text = "";
    //     var stats = GameManager.Instance.Player.actorComponents.OfType<IStatus>().FirstOrDefault().GetStatComponent<SurvivalStats>();
    //     foreach (var field in typeof(SurvivalStats).GetFields(BindingFlags.Public | BindingFlags.Instance))
    //     {
    //         if (field.GetValue(stats) is Stat stat)
    //         {
    //             infoUI.text += $"{field.Name}: {stat.Value}\n";
    //         }
    //     }
    // }
    bool TryUseItem(Item item)
    {
        if (item is IUseableItem usableItem)
        {
            usableItem.UseItem();
            UpdateUI();
            return true;
        }
        return false;
    }
    // bool TrySetShortCut(Item item)
    // {
    //     if (item is CountableItem && item is IUsableItem)
    //     {
    //         StartCoroutine(OnSetShortCut(item));
    //         return true;
    //     }
    //     return false;
    // }

    // IEnumerator OnSetShortCut(Item _item)
    // {
    //     float touchDownCount = 0f;
    //     while (touchDownCount <= 60f)
    //     {
    //         touchDownCount += 1f;
    //         yield return null;
    //     }
    //     UiUtils.GetUI<UnderBtnsUI>().SetShortCut(_item, () => TryUseItem(_item));
    //     gameObject.SetActive(false);
    // }

    // public void SetPlayer(Char player)
    // {
    //     OpenBtn.onClick.AddListener(() => { OpenUI(player); });
    // }
}
