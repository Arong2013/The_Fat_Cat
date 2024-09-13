using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public static class InteractionMethod
{
    public static void PickUpItem(Transform _transform)
    {

    }
}

public static class AnimatorMethod
{
    public static void SetAnimator()
    {

    }
    public static NodeState IsAnimationPlaying(Animator animator, string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            return NodeState.RUNNING;
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && animator.GetFloat("Walk") == 0)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }

    public static void PlayAnimation()
    {

    }
}

public static class CombatMethod
{
    public static void MeleeAttack(MeshCollider boxCollider, string animeName)
    {
        var animator = boxCollider.transform.root.GetComponent<Animator>();
        animator.Play(animeName);
        MonoBehaviour monoBehaviour = boxCollider.gameObject.GetComponent<MonoBehaviour>();
        monoBehaviour.StartCoroutine(EnableColliderAtHalfTime());

        IEnumerator EnableColliderAtHalfTime()
        {
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationLength / 2);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animeName))
                boxCollider.enabled = true;
            while (animator.GetCurrentAnimatorStateInfo(0).IsName(animeName))
                yield return null;
            boxCollider.enabled = false;
        }
    }
    public static void TakeDamaged(BoxCollider boxCollider, Animator animator)
    {
        animator.Play("TakeDamage");
        MonoBehaviour monoBehaviour = boxCollider.gameObject.GetComponent<MonoBehaviour>();
        monoBehaviour.StartCoroutine(DisableColliderDuringAnimation());

        IEnumerator DisableColliderDuringAnimation()
        {
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            boxCollider.enabled = false;
            yield return new WaitForSeconds(animationLength);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
            {
                boxCollider.enabled = true;
            }
        }
    }


    public static void EquipmentWeapon(GameObject weaponPrefab, Transform characterTransform)
    {
        if (characterTransform != null)
        {
            GameObject weapon = GameObject.Instantiate(weaponPrefab, characterTransform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("Finger_01 not found in the provided transform.");
        }
    }

    
     public static void ContectDMG(Collider collider,CombatStats combatStats)
    {
    }

}


public static class MovementMethod
{
    public static NodeState SimpleMove(Rigidbody _rb, Vector3 _direction, float _speed)
    {
        if (_direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * _speed);
        }
        _rb.velocity = _direction * _speed;
        return NodeState.SUCCESS;
    }

    public static NodeState CurvedMove(Rigidbody _rb, Vector3 _startPosition, Vector3 _controlPoint, Vector3 _endPosition, float _speed)
    {
        float t = Time.time * _speed;
        Vector3 newPosition = Mathf.Pow(1 - t, 2) * _startPosition +
                              2 * (1 - t) * t * _controlPoint +
                              Mathf.Pow(t, 2) * _endPosition;

        Vector3 moveDirection = newPosition - _rb.position;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * _speed);
        }

        _rb.MovePosition(newPosition);

        if (t < 1.0f)
        {
            return NodeState.RUNNING;
        }
        return NodeState.SUCCESS;
    }

    public static NodeState Teleport(Rigidbody _rb, Vector3 _targetPosition)
    {
        Vector3 moveDirection = _targetPosition - _rb.position;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rb.rotation = targetRotation;
        }

        _rb.position = _targetPosition;
        _rb.velocity = Vector3.zero;
        return NodeState.SUCCESS;
    }

    public static NodeState Jump(Rigidbody _rb, float _force)
    {
        if (_rb != null)
        {
            _rb.AddForce(Vector3.up * _force, ForceMode.Impulse);
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }

    public static NodeState OrbitMove(Rigidbody _rb, Vector3 _centerPosition, float _radius, float _speed)
    {
        float angle = _speed * Time.time;
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * _radius;
        Vector3 newPosition = _centerPosition + offset;

        Vector3 moveDirection = newPosition - _rb.position;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * _speed);
        }

        _rb.MovePosition(newPosition);
        return NodeState.RUNNING;
    }

    public static NodeState Flee(Rigidbody _rb, Vector3 _dangerPosition, float _speed)
    {
        Vector3 fleeDirection = (_rb.position - _dangerPosition).normalized;

        if (fleeDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(fleeDirection);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * _speed);
        }

        _rb.velocity = fleeDirection * _speed;
        return NodeState.RUNNING;
    }
}
