using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : IEntity,ICombatable, ITurnStateable
{
    public TurnState CurrentTurnState { get; set; }
    public ITurnAction turnAction; 
    public Animator animator { get; set; }

    public Player(float radius, Transform _transform)
    {
        Radius = radius;
        transform = _transform;
        IsObstacle = true;
        Position = _transform.position;
        CurrentTurnState = TurnState.FAILURE;
        animator = transform.GetComponent<Animator>();

        Utils.GetUI<TouchInputHandler>().SetTouchAction(new TouchableMovementHandler(this, SetTurnAction));
    }

    public void SetTurnAction(ITurnAction _turnAction) { turnAction = _turnAction; }

    public TurnState ExecuteTurn()
    {
        if (turnAction == null)
            return TurnState.FAILURE;
        CurrentTurnState = turnAction.Execute();

        if (CurrentTurnState == TurnState.SUCCESS)
            turnAction = null;
        return CurrentTurnState;
    }

    public void TakeDamage(ICombatable target)
    {
        throw new System.NotImplementedException();
    }
}
