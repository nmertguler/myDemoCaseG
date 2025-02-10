using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoldierDatas", menuName = "ScriptableObjects/Datas/SoldierDatas", order = 1)]
public class SoldierDatas : ScriptableObject
{
    public List<SoldierVariables> unityVariablesLevelBase;
}

[Serializable]
public class SoldierVariables
{
    public int levelNumber;
    public float attackSpeedMultiplier;
    public float hpMultiplier;
}
