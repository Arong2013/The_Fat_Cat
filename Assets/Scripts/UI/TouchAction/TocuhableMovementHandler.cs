using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public TouchableMovementHandler(IEntity entity, Action<ITurnAction> _turnAction)
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
        setTurnAction(new TargetMoveAction(ientity, ientity.Position + moveDirection));
        Debug.Log($"Moving in direction: {moveDirection}");
    }
    Vector3 GetMoveDirection(Vector2 touchPosition)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 화면 상단 또는 하단을 터치했을 때
        if (touchPosition.y > screenHeight * 0.65f)  // 상단 25% 부분
        {
            return new Vector3(1, 0, 1);  // 위로 대각선 이동
        }
        else if (touchPosition.y < screenHeight * 0.35f)  // 하단 25% 부분
        {
            return new Vector3(-1, 0, -1);  // 아래로 대각선 이동
        }
        // 중앙을 기준으로 좌우를 나눔
        else if (touchPosition.x < screenWidth / 2)  // 중앙 왼쪽
        {
            return new Vector3(-1, 0, 1);  // 왼쪽으로 이동
        }
        else  // 중앙 오른쪽
        {
            return new Vector3(1, 0, -1);  // 오른쪽으로 이동
        }
    }




}
