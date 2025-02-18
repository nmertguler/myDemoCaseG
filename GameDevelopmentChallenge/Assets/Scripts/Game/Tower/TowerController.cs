using DG.Tweening;
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
    public EnumArmyType towerType;
    public int towerNumber = 0;
    [SerializeField] int towerLevel;

    [Space(5)]
    [SerializeField] ColorChanger towerColorChanger;
    [SerializeField] TowerModelChanger towerModelChanger;
    public SoldierSpawner towerSoldierSpawner;

    [Space(5)]
    [SerializeField] TowerDatas towerData;
    [SerializeField] float baseSpawnTime;
    [SerializeField] Transform spawnDot;
    [SerializeField] TowerHpSlider towerHpSlider;
    [SerializeField] HighlightEffect highlightEffect;
    [SerializeField] Animator towerUpEffectAnimator;
    [SerializeField] GameObject spawnBar;


    // privates
    float maxHp = 100;
    float currentHp = 100;

    private void Start()
    {
        LevelVariableUpdate();
        HpBarUpdate();

        // color change
        ColorUpdate();

        
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
                switch (towerNumber)
                {
                    case 0:
                        towerColorChanger.ColorChangeFunc("enemyTowerMaterial");
                        break;

                    case 1:
                        towerColorChanger.ColorChangeFunc("enemyTowerMaterial2");
                        break;
                }
                break;
        }
    }

    // leveline gore kale gorunusunu gunceller
    public void LevelModelUpdate()
    {
        int levelNumber = ActiveLevelNumber();

        towerModelChanger.TowerModelUpdate(levelNumber);
    }

    int ActiveLevelNumber()
    {
        int levelNumber = towerLevel;

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
        List<GameObject> movementSoldierList = new List<GameObject>(towerSoldierSpawner.GetMovementSoldierList());
        //List<Vector3> positions = GetRandomPointsInCircle(movePosition, .1F * activeSoldierList.Count, activeSoldierList.Count);
        Vector3 dir = transform.position-  movePosition;
        List<Vector3> positions = GameVariables.Instance.attackPattern.GetPattern(movePosition, dir, movementSoldierList.Count, .45F);


        foreach (var item in movementSoldierList)
        {
            if (item.TryGetComponent(out SoldierController soldierController))
            {
                soldierController.MoveToTarget(positions[0]);
                positions.RemoveAt(0);
            }
        }
    }



    public EnumArmyType GetArmyType()
    {
        return towerType;
    }

    public void DeadSoldierRemoveFromList(GameObject deadSoldier)
    {
        towerSoldierSpawner.DeadSoldierRemoveFromList(deadSoldier);
    }

    public void TowerLevelUp()
    {
        towerLevel += 1;

        towerLevel = Mathf.Clamp(towerLevel, 1, 5);

        LevelVariableUpdate();
        HpBarUpdate();

        if(towerType == EnumArmyType.player)
        {
            towerUpEffectAnimator.Play("levelUp");
        }
    }

    void LevelVariableUpdate()
    {
        float tempMaxHp = maxHp;

        // current hp value update
        var tempVariable = towerData.towerVariables.Find(a => a.levelNumber == ActiveLevelNumber());
        maxHp = tempVariable.hpValue;

        baseSpawnTime /= tempVariable.spawnMultiplierValue;

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
        SfxManager.Instance.PlayClipOneShot("tab", .2F);
        SfxManager.Instance.SetHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType.LightImpact);


        highlightEffect.HitFX();

        if (currentHp <= 0)
        {
            currentHp = 0;

            // tower destroy
            gameObject.layer = LayerMask.NameToLayer("Default");
            towerSoldierSpawner.enabled = false;
            towerHpSlider.gameObject.SetActive(false);
            spawnBar.SetActive(false);

            gameObject.transform.DOMoveY(gameObject.transform.position.y - 5 , 2).OnComplete( ()=>
            {
                gameObject.SetActive(false);
            });
        }

        HpBarUpdate();
    }
}
