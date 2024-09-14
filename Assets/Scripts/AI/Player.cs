using System.Collections;
using System.Collections.Generic;
using System.Numerics;
public class Player : IMovable, ITurnStateable
{
    public Vector3 Position {get; set;}
    public float Radius {get; set;}
    public ITurnAction turnAction;



    public Player(Vector3 startPosition, float radius)
    {
        Position = startPosition;
        Radius = radius;
    }
    public TurnState ExecuteTurn()
    {
        return turnAction.Execute();
    }
}
