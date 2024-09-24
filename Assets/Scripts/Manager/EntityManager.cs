using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IEntity
{
    public Transform transform { get; set; }
    public Vector3 Position { get; set; }
    public float Radius { get; set; }
    public bool IsObstacle { get; set; }
}

public class EntityManager : MonoBehaviour
{
    private List<IEntity> entities = new List<IEntity>();

    public void RegisterEntity(IEntity entity) => entities.Add(entity);

    public void UnregisterEntity(IEntity entity) => entities.Remove(entity);

    // 두 객체가 충돌하는지 여부 계산
    bool AreEntitiesColliding(IEntity entity1, IEntity entity2) =>
        entity1.IsObstacle && entity2.IsObstacle && Vector3.Distance(entity1.Position, entity2.Position) <= (entity1.Radius + entity2.Radius);

    // 특정 IEntity가 다른 객체와 충돌하는지 확인
    // 하나의 제너릭 메서드로 통합
    public T GetCollisions<T>(IEntity entity, Func<List<IEntity>, T> collisionProcessor)
    {
        var collidingEntities = entities
            .Where(e => e != entity && AreEntitiesColliding(entity, e))  // 자신을 제외하고 충돌하는 엔티티 선택
            .ToList();  // 리스트로 변환

        return collisionProcessor(collidingEntities);  // 처리 함수에 리스트 전달
    }

    // 충돌 여부 확인
    public bool IsEntityColliding(IEntity entity)
    {
        return GetCollisions(entity, collidingEntities => collidingEntities.Any());
    }

    // 첫 번째로 충돌하는 객체 반환
    public IEntity GetFirstCollidingEntity(IEntity entity)
    {
        return GetCollisions(entity, collidingEntities => collidingEntities.FirstOrDefault());
    }

    // 충돌하는 모든 객체 리스트로 반환
    public List<IEntity> GetCollidingEntities(IEntity entity)
    {
        return GetCollisions(entity, collidingEntities => collidingEntities);
    }


    // 모든 등록된 객체들 간의 충돌을 확인하는 메서드
    public bool CheckCollisions() =>
        entities.SelectMany((entity1, i) => entities.Skip(i + 1), AreEntitiesColliding).Any(collision => collision);
}
