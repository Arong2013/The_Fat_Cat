using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임에서 땅에 붙어있는 모든것 
public class Field : MonoBehaviour
{
    [SerializeField] GameObject PlayerPre,EnemyPre;
    World parentWorld;
    public void MakeField(World world)
    {
        parentWorld = world;
    } 
}
