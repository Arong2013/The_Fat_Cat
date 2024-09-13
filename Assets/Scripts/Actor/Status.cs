using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class StatusUtils
{
    public static void CombineStatus(List<StatComponent> targetComponents, List<StatComponent> sourceComponents, bool isAdd)
    {
        sourceComponents.ForEach(sourceComponent =>
            targetComponents.ForEach(targetComponent =>
                CombineStatus(targetComponent, sourceComponent, isAdd)));
    }
    public static void CombineStatus<T>(T target, T source, bool isAdd) where T : StatComponent
    {
        if (target.GetType() != source.GetType())
        {
            Debug.LogWarning("Cannot combine different types of stat components.");
            return;
        }
        if (isAdd)
            CombineFields(target, source);
        else
        {
            target.RemoveModifiersFromSource(source);
        }
    }

    private static void CombineFields<T>(T target, T source)
    {
        Type type = typeof(T);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(Stat))
            {
                Stat targetStat = field.GetValue(target) as Stat;
                Stat sourceStat = field.GetValue(source) as Stat;
                if (targetStat != null && sourceStat != null)
                {
                    targetStat.AddModifier(new StatModifier(sourceStat.Value, StatModType.Flat, 1, source));
                }
            }
        }
    }
}

[System.Serializable]
public class CombatStats : StatComponent
{
    public Stat maxHP = new Stat(100);
    public Stat cunHp = new Stat(100);
    public Stat speed = new Stat(5);
    public Stat attack = new Stat(10);          // 공격력
    public Stat defense = new Stat(5);          // 방어력
    public Stat criticalChance = new Stat(5);   // 치명타 확률
    public void RemoveModifiersFromSource(object source)
    {
        maxHP.RemoveAllModifiersFromSource(source);
        cunHp.RemoveAllModifiersFromSource(source);
        speed.RemoveAllModifiersFromSource(source);
        attack.RemoveAllModifiersFromSource(source);
        defense.RemoveAllModifiersFromSource(source);
        criticalChance.RemoveAllModifiersFromSource(source);
    }
}

[System.Serializable]
public class SurvivalStats : StatComponent
{
    public Stat experience = new Stat(0);
    public Stat level = new Stat(1);

    public Stat stamina = new Stat(100);      // 스태미나
    public Stat oxygen = new Stat(100);       // 산소 레벨
    public Stat hunger = new Stat(100);       // 허기
    public Stat thirst = new Stat(100);       // 갈증

    public Stat radiationLevel = new Stat(0); // 방사능 수치 (우주에서는 방사능 노출 가능성 존재)
    public Stat temperature = new Stat(37);   // 체온 (우주 환경에서는 온도 관리가 중요)

    // 기타 스탯들
    public Stat inventoryCapacity = new Stat(20); // 인벤토리 용량 (아이템을 얼마나 많이 들 수 있는지)

    // 모디파이어 제거 함수
    public void RemoveModifiersFromSource(object source)
    {
        experience.RemoveAllModifiersFromSource(source);
        level.RemoveAllModifiersFromSource(source);
        stamina.RemoveAllModifiersFromSource(source);
        oxygen.RemoveAllModifiersFromSource(source);
        hunger.RemoveAllModifiersFromSource(source);
        thirst.RemoveAllModifiersFromSource(source);
    }
}


[System.Serializable]
public class Attributes : StatComponent
{
    public Stat strengths = new Stat(10);     // 힘
    public Stat intelligence = new Stat(10);  // 지능
    public Stat stamina = new Stat(10);       // 지구력
    public void RemoveModifiersFromSource(object source)
    {
        strengths.RemoveAllModifiersFromSource(source);
        intelligence.RemoveAllModifiersFromSource(source);
        stamina.RemoveAllModifiersFromSource(source);
    }
}
[System.Serializable]
public class SkillStats : StatComponent
{
    public Stat smallGuns = new Stat(0);
    public Stat bigGuns = new Stat(0);
    public Stat energyWeapons = new Stat(0);
    public Stat explosives = new Stat(0);
    public Stat meleeWeapons = new Stat(1);
    public Stat unarmed = new Stat(0);
    public Stat throwing = new Stat(0);

    public void RemoveModifiersFromSource(object source)
    {
        smallGuns.RemoveAllModifiersFromSource(source);
        bigGuns.RemoveAllModifiersFromSource(source);
        energyWeapons.RemoveAllModifiersFromSource(source);
        explosives.RemoveAllModifiersFromSource(source);
        meleeWeapons.RemoveAllModifiersFromSource(source);
        unarmed.RemoveAllModifiersFromSource(source);
        throwing.RemoveAllModifiersFromSource(source);
    }
}