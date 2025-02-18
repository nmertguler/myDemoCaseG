using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using DG.Tweening;
using Unity.Services.Analytics.Internal;
using Unity.VisualScripting;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.PlayerLoop;

public class SoldierController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Soldier currentSoldierType;
    [SerializeField] Transform soldierTypeHolder;
    [SerializeField] HpBar hpBar;

    [Space(5)]
    public int soldierLevel;
    [SerializeField] EnumSoldierStates soldierStates;
    [SerializeField] float scanEnemyRadius;

    [Space(5)]
    [SerializeField] List<ClassUnitHolder> unitHolders;


    LayerMask enemyLayerMasks;
    Coroutine scanCoroutine;
    GameObject myTower;
    bool playerSoldier = false;
    bool givedXp = false;

    private void OnEnable()
    {
        scanCoroutine = StartCoroutine(EnumeratorScanEnemy());
    }
    private void OnDisable()
    {
        StopCoroutine(scanCoroutine);
    }
    public void SoldierInit(EnumArmyType armyType, GameObject myTowerObject, EnumUnitType unitType, int unitLevel, int towerNumber)
    {
        // unit type
        UnitTypeUpdate(armyType, myTowerObject, unitType, unitLevel, towerNumber);

    }

    void UnitTypeUpdate(EnumArmyType armyType, GameObject myTowerObject, EnumUnitType unitType, int unitLevel, int towerNumber)
    {

        GameObject tempSoldierType = null;
        string prefabAddress = unitHolders.Find(a => a.unitType == unitType).prefabAddress;
        Addressables.InstantiateAsync(prefabAddress).Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                tempSoldierType = handle.Result;

                currentSoldierType = tempSoldierType.GetComponent<Soldier>();

                currentSoldierType.transform.SetParent(soldierTypeHolder);
                currentSoldierType.transform.localPosition = Vector3.zero;
                currentSoldierType.transform.localRotation = Quaternion.identity;
                currentSoldierType.transform.localScale = Vector3.one;

                agent.speed = currentSoldierType.soldierData.soldierStats.movementSpeed;
                currentSoldierType.soldierAnimator.SetFloat("MoveSpeedMulti", currentSoldierType.soldierData.soldierStats.movementSpeed / 3.5F);

                // my tower
                myTower = myTowerObject;

                // level
                soldierLevel = unitLevel;
                currentSoldierType.levelNumber = unitLevel;
                currentSoldierType.CurrentHp = currentSoldierType.MaxHp;

                // sfx
                SfxManager.Instance.PlayClipOneShot("spawn", .5F);

                // soldier color update
                if (currentSoldierType.gameObject.TryGetComponent<SoldierColorUpdate>(out SoldierColorUpdate soldierColorUpdate))
                {
                    soldierColorUpdate.ColorUpdate(armyType, towerNumber); 
                }

                // layer update
                playerSoldier = false;
                switch (armyType)
                {
                    case EnumArmyType.player:
                        gameObject.layer = LayerMask.NameToLayer("PlayerSoldier");
                        enemyLayerMasks = LayerMask.GetMask("EnemySoldier1", "EnemySoldier2", "Tower");
                        playerSoldier = true;
                        break;
                    case EnumArmyType.enemy:

                        switch (towerNumber)
                        {
                            case 0:
                                gameObject.layer = LayerMask.NameToLayer("EnemySoldier1");
                                enemyLayerMasks = LayerMask.GetMask("PlayerSoldier", "EnemySoldier2", "Tower");
                                break;

                            case 1:
                                gameObject.layer = LayerMask.NameToLayer("EnemySoldier2");
                                enemyLayerMasks = LayerMask.GetMask("PlayerSoldier", "EnemySoldier1", "Tower");
                                break;
                        }

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
            else
            {
                Debug.LogError("prefab dont loaded");
            }
        };
    }

    public void SoldierReset()
    {
        soldierStates = EnumSoldierStates.none;

        givedXp = false;

        currentSoldierType.CurrentHp = currentSoldierType.soldierData.soldierStats.health;

        hpBar.gameObject.SetActive(true);
        hpBar.HpBarUpdate(1);


    }

    public void SoldierDeath()
    {
        soldierStates = EnumSoldierStates.dead;

        currentSoldierType.PlayAnim("death");

        gameObject.layer = LayerMask.NameToLayer("Default");

        agent.enabled = false;

        hpBar.gameObject.SetActive(false);

        transform.DOMoveY(transform.position.y - 3, 2.0F).SetDelay(1.7F).OnComplete(() =>
        {
            ReturnToPool();
        });

        // remove from tower soldier list
        if (myTower != null && myTower.TryGetComponent<TowerController>(out TowerController towerController))
        {
            towerController.DeadSoldierRemoveFromList(gameObject);
        }
    }

    void ReturnToPool()
    {
        // reset
        transform.DOKill();
        Addressables.Release(currentSoldierType.gameObject);
        agent.enabled = true;

        // return to pool
        PoolController.Instance.Soldier = gameObject;

        
    }


    public void MoveToTarget(Vector3 targetPos)
    {
        if (soldierStates != EnumSoldierStates.idle && soldierStates != EnumSoldierStates.move || currentSoldierType == null)
        {
            return;
        }

        soldierStates = EnumSoldierStates.move;
        agent.enabled = true;
        agent.SetDestination(targetPos);
        currentSoldierType.PlayAnim("run");
         
        StartCoroutine(EnumeratorMoveStart());
    }

    public bool MoveControl()
    {
        // true: can movement
        // false: blocked

        bool control = true;

        if (soldierStates != EnumSoldierStates.idle && soldierStates != EnumSoldierStates.move)
        {
            control = false;
        }

        return control;
    }

    IEnumerator EnumeratorMoveStart()
    {
        yield return new WaitUntil(() => agent.velocity.magnitude < .1F
        && Vector3.Distance(transform.position, agent.destination) < .6F);

        if (soldierStates == EnumSoldierStates.move)
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
            if (soldierStates == EnumSoldierStates.idle)
            {
                ScanEnemy();
            }

            yield return new WaitForSeconds(.3F);
        }
    }

    void ScanEnemy()
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

    IEnumerator AttackTheEnemy(GameObject enemy)
    {
        soldierStates = EnumSoldierStates.attack;
        float attackDistance = currentSoldierType.soldierData.soldierStats.attackRange;
        transform.LookAt(enemy.transform.position);
        TowerController towerControllerSc = enemy.GetComponent<TowerController>();

        while (true)
        {
            if (agent.enabled == false)
            {
                break;
            }

            if (enemy.activeInHierarchy == false)
            {
                soldierStates = EnumSoldierStates.idle;
                currentSoldierType.PlayAnim("idle");
                break;
            }

            agent.SetDestination(enemy.transform.position);

            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (towerControllerSc != null)
            {
                // target is tower 
                targetDistance *= .75F;
            }

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
        float attackSpeed = currentSoldierType.soldierData.soldierStats.attackSpeed;


        yield return new WaitForSeconds(attackDamageDelayTime * 1.4F);

        // do damage to enemy
        float damage = currentSoldierType.AttackDamage;
        if (enemy.TryGetComponent<SoldierController>(out SoldierController enemySoldierController))
        {
            enemySoldierController.DoDamage(damage);

            if (enemySoldierController.IsDead() && playerSoldier && enemySoldierController.GivedXp == false)
            {
                // kill and give xp
                GameVariables.Instance.giveXp.GiveXpFunc(enemy.transform.position, enemySoldierController.soldierLevel);
                enemySoldierController.GivedXp = true;
            }
        }

        // do damage to tower
        if (enemy.TryGetComponent<TowerController>(out TowerController towerController))
        {
            towerController.DoDamage(damage);
        }

        //yeni bir atis yapmak icin bekle
        yield return new WaitForSeconds(attackSpeed);

        // attack complete
        soldierStates = EnumSoldierStates.idle;

        yield return null;
    }


    public void DoDamage(float damage)
    {
        // If he takes damage while idle, he will attack
        if (soldierStates == EnumSoldierStates.idle || soldierStates == EnumSoldierStates.move)
        {
            ScanEnemy();
        }

        // get
        float maxHp = currentSoldierType.soldierData.soldierStats.health;
        float currentHp = currentSoldierType.CurrentHp;

        //hit fx
        currentSoldierType.highlightEffect.HitFX();

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
        currentSoldierType.CurrentHp = currentHp;

    }

    public bool IsDead()
    {
        bool isDead = false;

        if (currentSoldierType.CurrentHp == 0)
        {
            isDead = true;
        }

        return isDead;
    }

    public bool GivedXp
    {
        get
        {
            return givedXp;
        }

        set
        {
            givedXp = value;
        }
    }


}
