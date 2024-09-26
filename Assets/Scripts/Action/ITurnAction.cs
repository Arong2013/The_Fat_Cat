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

    private Animator animator;

    public MoveActionBase(IEntity entity)
    {
        this.entity = entity;
        animator = entity.transform.GetComponent<Animator>(); // Animator 캐싱
    }

    // 공통된 이동 로직
    protected TurnState MoveToTarget()
    {
        float distanceToTarget = Vector3.Distance(entity.transform.position, targetPosition);

        if (distanceToTarget <= 0.1f)
        {
            entity.transform.position = targetPosition;
            animator.CrossFade("Idle_A", 0.2f);
            return TurnState.SUCCESS;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            animator.Play("Walk");

        Vector3 direction = (targetPosition - entity.transform.position).normalized;
        entity.transform.rotation = Quaternion.LookRotation(direction);
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
        return MoveToTarget(); // 공통 이동 로직 사용
    }
}

// RandomMoveAction: 랜덤하게 이동하는 액션
public class RandomMoveAction : MoveActionBase
{
    private Vector3[] directions = new Vector3[]
    {
        new Vector3(0, 0, 1),   // 위쪽
        new Vector3(0, 0, -1),  // 아래쪽
        new Vector3(-1, 0, 0),  // 왼쪽
        new Vector3(1, 0, 0),   // 오른쪽
        Vector3.zero            // 제자리
    };

    public RandomMoveAction(IEntity entity) : base(entity)
    {
        // 초기 이동 목표 위치 설정
        SetRandomTarget();
    }

    public override TurnState Execute()
    {
        TurnState state = MoveToTarget();
        if (state == TurnState.SUCCESS)
        {
            SetRandomTarget(); // 성공 시 새로운 랜덤 목표 설정
        }
        return state;
    }

    private void SetRandomTarget()
    {
        Vector3 randomDirection = directions[Random.Range(0, directions.Length)];
        targetPosition = entity.Position + randomDirection;
    }
}
