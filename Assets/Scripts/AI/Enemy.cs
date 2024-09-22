using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IEntity, ITurnStateable
{
    public Transform transform { get; set; }
    public Vector3 Position { get; set; }
    public TurnState CurrentTurnState { get; set; }
    public ITurnAction turnAction;
    public float Radius { get; set; }
    public bool IsObstacle { get; set; }
    public Enemy(float radius, Transform _transform)
    {
        Radius = radius;
        transform = _transform;
        IsObstacle = true;
        Position = _transform.position;
        CurrentTurnState = TurnState.FAILURE;

        turnAction = new RandomMoveAction(this);
    }
    public TurnState ExecuteTurn()
    {
        if (turnAction == null)
        {
            Debug.Log("아아");
            return TurnState.FAILURE;
        }
        CurrentTurnState = turnAction.Execute();
        return CurrentTurnState;
    }
}
