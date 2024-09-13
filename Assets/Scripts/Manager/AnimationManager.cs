using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    [OdinSerialize, SerializeField]
    Dictionary<int, AnimationClip[]> AnimaionClipData;
    private int GetAnimationOrder(AnimationClip clip)
    {
        foreach (var kvp in AnimaionClipData)
        {
            if (System.Array.Exists(kvp.Value, element => element == clip))
            {
                return kvp.Key;
            }
        }
        return -1;
    }
    public bool CanPlayAnime(AnimationClip currentAnimationClip, AnimationClip nextAnimationClip)
    {
        int currentOrder = GetAnimationOrder(currentAnimationClip);
        int nextOrder = GetAnimationOrder(nextAnimationClip);

        if (nextOrder == -1)
        {
            Debug.LogWarning("The next animation clip is not found in the dictionary.");
            return false;
        }
        if (currentOrder == -1)
        {
            // 현재 애니메이션이 없는 경우(예: 처음 재생할 때) 다음 애니메이션을 재생 가능
            return true;
        }
        return nextOrder <= currentOrder;
    }
}
