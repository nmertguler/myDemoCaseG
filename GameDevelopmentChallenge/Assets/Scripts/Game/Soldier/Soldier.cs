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
    public EnumUnitType soldierType;

    [Space(5)]
    public float defaultHp;
    public float currentHp;

    [Space(5)]
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

    [Space(5)]
    [SerializeField] UnityEvent attackStartEvent;

    public Animator soldierAnimator;
    public HighlightEffect highlightEffect;

    [Space(5)]
    [SerializeField] Animator horseAnimator;


    // privates
    ArcherArrowAttack archerArrowAttack;

    public void PlayAnim(string animName)
    {
        switch (animName)
        {
            case "spawn":
                break;

            case "idle":
                soldierAnimator.Play("idle");
                if (horseAnimator != null) HorseAnim(animName);
                break;

            case "run":
                if(!soldierAnimator.GetCurrentAnimatorStateInfo(0).IsName("run"))
                {
                    soldierAnimator.Play("run");
                    if (horseAnimator != null)
                    {
                        HorseAnim(animName);
                        soldierAnimator.Play("idle");
                    }
                }
                break;

            case "attack":
                soldierAnimator.Play("attack");
                if (horseAnimator != null) HorseAnim("idle");
                break;

            case "death":
                soldierAnimator.Play("death");
                if (horseAnimator != null) HorseAnim(animName);
                break;

            default:
                Debug.LogError("wrong animation value");
                break;
        }
    }

    void HorseAnim(string animName)
    {
        horseAnimator.Play(animName); 
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
