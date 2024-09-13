using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UIEventType
{
    PointerDown,
    PointerUp,
    PointerClick,
    // 추가적인 UI 이벤트를 정의할 수 있습니다.
}

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Item item;

    [SerializeField] Sprite OrizinImage;
    [SerializeField] Image iconImage;

    // UI 이벤트에 대한 딕셔너리 선언
    public Dictionary<UIEventType, Action<PointerEventData>> uiEventActions = new Dictionary<UIEventType, Action<PointerEventData>>
        {
            { UIEventType.PointerDown, null },
            { UIEventType.PointerUp, null },
            { UIEventType.PointerClick, null }
        };

    private void Awake()
    {
        OrizinImage = iconImage.sprite;
    }

    public void SetItem(Item _item)
    {
        item = _item;
        iconImage.sprite = item?.Data.IconSprite ?? OrizinImage;
        iconImage.color = iconImage.sprite != null ? Color.white : new Color(0, 0, 0, 0);
    }

    // PointerDown 이벤트 핸들러
    public void OnPointerDown(PointerEventData eventData)
    {
        HandleUIEvent(UIEventType.PointerDown, eventData);
    }

    // PointerUp 이벤트 핸들러
    public void OnPointerUp(PointerEventData eventData)
    {
        HandleUIEvent(UIEventType.PointerUp, eventData);
    }

    // PointerClick 이벤트 핸들러
    public void OnPointerClick(PointerEventData eventData)
    {
        HandleUIEvent(UIEventType.PointerClick, eventData);
    }

    // 이벤트 처리 메소드
    private void HandleUIEvent(UIEventType eventType, PointerEventData eventData)
    {
        if (uiEventActions != null && uiEventActions.TryGetValue(eventType, out var action))
        {
            action?.Invoke(eventData);
        }
    }
}
