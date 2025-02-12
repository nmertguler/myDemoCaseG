using DG.Tweening;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class Soldier : MonoBehaviour
{
    [Space(5)]
    public int levelNumber = 1;
    public SoldierDatas archerData;
    public EnumUnitType soldierType;

    [Space(5)]
    public float defaultHp;
    public float currentHp;

    [Space(5)]
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;

    [Space(5)]
    [SerializeField] UnityEvent attackStartEvent;

    public Animator soldierAnimator;
    public HighlightEffect highlightEffect;


    // privates
    ArcherArrowAttack archerArrowAttack;

    public void PlayAnim(string animName)
    {
        switch (animName)
        {
            case "spawn":
                break;

            case "idle":
                soldierAnimator.CrossFade("idle" , .2F);
                break;

            case "run":
                if(!soldierAnimator.GetCurrentAnimatorStateInfo(0).IsName("run"))
                {
                    soldierAnimator.CrossFade("run", .2F);
                }
                break;

            case "attack":
                soldierAnimator.Play("attack");
                break;

            case "death":
                soldierAnimator.CrossFade("death", .2F);
                break;

            default:
                Debug.LogError("wrong animation value");
                break;
        }
    }

    public float AttackAnimStart(GameObject enemy)
    {
        AnimatorStateInfo stateInfo = soldierAnimator.GetCurrentAnimatorStateInfo(0);
        float attackDelay = 0.15F;

        attackStartEvent?.Invoke();

        switch (soldierType)
        {
            case EnumUnitType.archer:

                // send arrow
                if(gameObject.TryGetComponent<ArcherArrowAttack>(out ArcherArrowAttack archerArrowAttack))
                {
                    DOVirtual.DelayedCall(attackDelay, ()=>
                    {
                        float duration = Vector3.Distance(transform.position, enemy.transform.position) / 10;
                        attackDelay += duration;
                        archerArrowAttack.ArrowAttackStart(enemy.transform.position , duration); 
                    });
                }
                break;
        }

        return attackDelay;
    }
}
