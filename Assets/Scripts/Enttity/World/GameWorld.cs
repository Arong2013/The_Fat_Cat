using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 내 모든 것을 관리하는 클래스, 한 턴이 진행될 때 월드의 시간도 흐름
public class GameWorld : MonoBehaviour
{
    private Field gameField;

    public void Initialize()
    {

    }
    private void CreateWorld()
    {

    }
    IEnumerator StartTurnSequence()
    {
        while (true)
        {
            while (gameField.ExecuteTurn() != TurnState.SUCCESS)
                yield return null;
            yield return null;
        }
    }
}
