using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelXpDatas", menuName = "ScriptableObjects/Datas/LevelXpDatas", order = 1)]
public class LevelXpDatas : ScriptableObject
{
    public List<ClassLevelXpVariables> levelXpVariables;
    public float stabilXpIncrease = 200;
}

[Serializable]
public class ClassLevelXpVariables
{
    public int levelNumber;
    public int levelUpValue;

}
