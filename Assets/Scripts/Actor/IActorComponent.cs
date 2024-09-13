using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Sirenix.Serialization;
using UnityEditor.Animations;
public abstract class IActorComponent
{
    public NodeState nodeState;
}

public interface IMoveable { void Move(Rigidbody rigidbody, float speed, Vector3 _direction); }
public class SimpleMove : IMoveable
{
    Animator animator;
    string currentState;
    public void Move(Rigidbody rigidbody, float speed, Vector3 direction)
    {
        animator ??= rigidbody.GetComponent<Animator>(); // Null-coalescing assignment
        float directionMagnitude = direction.magnitude;
        float horizontalMagnitude = new Vector3(direction.x, 0, direction.z).magnitude;

        string targetAnimation = directionMagnitude == 0 ? "Idle" : (directionMagnitude < 0.5f ? "Walk" : "Sprint");
        float baseSpeed = directionMagnitude < 0.5f ? 5f : 10f;
        float speedAdjustmentFactor = 1f - Mathf.Clamp(horizontalMagnitude / directionMagnitude, 0f, 0.5f);
        float speeds = directionMagnitude == 0 ? 0 : baseSpeed * speedAdjustmentFactor;

        if (float.IsNaN(speeds)) speeds = 0f;

        MovementMethod.SimpleMove(rigidbody, direction, speeds);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(targetAnimation) == false && currentState != targetAnimation)
        {
            currentState = targetAnimation;
            animator.CrossFade(targetAnimation, 0.1f);
        }
    }
}
public class CombatComponent : IActorComponent
{
    public CombatStats combatStats = new CombatStats();
    public void TakeDamage(CombatStats _combatStats)
    {
        var damage = Mathf.Max(0, _combatStats.attack.Value - combatStats.defense.Value);
        combatStats.cunHp.AddModifier(new StatModifier(-damage, StatModType.Flat));
    }
}

public class SurvivalComponent : IActorComponent
{
    public SurvivalStats survivalStats = new SurvivalStats();
}