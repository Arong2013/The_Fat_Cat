using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


// Unity의 라이프사이클 이벤트를 Enum으로 정의
public enum LifecycleEventType
{
    Awake,
    OnEnable,
    Start,
    Update,       // Update 이벤트 추가
    OnDisable,
    OnDestroy,
    // 필요한 경우 추가 가능
}


public enum PhysicsEventType
{
    OnTriggerEnter,
    OnTriggerExit,
    OnCollisionEnter,
    OnCollisionExit,
    OnCollisionStay,
    OnTriggerStay,
    // 필요한 만큼 추가 가능
}

public class ActorObject : SerializedMonoBehaviour
{
    [OdinSerialize]
    public Actor actor;
    private void Start()
    {
        actor.Init(transform);
    }
    private void Update()
    {
        actor.lifecycleEventActions.GetValueOrDefault(LifecycleEventType.Update)?.Invoke();
    }
    private void HandleEvent(PhysicsEventType eventType, object data)
    {
        if (actor == null)
        {
            Debug.LogWarning("Actor is not initialized.");
            return;
        }
        switch (data)
        {
            case Collider collider when actor.triggerEventActions != null && actor.triggerEventActions.TryGetValue(eventType, out var triggerAction):
                triggerAction?.Invoke(collider);
                break;
            case Collision collision when actor.collisionEventActions != null && actor.collisionEventActions.TryGetValue(eventType, out var collisionAction):
                collisionAction?.Invoke(collision);
                break;
        }
    }
    private void OnTriggerEnter(Collider other) => HandleEvent(PhysicsEventType.OnTriggerEnter, other);
    private void OnTriggerExit(Collider other) => HandleEvent(PhysicsEventType.OnTriggerExit, other);
    private void OnTriggerStay(Collider other) => HandleEvent(PhysicsEventType.OnTriggerStay, other);
    private void OnCollisionEnter(Collision collision) => HandleEvent(PhysicsEventType.OnCollisionEnter, collision);
    private void OnCollisionExit(Collision collision) => HandleEvent(PhysicsEventType.OnCollisionExit, collision);
    private void OnCollisionStay(Collision collision) => HandleEvent(PhysicsEventType.OnCollisionStay, collision);
}
