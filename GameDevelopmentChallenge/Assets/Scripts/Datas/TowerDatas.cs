using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerDatas", menuName = "ScriptableObjects/Datas/Tower", order = 1)]
public class TowerDatas : ScriptableObject
{
    public List<ClassTowerVariables> towerVariables;
}

[Serializable]
public class ClassTowerVariables
{
    public int levelNumber;
    public float spawnMultiplierValue;
    public float baseAttackMultiplierValue;
    public float hpValue;
    public int maxSoldierValue;

}
