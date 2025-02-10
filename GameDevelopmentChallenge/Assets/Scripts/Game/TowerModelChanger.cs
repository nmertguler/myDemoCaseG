using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerModelChanger : MonoBehaviour
{
    [SerializeField] List<ClassTowerModelVariables> towerModelVariableList;

    [Serializable]
    public class ClassTowerModelVariables
    {
        public int levelNumber;
        public GameObject towerModel;
    }

    public void TowerModelUpdate(int towerLevelNumber)
    {
        foreach (var item in towerModelVariableList)
        {
            item.towerModel.SetActive(false);
        }

        ClassTowerModelVariables ctmv = towerModelVariableList.Find(a => a.levelNumber == towerLevelNumber);
        ctmv.towerModel.SetActive(true);
    }
}
