using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class IEntity
{
    public Transform transform { get; protected set; }
    public Vector3 Position { get; set; }
    public float Radius { get; protected set; }
    public abstract bool ObstacleEvent(IEntity entity);
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
