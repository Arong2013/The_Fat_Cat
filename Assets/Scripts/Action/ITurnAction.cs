using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnAction
{
    TurnState Execute();
}
public class MoveToPositionAction : ITurnAction
{
    private IEntity movable;   
    private Vector3 targetPosition;
    public MoveToPositionAction(IEntity movable, Vector3 targetPosition)
    {
        this.movable = movable;
        this.targetPosition = targetPosition;
    }
    public TurnState Execute()
    {
        Vector3 direction = Vector3.Normalize(targetPosition - movable.Position);
        if (Vector3.Distance(movable.Position, targetPosition) <= 0.1f)
        {
            movable.Position = targetPosition;
            return TurnState.SUCCESS;
        }
        else
        {
            movable.Position += direction * 5f;
            return TurnState.RUNNING;
        }
    }
}
