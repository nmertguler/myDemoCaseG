using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Space(5)]
    public int levelNumber = 1;
    public SoldierDatas archerData;

    [Space(5)]
    [SerializeField] float defaultHp;
    [SerializeField] float currentHp;

    [Space(5)]
    [SerializeField] float attackDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRange;

    [SerializeField] Animator soldierAnimator;
}
