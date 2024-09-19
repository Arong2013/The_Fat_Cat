using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnAction
{
    TurnState Execute();
}
public class BassMoveAction : ITurnAction
{
    private IEntity movable;
    private Vector3 targetPosition;
    public BassMoveAction(IEntity movable, Vector3 targetPosition)
    {
        this.movable = movable;
        this.targetPosition = targetPosition;
    }
    public TurnState Execute()
    {
        // 목표 위치에 매우 근접한 경우
        if (Vector3.Distance(movable.transform.position, targetPosition) <= 0.1f)
        {
            movable.transform.position = targetPosition; // 정확하게 목표 위치로 이동
            return TurnState.SUCCESS;
        }
        else
        {
            float moveSpeed = 5f * Time.deltaTime; // 속도 조정
            movable.transform.position = Vector3.MoveTowards(movable.transform.position, targetPosition, moveSpeed);
            return TurnState.RUNNING;
        }
    }

}

public class RandomMoveAction : ITurnAction
{
    private IEntity movable;
    private float moveDistance;
    private Vector3[] directions = new Vector3[]
    {
        Vector3.forward,    // 위쪽
        Vector3.back,  // 아래쪽
        Vector3.left,  // 왼쪽
        Vector3.right,  // 오른쪽
        Vector3.zero
    };

    public RandomMoveAction(IEntity movable, float moveDistance = 1f)
    {
        this.movable = movable;
        this.moveDistance = moveDistance;
    }

    public TurnState Execute()
    {
        // 4방향 중 하나를 랜덤으로 선택
        Vector3 randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3 targetPosition = movable.Position + randomDirection * moveDistance;
        movable.Position = targetPosition;
        return TurnState.SUCCESS;
    }
}

