using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ITouchable
{
    // 여러 핸들러와 그에 따른 액션을 저장할 수 있는 딕셔너리
    Dictionary<Type, Action<PointerEventData>> touchActions { get; set; }
}
public class TouchableMovementHandler : ITouchable
{
    // ITouchable 인터페이스 구현: 핸들러 타입과 액션을 저장하는 딕셔너리
    public Dictionary<Type, Action<PointerEventData>> touchActions { get; set; }
    private IEntity movableObject;
    public TouchableMovementHandler()
    {
        // 딕셔너리 초기화 및 IPointerDownHandler 이벤트 추가
        touchActions = new Dictionary<Type, Action<PointerEventData>>();
        touchActions.Add(typeof(IPointerDownHandler), OnPointerDown);
    }

    // ITouchable 인터페이스 구현: 터치 입력 처리 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 touchPosition = eventData.position;
        Vector3 moveDirection = GetMoveDirection(touchPosition);
        Debug.Log($"Moving in direction: {moveDirection}");
    }

    // 터치 위치에 따른 방향을 결정하는 메서드
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
