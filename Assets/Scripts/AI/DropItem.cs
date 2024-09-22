using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : IEntity, ITurnStateable
{
    public Transform transform { get; set; }
    public Vector3 Position { get; set; }
    public TurnState CurrentTurnState { get; set; }
    public ITurnAction turnAction;


    public float Radius { get; set; }
    public bool IsObstacle { get; set; }

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
