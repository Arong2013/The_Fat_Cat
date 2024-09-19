using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState
{
    SUCCESS,    // 턴이 성공적으로 완료됨
    RUNNING,    // 턴이 진행 중임
    FAILURE     // 턴이 실패함
}

// 각 턴을 담당하는 엔티티가 반드시 구현해야 하는 메서드 정의
public interface ITurnStateable
{
    TurnState CurrentTurnState { get; set; }  // 현재 턴 상태
    TurnState ExecuteTurn();                  // 턴을 실행하고 그 결과를 반환
}

// 턴을 관리하는 매니저 클래스
public class TurnManager : MonoBehaviour
{
    private List<ITurnStateable> turnEntities = new List<ITurnStateable>();  // 각 턴을 담당하는 엔티티들의 리스트

    // 엔티티를 등록하는 메서드
    public void RegisterEntity(ITurnStateable entity)
    {
        if (!turnEntities.Contains(entity))
            turnEntities.Add(entity);
    }

    // MonoBehaviour의 Start 메서드를 활용하여 초기화
    public void Init()
    {
        StartCoroutine(TurnStart());
    }
    IEnumerator TurnStart()
    {
        // 무한 루프
        while (true)
        {
            while (turnEntities.Count == 0)
            {
                yield return null;  // 엔티티가 등록될 때까지 프레임마다 대기
            }
            
            foreach (var entity in turnEntities)
            {
                if (entity.CurrentTurnState == TurnState.SUCCESS)
                    continue;  // 이미 성공한 턴은 건너뜀
                entity.ExecuteTurn();  // 턴 실행
            }
            // 다음 프레임까지 대기, 계속해서 실행됨
            yield return null;
        }
    }
}
