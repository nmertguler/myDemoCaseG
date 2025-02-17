using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectController : MonoBehaviour
{
    [SerializeField] List<ClassSoldierValues> activeSoldierValues;
    int maxCardLevel = 5;

    [Space(5)]
    [SerializeField] List<UnitCard> unitCards;

    [Space(5)]
    [SerializeField] GameObject upgradeSelectPanel;


    // privates
    float tempTimeScale = 1;

    private void Start()
    {
        upgradeSelectPanel.SetActive(false);

        CardSelectShow();
    }

    [ContextMenu("Card Select Panel Show")]
    public void CardSelectShow()
    {
        List<EnumUnitType> showCardTypeList = new List<EnumUnitType>(GetRandomShowCards());

        if (showCardTypeList.Count == 0)
        {
            // all level complete
            return;
        }

        // show card select
        upgradeSelectPanel.SetActive(true);

        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;

        foreach (var item in unitCards)
        {
            item.gameObject.SetActive(false);
        }

        float activeDelay = .1F;
        float diffDelay = .2F;

        foreach (var item in showCardTypeList)
        {
            UnitCard showCard = unitCards.Find(a => a.cardType == item);

            showCard.gameObject.SetActive(true);

            int cardLevel = activeSoldierValues.Find(a => a.soldierType == item).levelNumber;

            showCard.CardCreate(cardLevel);

            DOVirtual.DelayedCall(activeDelay, () =>
            {
                showCard.SlideAnimPlay();
            });

            activeDelay += diffDelay;
        }
    }

    public void CardSelectHide()
    {
        upgradeSelectPanel.SetActive(false);

        Time.timeScale = tempTimeScale;
    }

    public List<EnumUnitType> GetRandomShowCards()
    {
        List<EnumUnitType> tempUnitType = new List<EnumUnitType>();

        List<ClassSoldierValues> tempSoldierValues = new List<ClassSoldierValues>(activeSoldierValues);

        if(FirstCardSelected() == true)
        {
            var towerTemp = tempSoldierValues.Find(a => a.soldierType == EnumUnitType.tower);
            tempSoldierValues.Remove(towerTemp);

        }

        foreach (var item in tempSoldierValues)
        {
            if (item.levelNumber < maxCardLevel)
            {
                tempUnitType.Add(item.soldierType);
            }
        }

        while (tempUnitType.Count > 3)
        {
            tempUnitType.RemoveAt(UnityEngine.Random.Range(0, tempUnitType.Count));
        }

        return tempUnitType;
    }

    bool FirstCardSelected()
    {
        bool control = true;

        foreach (var item in activeSoldierValues)
        {
            if(item.levelNumber == 0)
            {
                control = true;
            }
            else
            {
                control = false;
                break;
            }
        }

        return control;
    }

    public void CardSelect(UnitCard selectedCard)
    {
        // other card hide
        foreach (var item in unitCards)
        {
            if (item != selectedCard)
            {
                item.gameObject.SetActive(false);
            }
        }

        //level up
        EnumUnitType cardType = selectedCard.cardType;
        ClassSoldierValues soldierVal = activeSoldierValues.Find(x => x.soldierType == cardType);
        soldierVal.levelNumber += 1;

        if(cardType == EnumUnitType.tower)
        {
            // tower up
            LevelHolder.Instance.playerTower.TowerLevelUp();
        }
        else
        {
            // soldier up
            LevelHolder.Instance.playerTower.towerSoldierSpawner.SoldierValueUp(cardType);
        }
        

        selectedCard.CardSelectAnimPlay();

        SfxManager.Instance.PlayClipOneShot("tab" , .5F);

        float animTime = 1.05F;
        DOVirtual.DelayedCall(animTime, () =>
        {
            CardSelectHide();
        });

    }
}
