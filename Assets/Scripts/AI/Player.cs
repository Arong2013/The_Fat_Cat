using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : IEntity, ITurnStateable
{
    public Transform transform { get; set; }
    public Vector3 Position { get; set; }
    public TurnState CurrentTurnState { get; set; }
    public ITurnAction turnAction;


    public float Radius { get; set; }
    public bool IsObstacle { get; set; }

    public Player(float radius, Transform _transform)
    {
        Radius = radius;
        transform = _transform;
        IsObstacle = true;
        Position = _transform.position;
        CurrentTurnState = TurnState.FAILURE;

        
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
}
