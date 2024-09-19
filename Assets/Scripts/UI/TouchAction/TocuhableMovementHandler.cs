using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ITouchableAction
{
    // 여러 핸들러와 그에 따른 액션을 저장할 수 있는 딕셔너리
    Dictionary<Type, Action<PointerEventData>> touchActions { get; set; }
}
public class TouchableMovementHandler : ITouchableAction
{
    // ITouchable 인터페이스 구현: 핸들러 타입과 액션을 저장하는 딕셔너리
    public Dictionary<Type, Action<PointerEventData>> touchActions { get; set; }
    private IEntity ientity;
    Action<ITurnAction> setTurnAction;
    public TouchableMovementHandler(IEntity entity,Action<ITurnAction>  _turnAction)
    {
        ientity = entity;
        setTurnAction = _turnAction;
        touchActions = new Dictionary<Type, Action<PointerEventData>>()
        {
            { typeof(IPointerDownHandler), OnPointerDown },
        };
    }

    // ITouchable 인터페이스 구현: 터치 입력 처리 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 touchPosition = eventData.position;
        Vector3 moveDirection = GetMoveDirection(touchPosition);
        setTurnAction(new BassMoveAction(ientity,ientity.Position + moveDirection));
        Debug.Log($"Moving in direction: {moveDirection}");
    }
    Vector3 GetMoveDirection(Vector2 touchPosition)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (touchPosition.x < screenWidth / 2 && touchPosition.y > screenHeight / 2)
        {
            return Vector3.forward;  // 위로 이동
        }
        else if (touchPosition.x < screenWidth / 2 && touchPosition.y < screenHeight / 2)
        {
            return Vector3.left;  // 왼쪽으로 이동
        }
        else if (touchPosition.x > screenWidth / 2 && touchPosition.y > screenHeight / 2)
        {
            return Vector3.right;  // 오른쪽으로 이동
        }
        else if (touchPosition.x > screenWidth / 2 && touchPosition.y < screenHeight / 2)
        {
            return Vector3.back;  // 아래로 이동
        }

        return Vector3.zero;  // 터치가 감지되지 않은 경우
    }
}
