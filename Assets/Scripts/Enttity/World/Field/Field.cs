using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//게임에서 땅에 붙어있는 모든것 
public class Field : MonoBehaviour, ITurnStateable
{
    List<ITurnStateable> TurnStateablelist;
    [SerializeField] GameObject PlayerPre, EnemyPre;
    GameWorld parentWorld;
    public TurnState CurrentTurnState { get; set; }

    public TurnState ExecuteTurn()
    {
        if (TurnStateablelist.Count == 0)
            return TurnState.SUCCESS;

        TurnStateablelist.ForEach(entity => { if (entity.CurrentTurnState != TurnState.SUCCESS) entity.ExecuteTurn(); });

        if (TurnStateablelist.All(x => x.CurrentTurnState == TurnState.SUCCESS))
        {
            TurnStateablelist.ForEach(entity => entity.CurrentTurnState = TurnState.FAILURE);
            return TurnState.SUCCESS;
        }
        else
        {
            return TurnState.RUNNING;
        }
    }

    public void MakeField(GameWorld world)
    {
        parentWorld = world;
    }
}
