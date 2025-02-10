using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public class ClassTowerLevelVariables
    {
        public int levelNumber;
        public GameObject modelGameobject;
    }


    [Header("Tower Variables")]
    [SerializeField] EnumArmyType towerType;
    [SerializeField] EnumTowerLevel towerLevel;

    [Space(5)]
    [SerializeField] ColorChanger towerColorChanger;
    [SerializeField] TowerModelChanger towerModelChanger;
    [SerializeField] SoldierSpawner towerSoldierSpawner;

    [Space(5)]
    [SerializeField] TowerDatas towerData;
    [SerializeField] float baseSpawnTime;
    [SerializeField] Transform spawnDot;


    private void OnValidate()
    {
        // color change
        ColorUpdate();

        // level update
        LevelModelUpdate();
    }

    // tarafina gore kalenin renklerini duzenler
    void ColorUpdate()
    {
        switch (towerType)
        {
            case EnumArmyType.none:
                break;

            case EnumArmyType.player:
                towerColorChanger.ColorChangeFunc("playerTowerMaterial");
                break;

            case EnumArmyType.enemy:
                towerColorChanger.ColorChangeFunc("enemyTowerMaterial");
                break;
            case EnumArmyType.enemy2:
                towerColorChanger.ColorChangeFunc("enemyTowerMaterial2");
                break;
        }
    }

    // leveline gore kale gorunusunu gunceller
    void LevelModelUpdate()
    {
        int levelNumber = ActiveLevelNumber();

        towerModelChanger.TowerModelUpdate(levelNumber);
    }

    int ActiveLevelNumber()
    {
        int levelNumber = 0;
        switch (towerLevel)
        {
            case EnumTowerLevel.level1:
                levelNumber = 1;
                break;
            case EnumTowerLevel.level2:
                levelNumber = 2;
                break;
            case EnumTowerLevel.level3:
                levelNumber = 3;
                break;
            case EnumTowerLevel.level4:
                levelNumber = 4;
                break;
            case EnumTowerLevel.level5:
                levelNumber = 5;
                break;
        }

        return levelNumber;
    }

    public float GetActiveSpawnTime()
    {
        float tempTimer = 0;

        int activeLevelNumber = ActiveLevelNumber();
        tempTimer = baseSpawnTime * towerData.towerVariables[activeLevelNumber - 1].spawnMultiplierValue;

        return tempTimer;
    }

    public int GetActiveMaxSoldierValue()
    {
        int tempMaxSoldier = 0;

        int activeLevelNumber = ActiveLevelNumber();
        tempMaxSoldier = towerData.towerVariables[activeLevelNumber - 1].maxSoldierValue;

        return tempMaxSoldier;
    }

    public Transform GetSpawnDot()
    {
        return spawnDot;
    }

    public void MoveToPoint(Vector3 movePosition)
    {
        List<GameObject> activeSoldierList = new List<GameObject>(towerSoldierSpawner.GetActiveSoldierList());

        foreach (var item in activeSoldierList)
        {
            if(item.TryGetComponent(out SoldierController soldierController))
            {
                soldierController.MoveToTarget(movePosition);
            }
        }
    }
}
