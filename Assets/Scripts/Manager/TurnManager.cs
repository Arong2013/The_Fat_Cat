using System.Collections;
using System.Collections.Generic;

public enum TurnState
{
    SUCCESS,    // 턴이 성공적으로 완료됨
    RUNNING,    // 턴이 진행 중임
    FAILURE     // 턴이 실패함
}
// 각 턴을 담당하는 엔티티가 반드시 구현해야 하는 메서드 정의
public interface ITurnStateable
{
    public TurnState ExecuteTurn();  // 턴을 실행하고 그 결과를 반환
}

// 턴을 관리하는 매니저 클래스
public class TurnManager
{
    private List<ITurnStateable> turnEntities;  // 각 턴을 담당하는 엔티티들의 리스트
    public TurnManager()
    {
        turnEntities = new List<ITurnStateable>();
    }
    // 엔티티를 등록하는 메서드
    public void RegisterEntity(ITurnStateable entity)
    {
        turnEntities.Add(entity);
    }
    public void UpdateTurns()
    {
    }
}
