using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClassSoldierValues
{
    public int levelNumber;
    public EnumUnitType soldierType;
}

[Serializable]
public class ClassUnitHolder
{
    public EnumUnitType unitType;
    public Soldier soldierSc;
}
