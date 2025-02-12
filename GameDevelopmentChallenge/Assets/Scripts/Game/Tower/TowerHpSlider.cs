using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerHpSlider : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI currentHpText;
    [SerializeField] Image fillImage;

    public void HpBarUpdate(int levelCount, float maxHp, float currentHp)
    {
        // level text update
        levelText.text = levelCount.ToString();

        // image fill value update
        float fillValue = currentHp / maxHp;
        fillImage.fillAmount = fillValue;

        // hp value uptade
        currentHpText.text = ((int)currentHp).ToString();
    }
}
