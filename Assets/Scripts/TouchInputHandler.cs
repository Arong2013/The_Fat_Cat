using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public ITouchable touchable;
    private void Start()
    {
        // TouchableMovementHandler의 인스턴스를 생성하여 touchable에 할당
        touchable = new TouchableMovementHandler();
    }
    // IPointerDownHandler 인터페이스 구현: 터치 이벤트 처리
    public void OnPointerDown(PointerEventData eventData)
    {
        HandleTouchAction<IPointerDownHandler>(eventData);
    }

    // IDragHandler 인터페이스 구현: 드래그 이벤트 처리
    public void OnDrag(PointerEventData eventData)
    {
        HandleTouchAction<IDragHandler>(eventData);
    }

    // IPointerUpHandler 인터페이스 구현: 터치가 끝날 때의 이벤트 처리
    public void OnPointerUp(PointerEventData eventData)
    {
        HandleTouchAction<IPointerUpHandler>(eventData);
    }

    // 제네릭 메서드: 이벤트 타입에 따라 적절한 액션을 호출하는 메서드
    private void HandleTouchAction<T>(PointerEventData eventData) where T : IEventSystemHandler
    {
        var handlerType = typeof(T);
        if (touchable.touchActions.ContainsKey(handlerType))
        {
            touchable.touchActions[handlerType]?.Invoke(eventData);
        }
    }
}
