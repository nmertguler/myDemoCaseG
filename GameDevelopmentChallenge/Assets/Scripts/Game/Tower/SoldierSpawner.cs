using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierSpawner : MonoBehaviour
{
    [Space(5)]
    [SerializeField] TowerController towerController;
    [SerializeField] Image spawnFillImage;

    [Space(5)]
    [SerializeField] List<ClassSoldierValues> activeSoldierValues;

    [Space(5)]
    [SerializeField] List<GameObject> activeSoldier;

    // timer
    float currentSpawnValue = 99;
    float timer = 0;

    Transform spawnDotTransform;
    Transform soldierHolder;

    private void Start()
    {
        SpawnTimeUpdate();

        spawnDotTransform = towerController.GetSpawnDot();

        soldierHolder = GameVariables.Instance.soldierHolder;
    }

    private void Update()
    {
        bool maxSoldierControl = activeSoldier.Count >= towerController.GetActiveMaxSoldierValue();
        bool levelEnd = false;

        if (maxSoldierControl == false && levelEnd == false)
        {
            timer += Time.deltaTime;
            float fillAmount = timer / currentSpawnValue;
            spawnFillImage.fillAmount = fillAmount;

            if (timer >= currentSpawnValue)
            {
                SoldierSpawn();
            }
        }
    }

    void SoldierSpawn()
    {
        GameObject tempSoldier = PoolController.Instance.Soldier;

        if (activeSoldier.Contains(tempSoldier) || activeSoldierValues.Count == 0 || activeSoldierValues.Count == 0)
        {
            return;
        }


        // timer sifirla
        timer = 0;
        SpawnTimeUpdate();



        tempSoldier.transform.SetParent(soldierHolder);
        tempSoldier.transform.position = spawnDotTransform.position
            + new Vector3(UnityEngine.Random.Range(-.5F, .5F), 0, UnityEngine.Random.Range(-.2F, .2F));
        tempSoldier.transform.rotation = spawnDotTransform.rotation;

        // todo: EnumUnitType activeRandomUnitType 

        var soldierSpawn = activeSoldierValues[Random.Range(0, activeSoldierValues.Count)];

        tempSoldier.GetComponent<SoldierController>().SoldierInit(towerController.GetArmyType(), gameObject, soldierSpawn.soldierType, soldierSpawn.levelNumber , towerController.towerNumber);

        activeSoldier.Add(tempSoldier);


        SoldierAttackControl();
    }

    void SoldierAttackControl()
    {
        if(towerController.towerType == EnumArmyType.player)
        {
            return;
        }

        int soldierCount = activeSoldier.Count;

        if(soldierCount > UnityEngine.Random.Range(3,6))
        {
            //attack start
            Vector3 attackTargetPos = LevelHolder.Instance.RandomEnemyPos(towerController);
            towerController.MoveToPoint(attackTargetPos);
        }
    }

    void SpawnTimeUpdate()
    {
        currentSpawnValue = towerController.GetActiveSpawnTime();
    }

    public List<GameObject> GetMovementSoldierList()
    {
        List<GameObject> tempMovementSoldiers = new List<GameObject>();

        for (int i = 0; i < activeSoldier.Count; i++)
        {
            bool control = activeSoldier[i].GetComponent<SoldierController>().MoveControl();

            if(control)
            {
                tempMovementSoldiers.Add(activeSoldier[i]);
            }
        }

        return tempMovementSoldiers;
    }

    public void DeadSoldierRemoveFromList(GameObject deadSoldier)
    {
        while (activeSoldier.Contains(deadSoldier))
        {
            activeSoldier.Remove(deadSoldier);
        }

        List<GameObject> tempActiveSoldier = new List<GameObject>(activeSoldier);
        foreach (var item in tempActiveSoldier)
        {
            if (item.activeSelf == false)
            {

            }
        }
    }

    public void SoldierValueUp(EnumUnitType unitType)
    {
        ClassSoldierValues tempSoldierVal = activeSoldierValues.Find(a => a.soldierType == unitType);

        if (tempSoldierVal == null)
        {
            tempSoldierVal = new ClassSoldierValues();
            tempSoldierVal.soldierType = unitType;
            tempSoldierVal.levelNumber = 1;

            activeSoldierValues.Add(tempSoldierVal);
        }
        else
        {
            tempSoldierVal.levelNumber += 1;
        }
    }
}
