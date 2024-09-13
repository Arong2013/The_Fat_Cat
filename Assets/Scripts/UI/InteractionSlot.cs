using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InteractionSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image Icon;
    Action Action;
    public void SetAction(Sprite sprite,Action action) => (Icon.sprite, Action) = (sprite, action);

    public void OnPointerDown(PointerEventData eventData)
    {
        Action?.Invoke();
    }
}
