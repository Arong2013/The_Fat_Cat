using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    public Vector3 Position { get; set; }
    public float Radius { get; set; } // 객체의 반지름 (크기)
    public bool IsObstacle{get;set;}
}

// 엔티티들을 관리하는 매니저 클래스
public class EntityManager
{
    private List<IEntity> entities; // IEntity 객체들을 관리하는 리스트

    public EntityManager()
    {
        entities = new List<IEntity>();
    }

    public void RegisterEntity(IEntity entity)
    {
        if (!entities.Contains(entity))
        {
            entities.Add(entity);
        }
    }

    // IEntity 객체를 제거하는 메서드
    public void UnregisterEntity(IEntity entity)
    {
        if (entities.Contains(entity))
        {
            entities.Remove(entity);
        }
    }

    // 두 객체가 충돌하는지 여부를 계산하는 메서드
    bool AreEntitiesColliding(IEntity entity1, IEntity entity2)
    {
        float distance = Vector3.Distance(entity1.Position, entity2.Position);
        float combinedRadius = entity1.Radius + entity2.Radius;
        return distance <= combinedRadius;
    }

    // 모든 등록된 객체들 간의 충돌을 확인하는 메서드
    public bool CheckCollisions()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            for (int j = i + 1; j < entities.Count; j++)
            {
                if (AreEntitiesColliding(entities[i], entities[j]))
                {
                    return true; // 충돌 발생
                }
            }
        }
        return false; // 충돌 없음
    }
}
