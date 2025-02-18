using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClassTowerTypes
{
    public int towerNumber = 0;
    public TowerController towerController;
}

public class LevelHolder : MonoBehaviour
{
    public int levelCount;

    [Space(5)]
    public List<ClassTowerTypes> towerTypes;

    // privates
    TowerController playerTower = null;

    public static LevelHolder Instance;
    private void Awake()
    {
        Instance = this;
    }



    public Vector3 RandomEnemyPos(TowerController tower)
    {
        Vector3 randEnemyPos = new Vector3(-10, 0, 0);

        List<ClassTowerTypes> tempTowerTypes = new List<ClassTowerTypes>(towerTypes);
        var currentTower = tempTowerTypes.Find(a => a.towerController == tower);
        tempTowerTypes.Remove(currentTower);

        if (tempTowerTypes.Count == 0)
        {
            return randEnemyPos;
        }

        randEnemyPos = tempTowerTypes[UnityEngine.Random.Range(0, tempTowerTypes.Count)].
            towerController.transform.position;


        return randEnemyPos;
    }

    public TowerController GetPlayerTower()
    {
        return towerTypes.Find(a => a.towerController.GetArmyType() == EnumArmyType.player).towerController;
    }

}
