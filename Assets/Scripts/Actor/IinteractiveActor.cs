using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Sirenix.Serialization;
using UnityEditor.Animations;
public interface Iinteractive
{
    void Interactive(Transform transform);
}
public class DropItem : Actor, Iinteractive
{
    [SerializeField] ItemData data;
    [OdinSerialize] [SerializeField] Item item;
    Transform Transform;
    public override void Init(Transform _transform)
    {
        Transform = _transform;
        item =  data.item.Clone();
        _transform.GetComponent<MeshFilter>().mesh = data.mesh;
        _transform.GetComponent<MeshRenderer>().material = data.material; 
    }
    public void Interactive(Transform transform)
    {
        if (transform.TryGetComponent<ActorObject>(out ActorObject component) && component.actor is Player chars)
        {
            Utils.GetUI<InteractionSlot>("InteractiveSlot").SetAction(item.Data.IconSprite, () => { PickUP(chars.inventory); });
        }
    }
    void PickUP(Inventory inventory)
    {
        if (inventory.AddItem(item))
        {
            Transform.Destroy(Transform.gameObject);
        }
    }
}
public class Box : Actor
{
    [SerializeField] List<Item> items;
    public Box(List<Item> items)
    {
        this.items = items;
    }
    public override void Init(Transform _transform)
    {

    }
}
