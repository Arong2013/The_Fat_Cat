
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory System/Item Data", order = 3)]
public class ItemData : SerializedScriptableObject
{
    // 인스펙터에서 ID를 편집 가능하게 표시, 그룹화
    [FoldoutGroup("General Info"), OdinSerialize, LabelText("Item ID"), Tooltip("The unique identifier for the item.")]
    public readonly int ID;

    // 인스펙터에서 Name을 편집 가능하게 표시, 그룹화
    [FoldoutGroup("General Info"), OdinSerialize, LabelText("Item Name"), Tooltip("The name of the item.")]
    public readonly string Name;

    // 인스펙터에서 Tooltip을 편집 가능하게 표시, 그룹화
    [FoldoutGroup("General Info"), OdinSerialize, MultiLineProperty, LabelText("Item Tooltip"), Tooltip("The tooltip description of the item.")]
    public readonly string Tooltip;

    // 인스펙터에서 IconSprite를 편집 가능하게 표시, 그룹화
    [FoldoutGroup("Visuals"), OdinSerialize, LabelText("Icon"), Tooltip("The icon representing the item.")]
    public readonly Sprite IconSprite;

    // 인스펙터에서 mesh를 편집 가능하게 표시, 그룹화
    [FoldoutGroup("Visuals"), OdinSerialize, LabelText("Mesh"), Tooltip("The 3D mesh of the item.")]
    public readonly Mesh mesh;

    // 인스펙터에서 material을 편집 가능하게 표시, 그룹화
    [FoldoutGroup("Visuals"), OdinSerialize, LabelText("Material"), Tooltip("The material applied to the item's mesh.")]
    public readonly Material material;
    [OdinSerialize] public Item  item;
}
