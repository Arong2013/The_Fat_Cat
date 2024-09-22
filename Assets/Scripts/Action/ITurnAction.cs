using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ITurnAction 인터페이스 정의
public interface ITurnAction
{
    TurnState Execute();
}
public abstract class MoveActionBase : ITurnAction
{
    protected IEntity entity;
    protected Vector3 targetPosition;

    public MoveActionBase(IEntity entity) => this.entity = entity;

    // 공통된 이동 로직
    protected TurnState MoveToTarget()
    {
        entity.Position = targetPosition;

        if (Vector3.Distance(entity.transform.position, targetPosition) <= 0.1f)
        {
            entity.transform.position = targetPosition;
            return TurnState.SUCCESS;
        }
        // 카메라 45도 기준으로 고정된 회전값 설정
        Vector3 moveDirection = (targetPosition - entity.transform.position).normalized;

        // 카메라의 방향에 맞는 회전 방향 계산 (카메라 기준으로 좌우 이동)
        Quaternion targetRotation;

        // X축과 Z축 중 어느 축으로 이동했는지 판단
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
        {
            // 좌우 회전 처리
            if (moveDirection.x > 0)
                targetRotation = Quaternion.Euler(0, 45, 0);  // 오른쪽으로 이동 (카메라 기준으로 오른쪽)
            else
                targetRotation = Quaternion.Euler(0, 225, 0); // 왼쪽으로 이동 (카메라 기준으로 왼쪽)
        }
        else
        {
            // 상하 회전 처리
            if (moveDirection.z > 0)
                targetRotation = Quaternion.Euler(0, 315, 0); // 위쪽으로 이동 (카메라 기준으로 위쪽)
            else
                targetRotation = Quaternion.Euler(0, 135, 0); // 아래쪽으로 이동 (카메라 기준으로 아래쪽)
        }

        // 회전 적용
        entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, targetRotation, 5f * Time.deltaTime);

        entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, targetRotation, 10f * Time.deltaTime);

        // 이동 처리 (대각선 이동 가능)
        entity.transform.position = Vector3.MoveTowards(entity.transform.position, targetPosition, 5f * Time.deltaTime);
        return TurnState.RUNNING;
    }

    public abstract TurnState Execute();
}

// TargetMoveAction: 특정 목표 위치로 이동하는 액션
public class TargetMoveAction : MoveActionBase
{
    public TargetMoveAction(IEntity entity, Vector3 targetPosition) : base(entity)
    {
        this.targetPosition = targetPosition;
        var previousPosition = entity.Position;

        entity.Position = targetPosition;
        if (GameManager.Instance.entityManager.IsEntityColliding(entity))
        {
            entity.Position = previousPosition; // 충돌 시 원래 위치로 복원
        }
    }

    public override TurnState Execute()
    {
        var turnstate = TurnState.FAILURE;
        if (entity.Position == targetPosition)
            turnstate = MoveToTarget();
        return turnstate; // 공통 이동 로직 사용
    }
}

// RandomMoveAction: 랜덤하게 이동하는 액션
public class RandomMoveAction : MoveActionBase
{
    private Vector3[] directions = new Vector3[]
    {
        new Vector3(1, 0, 1),   // 대각선 위쪽
        new Vector3(-1, 0, -1), // 대각선 아래쪽
        new Vector3(-1, 0, 1),  // 대각선 왼쪽
        new Vector3(1, 0, -1),  // 대각선 오른쪽
        Vector3.zero            // 제자리
    };

    public RandomMoveAction(IEntity entity) : base(entity) { this.targetPosition = entity.Position + directions[Random.Range(0, directions.Length)]; }

    public override TurnState Execute()
    {
        var isMove = MoveToTarget();
        if (isMove == TurnState.SUCCESS)
        {
            Vector3 randomDirection = directions[Random.Range(0, directions.Length)];
            targetPosition = entity.Position + randomDirection;
            return isMove;
        }
        return isMove;
    }
}
