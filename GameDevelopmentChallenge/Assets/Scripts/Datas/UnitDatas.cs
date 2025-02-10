using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "unitName", menuName = "ScriptableObjects/UnitDatas", order = 1)]
public class UnitDatas : ScriptableObject
{
    public List<UnitVariables> unityVariablesLevelBase;
}

[Serializable]
public class UnitVariables
{
    public int levelNumber;
    public EnumUnitType unitType;

}
