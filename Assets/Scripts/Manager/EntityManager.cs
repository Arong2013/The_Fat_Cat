using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class IEntity
{
    public Transform transform { get; protected set; }
    public Vector3 Position { get; set; }
    public float Radius { get; protected set; }
    public bool IsObstacle { get; protected set; } // 이 속성을 사용한 충돌 계산이 필요할 때 사용
}

public interface ICombatable
{
    Animator animator { get; set; }
    public void TakeDamage(ICombatable target);
}

public class EntityManager : MonoBehaviour
{
    private List<IEntity> entities = new List<IEntity>();

    public void RegisterEntity(IEntity entity) => entities.Add(entity);
    public void UnregisterEntity(IEntity entity) => entities.Remove(entity);

    // 두 객체가 충돌하는지 여부 계산 (IsObstacle 무시)
    bool AreEntitiesColliding(IEntity entity1, IEntity entity2) =>
        Vector3.Distance(entity1.Position, entity2.Position) <= (entity1.Radius + entity2.Radius);

    // 두 객체가 충돌하는지 여부 계산 (IsObstacle 고려)
    bool AreEntitiesCollidingConsideringObstacle(IEntity entity1, IEntity entity2) =>
        entity1.IsObstacle && entity2.IsObstacle && Vector3.Distance(entity1.Position, entity2.Position) <= (entity1.Radius + entity2.Radius);

    // 특정 IEntity가 다른 객체와 충돌하는지 확인 (IsObstacle 고려)
    public bool IsEntityCollidingWithObstacle(IEntity entity)
    {
        return entities.Any(e => e != entity && AreEntitiesCollidingConsideringObstacle(entity, e));
    }

    // 특정 IEntity가 다른 객체와 충돌하는지 확인 (IsObstacle 무시)
    T GetCollisions<T>(IEntity entity, Func<List<IEntity>, T> collisionProcessor)
    {
        var collidingEntities = entities
            .Where(e => e != entity && AreEntitiesColliding(entity, e))  // 자신을 제외하고 충돌하는 엔티티 선택
            .ToList();  // 리스트로 변환

        return collisionProcessor(collidingEntities);  // 처리 함수에 리스트 전달
    }
    public bool IsEntityColliding(IEntity entity)
    {
        return GetCollisions(entity, collidingEntities => collidingEntities.Any());
    }
    public IEntity GetFirstCollidingEntity(IEntity entity)
    {
        return GetCollisions(entity, collidingEntities => collidingEntities.FirstOrDefault());
    }
    public T GetInterfaceOfType<T>(IEntity entity) where T : class
    {
        var target = GetFirstCollidingEntity(entity);
        if (target != null)
        {
            if (target is T tClass)
            {
                return tClass;
            }
        }
        return null;  // 없으면 null 반환
    }
}
