using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : MonoBehaviour
{
    public EnumUnitType cardType;

    [Space(5)]
    [SerializeField] List<GameObject> icons;
    [SerializeField] List<GameObject> stars;
    [SerializeField] Animator animator;
    [SerializeField] GameObject buffs;

    bool buttonClickControl = false;

    public void CardCreate(int cardLevel)
    {
        // icon show
        foreach (var item in icons)
        {
            item.SetActive(false);
        }
        icons[cardLevel].SetActive(true);

        // star show
        foreach (var item in stars)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < cardLevel + 1; i++)
        {
            stars[i].SetActive(true);
        }

        // buffs show
        if(cardLevel == 0)
        {
            buffs.SetActive(false);
        }
        else
        {
            buffs.SetActive(true);
        }

        buttonClickControl = false;
    }

    public void ButtonDown()
    {
        if(buttonClickControl == true)
        {
            //butona tiklanmis
            return;
        }

        buttonClickControl = true;

        GameVariables.Instance.cardSelectController.CardSelect(this);
    }

    public void SlideAnimPlay()
    {
        animator.Play("CardSlide");
    }

    public void CardSelectAnimPlay()
    {
        animator.Play("CardSelect");
    }

    
}
