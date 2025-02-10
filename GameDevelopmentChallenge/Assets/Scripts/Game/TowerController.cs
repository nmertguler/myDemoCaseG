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
    [SerializeField] EnumUnitType unitType;
    [SerializeField] EnumTowerLevel towerLevel;

    [Space(5)]
    [SerializeField] ColorChanger towerColorChanger;
    [SerializeField] TowerModelChanger towerModelChanger;


    private void OnValidate()
    {
        // color change
        ColorUpdate();

        // level update
        LevelModelUpdate();
    }

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
        }
    }

    void LevelModelUpdate()
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
            default:
                break;
        }

        towerModelChanger.TowerModelUpdate(levelNumber);
    }
}
