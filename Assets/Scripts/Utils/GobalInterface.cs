using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEditor.Animations;
public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

public interface IPickupable
{
    void PickUp();
}
public interface IAnimatable
{
    void SetAnimator(AnimatorController animatorController, bool isSet = false);
    NodeState IsAnimationPlaying(string _animeName);
    void PlayAnimation(string _animeName, object key = null);
}
public interface StatComponent
{
    void RemoveModifiersFromSource(object source);
}
