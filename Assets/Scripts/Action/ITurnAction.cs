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

    Animator animator;

    public MoveActionBase(IEntity entity)
    {
        this.entity = entity;
        animator = entity.transform.GetComponent<Animator>();
    }

    // 공통된 이동 로직
    protected TurnState MoveToTarget()
    {
        entity.Position = targetPosition;

        if (Vector3.Distance(entity.transform.position, targetPosition) <= 0.1f)
        {
            entity.transform.position = targetPosition;
            animator.CrossFade("Idle_A", 0.2f);
            return TurnState.SUCCESS;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            animator.Play("Walk");
        entity.transform.rotation = Quaternion.LookRotation((targetPosition - entity.transform.position).normalized);
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
