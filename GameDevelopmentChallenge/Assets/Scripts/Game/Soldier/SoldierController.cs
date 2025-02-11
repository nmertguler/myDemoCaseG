using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using DG.Tweening;

public class SoldierController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Soldier currentSoldierType;
    [SerializeField] HpBar hpBar;

    [Space(5)]
    [SerializeField] EnumSoldierStates soldierStates;
    [SerializeField] float scanEnemyRadius;

    LayerMask enemyLayerMasks;
    Coroutine scanCoroutine;
    GameObject myTower;

    private void OnEnable()
    {
        scanCoroutine = StartCoroutine(EnumeratorScanEnemy());
    }
    private void OnDisable()
    {
        StopCoroutine(scanCoroutine);
    }
    public void SoldierCreate(EnumArmyType armyType, GameObject myTowerObject, EnumUnitType unitType)
    {

        // my tower
        myTower = myTowerObject;

        // layer update
        switch (armyType)
        {
            case EnumArmyType.player:
                gameObject.layer = LayerMask.NameToLayer("PlayerSoldier");
                enemyLayerMasks = LayerMask.GetMask("EnemySoldier1", "EnemySoldier2", "Tower");
                break;
            case EnumArmyType.enemy:
                gameObject.layer = LayerMask.NameToLayer("EnemySoldier1");
                enemyLayerMasks = LayerMask.GetMask("PlayerSoldier", "EnemySoldier2", "Tower");
                break;
            case EnumArmyType.enemy2:
                gameObject.layer = LayerMask.NameToLayer("EnemySoldier2");
                enemyLayerMasks = LayerMask.GetMask("PlayerSoldier", "EnemySoldier1", "Tower");
                break;
            default:
                Debug.LogError("yanlis armytype degeri geldi");
                break;
        }

        // delay the spawn animation duration
        DOVirtual.DelayedCall(.3F, () =>
        {
            soldierStates = EnumSoldierStates.idle;
        });
    }

    public void SoldierReset()
    {
        soldierStates = EnumSoldierStates.none;
    }

    public void SoldierDeath()
    {
        soldierStates = EnumSoldierStates.dead;

        currentSoldierType.PlayAnim("death");

        gameObject.layer = LayerMask.NameToLayer("Default");

        agent.enabled = false;

        hpBar.gameObject.SetActive(false);
    }


    public void MoveToTarget(Vector3 targetPos)
    {
        if (soldierStates != EnumSoldierStates.idle)
        {
            return;
        }

        soldierStates = EnumSoldierStates.move;
        agent.SetDestination(targetPos);
        currentSoldierType.PlayAnim("run");

        StartCoroutine(EnumeratorMoveStart());
    }

    IEnumerator EnumeratorMoveStart()
    {
        yield return new WaitUntil(() => agent.velocity.magnitude < .1F 
        && Vector3.Distance(transform.position , agent.destination) < .6F);

        if(soldierStates == EnumSoldierStates.move)
        {
            // walk is over
            currentSoldierType.PlayAnim("idle");
            soldierStates = EnumSoldierStates.idle;
            agent.SetDestination(transform.position);
        }

        
    }

    IEnumerator EnumeratorScanEnemy()
    {
        while (true)
        {
            if (soldierStates == EnumSoldierStates.idle || soldierStates == EnumSoldierStates.move)
            {
                Collider[] enemies = Physics.OverlapSphere(transform.position, scanEnemyRadius, enemyLayerMasks);
                enemies = enemies.Where(e => e.gameObject != myTower).ToArray();

                if (enemies.Length > 0)
                {
                    GameObject closestEnemy = enemies.ToList().Select(e => e.gameObject)
                        .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
                        .FirstOrDefault();

                    StartCoroutine(AttackTheEnemy(closestEnemy));
                }
            }

            yield return new WaitForSeconds(.3F);
        }
    }

    IEnumerator AttackTheEnemy(GameObject enemy)
    {
        soldierStates = EnumSoldierStates.attack;
        float attackDistance = currentSoldierType.attackRange;
        transform.LookAt(enemy.transform.position);

        while (true)
        {
            if(agent.enabled == false)
            {
                break;
            }

            agent.SetDestination(enemy.transform.position);
            
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (targetDistance < attackDistance)
            {
                agent.SetDestination(transform.position);

                // attack to target
                StartCoroutine(AttackToEnemy(enemy));
                break;
            }

            if (Vector3.Distance(transform.position, enemy.transform.position) > .2F)
            {
                currentSoldierType.PlayAnim("run");
            }


            yield return new WaitForSeconds(.1F);
        }


        yield return null;
    }

    IEnumerator AttackToEnemy(GameObject enemy)
    {
        currentSoldierType.PlayAnim("attack");

        float attackDamageDelayTime = currentSoldierType.AttackAnimStart(enemy);


        yield return new WaitForSeconds(attackDamageDelayTime);

        // do damage to enemy
        float damage = currentSoldierType.attackDamage;
        if (enemy.TryGetComponent<SoldierController>(out SoldierController enemySoldierController))
        {
            enemySoldierController.DoDamage(damage);
        }

        // attack complete
        soldierStates = EnumSoldierStates.idle;

        yield return null;
    }


    public void DoDamage(float damage)
    {
        // get
        float maxHp = currentSoldierType.defaultHp;
        float currentHp = currentSoldierType.currentHp;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            // dead
            currentHp = 0;
            SoldierDeath();
        }

        // hp bar update
        hpBar.HpBarUpdate(currentHp / maxHp);

        // set
        currentSoldierType.currentHp = currentHp;

    }


}
