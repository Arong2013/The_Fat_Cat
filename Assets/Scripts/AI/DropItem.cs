using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : IEntity, ITurnStateable
{
    public TurnState CurrentTurnState { get; set; }
    public ITurnAction turnAction;

    public DropItem(float radius, Transform _transform)
    {
        Radius = radius;
        transform = _transform;
        IsObstacle = true;
        Position = _transform.position;
        CurrentTurnState = TurnState.FAILURE;
    }

    public TurnState ExecuteTurn()
    {
        if (turnAction == null)
            return TurnState.FAILURE;
        CurrentTurnState = turnAction.Execute();
        if (CurrentTurnState == TurnState.SUCCESS)
            turnAction = null;
        return CurrentTurnState;
    }
}
