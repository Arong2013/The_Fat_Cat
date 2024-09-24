using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICombatable
{
    Animator animator { get; set; }
    public void TakeDamage(ICombatable target);
}
public class BassAttackAction : ITurnAction
{
    ICombatable my, target;

    public BassAttackAction(ICombatable _combatable1, ICombatable _combatable2)
    {
        my = _combatable1;
        target = _combatable2;
    }
    public TurnState Execute()
    {
        my.animator.Play("Attack");
        target.animator.Play("Hit");

        if (my.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return TurnState.RUNNING;

            
        return TurnState.SUCCESS;
    }
}
