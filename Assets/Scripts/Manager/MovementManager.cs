using System.Collections;
using System.Collections.Generic;
using System.Numerics;

// 이동 가능한 객체에 대한 인터페이스 정의 (위치와 반지름 크기 포함)
public interface IMovable
{
    public Vector3 Position { get; set; }  // 객체의 위치
    public float Radius { get; set; }      // 객체의 반지름 (크기)
}

// 이동하는 객체들을 관리하는 매니저 클래스
public class MovementManager
{
    private List<IMovable> movableObjects;  // IMovable 객체들을 관리하는 리스트
    public MovementManager()
    {
        movableObjects = new List<IMovable>();
    }
    public void RegisterObject(IMovable obj)
    {
        if (!movableObjects.Contains(obj))
        {
            movableObjects.Add(obj);
        }
    }

    // IMovable 객체를 제거하는 메서드
    public void UnregisterObject(IMovable obj)
    {
        if (movableObjects.Contains(obj))
        {
            movableObjects.Remove(obj);
        }
    }

    // 두 객체가 충돌하는지 여부를 계산하는 메서드
    public bool AreObjectsColliding(IMovable obj1, IMovable obj2)
    {
        float distance = Vector3.Distance(obj1.Position, obj2.Position);
        float combinedRadius = obj1.Radius + obj2.Radius;
        return distance <= combinedRadius;
    }

    // 모든 등록된 객체들 간의 충돌을 확인하는 메서드
    public void CheckCollisions()
    {
        for (int i = 0; i < movableObjects.Count; i++)
        {
            for (int j = i + 1; j < movableObjects.Count; j++)
            {
                if (AreObjectsColliding(movableObjects[i], movableObjects[j]))
                {
                    // 충돌 처리 로직 추가
                    System.Console.WriteLine($"{movableObjects[i]} and {movableObjects[j]} are colliding!");
                }
            }
        }
    }
}
