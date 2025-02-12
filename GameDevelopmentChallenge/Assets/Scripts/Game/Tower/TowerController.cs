using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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
    [SerializeField] TowerHpSlider towerHpSlider;
    [SerializeField] HighlightEffect highlightEffect;

    // privates
    float maxHp = 100;
    float currentHp = 100;

    private void Start()
    {
        LevelVariableUpdate();
        HpBarUpdate();
    }

    //private void OnValidate() => UnityEditor.EditorApplication.delayCall += _OnValidate;

    //private void _OnValidate()
    //{
    //    UnityEditor.EditorApplication.delayCall -= _OnValidate;
    //    if (this == null) return;

    //    // color change
    //    ColorUpdate();

    //    // level update
    //    LevelModelUpdate();
    //}

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
        List<Vector3> positions = GetRandomPointsInCircle(movePosition, .1F * activeSoldierList.Count, activeSoldierList.Count);


        foreach (var item in activeSoldierList)
        {
            if (item.TryGetComponent(out SoldierController soldierController))
            {
                soldierController.MoveToTarget(positions[0]);
                positions.RemoveAt(0);
            }
        }
    }

    // noktanin etrafindan noktalar secer boylece hareket eden soldier'lar ic ice gecmezler
    List<Vector3> GetRandomPointsInCircle(Vector3 center, float maxRadius, int count)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            // Rastgele bir mesafe (0 ile maxRadius arasýnda)
            float radius = Random.Range(0f, maxRadius);

            // Rastgele bir açý (0 ile 2 * PI arasýnda)
            float angle = Random.Range(0f, 2 * Mathf.PI);

            // X ve Z koordinatlarýný hesapla
            float x = center.x + radius * Mathf.Cos(angle);
            float z = center.z + radius * Mathf.Sin(angle);

            points.Add(new Vector3(x, center.y, z));
        }

        return points;
    }



    public EnumArmyType GetArmyType()
    {
        return towerType;
    }

    public void DeadSoldierRemoveFromList(GameObject deadSoldier)
    {
        towerSoldierSpawner.DeadSoldierRemoveFromList(deadSoldier);
    }

    void LevelVariableUpdate()
    {
        float tempMaxHp = maxHp;

        // current hp value update
        var tempVariable = towerData.towerVariables.Find(a => a.levelNumber == ActiveLevelNumber());
        maxHp = tempVariable.hpValue;

        currentHp += maxHp - tempMaxHp;
    }

    void HpBarUpdate()
    {
        towerHpSlider.HpBarUpdate(ActiveLevelNumber() , maxHp , currentHp);
    }

    public void DoDamage(float damageValue)
    {
        if(currentHp == 0)
        {
            return;
        }

        currentHp -= damageValue;

        highlightEffect.HitFX();

        if (currentHp <= 0)
        {
            currentHp = 0;

            // tower destroy
            gameObject.layer = LayerMask.NameToLayer("Default");
            towerSoldierSpawner.enabled = false;
        }

        HpBarUpdate();
    }
}
