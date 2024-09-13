using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Sirenix.Serialization;
using UnityEditor.Animations;
using DungeonArchitect.Editors.Visualization.Implementation;


public abstract class Actor
{
    public Dictionary<LifecycleEventType, Action> lifecycleEventActions = new Dictionary<LifecycleEventType, Action>();
    public Dictionary<PhysicsEventType, Action<Collider>> triggerEventActions = new Dictionary<PhysicsEventType, Action<Collider>>();
    public Dictionary<PhysicsEventType, Action<Collision>> collisionEventActions = new Dictionary<PhysicsEventType, Action<Collision>>();
    public abstract void Init(Transform _transform);
}
public class Player : Actor
{
    Rigidbody RB;
    Joystick joystick;
    Animator animator;
    GameObject WeaponObj;
    public Transform weaponTransform;


    IMoveable moveable;
    public CombatStats combatStats = new CombatStats();
    public SurvivalStats survivalStats = new SurvivalStats();
    public Inventory inventory = new Inventory();

    public override void Init(Transform _transform)
    {
        GameManager.Instance.AddPlayer(this);
        RB = _transform.GetComponent<Rigidbody>();
        joystick = Utils.GetUI<Joystick>();
        animator = _transform.GetComponent<Animator>();

        lifecycleEventActions.Add(LifecycleEventType.Update, Update);

        moveable = new SimpleMove();
        survivalStats = new SurvivalStats();
        inventory = new Inventory();
    }
    public void Update()
    {
        moveable.Move(RB, 10f, new Vector3(joystick.Horizontal, 0, joystick.Vertical));
        interactive();
    }
    public void EquipmentWeapon(EquipmentItem equipmentItem)
    {
        if (WeaponObj != null)
            MonoBehaviour.Destroy(WeaponObj.gameObject);
        if (equipmentItem != null)
        {
            var gameObject = Utils.FindGameObjectField(equipmentItem.EquipmentData);
            Debug.Log(gameObject.name );
            WeaponObj = GameObject.Instantiate(Utils.FindGameObjectField(equipmentItem.EquipmentData), weaponTransform);
            Utils.GetUI<InteractionSlot>("CombatSlot").SetAction(equipmentItem.Data.IconSprite, () => { CombatMethod.MeleeAttack(WeaponObj.transform.GetComponent<MeshCollider>(), "MeleeAttack"); });
            WeaponObj.GetComponent<ActorObject>().actor.triggerEventActions.Add(PhysicsEventType.OnTriggerEnter, (Collider) => { CombatMethod.ContectDMG(Collider, combatStats); });
        }
    }

    void interactive()
    {
        Iinteractive iinteractive = null;
        var closestDistance = float.MaxValue;  // 매 프레임마다 초기화
        foreach (var actorObject in GameObject.FindObjectsOfType<ActorObject>())
        {
            float distance = Vector3.Distance(RB.transform.position, actorObject.transform.position);

            if (distance < closestDistance && distance < 2)
            {
                if (actorObject.actor is Iinteractive interactiveActor)
                {
                    closestDistance = distance;
                    iinteractive = interactiveActor;

                }
            }
        }
        if (iinteractive != null)
        {
            Debug.Log("상호작용");
            iinteractive.Interactive(RB.transform);
        }
    }
}
public class EmptyActor : Actor
{
    public override void Init(Transform _transform)
    {

    }
}