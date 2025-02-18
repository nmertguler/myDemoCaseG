using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoldierDatas", menuName = "ScriptableObjects/Datas/SoldierDatas", order = 1)]
public class SoldierDatas : ScriptableObject
{
    public SoldierVariables soldierStats; 
}

[Serializable]
public class SoldierVariables
{
    public float health;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;
}
