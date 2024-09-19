using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    ITouchableAction touchable;

    public void SetTouchAction(ITouchableAction touchableAction)
    {
        touchable = touchableAction;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        HandleTouchAction<IPointerDownHandler>(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        HandleTouchAction<IDragHandler>(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        HandleTouchAction<IPointerUpHandler>(eventData);
    }
    private void HandleTouchAction<T>(PointerEventData eventData) where T : IEventSystemHandler
    {
        var handlerType = typeof(T);
        if (touchable.touchActions.ContainsKey(handlerType))
        {
            touchable.touchActions[handlerType]?.Invoke(eventData);
        }
    }
}
