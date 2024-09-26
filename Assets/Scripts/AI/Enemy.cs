using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IEntity, ITurnStateable, ICombatable
{
    public TurnState CurrentTurnState { get; set; }
    public Animator animator { get; set; }

    public ITurnAction turnAction;
    public Enemy(float radius, Transform _transform)
    {
        Radius = radius;
        transform = _transform;
        IsObstacle = true;
        Position = _transform.position;
        animator = transform.GetComponent<Animator>();
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

    public void TakeDamage(ICombatable target)
    {
        throw new System.NotImplementedException();
    }
}
