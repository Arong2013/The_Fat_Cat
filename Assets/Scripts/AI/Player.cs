using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : IEntity, ITurnStateable
{
    public Transform transform { get; set; }
    public float Radius { get; set; }
    public Vector3 Position
    {
        get => transform.position;
        set
        {
            Vector3 direction = value - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
            transform.position = value;
        }
    }
    public bool IsObstacle { get; set;}

    public ITurnAction turnAction;

    public Player(float radius, Transform _transform)
    {
        Radius = radius;
        transform = _transform;
        IsObstacle = true;
    }
    public TurnState ExecuteTurn() => turnAction.Execute();
}
