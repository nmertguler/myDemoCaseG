using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpBarController : MonoBehaviour
{


    [Space(5)]
    [SerializeField] LevelXpDatas levelXpDatas;

    [Space(5)]
    [SerializeField] Image xpFill_img;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI xpText;


    // privates
    float levelUpXpValue = 0;
    float currentXpValue = 0;
    int currentLevel = 1;

    private void Start()
    {
        LevelUpXpUpdate();
        XpIncrease();
    }

    void LevelUpXpUpdate()
    {
        if (levelXpDatas.levelXpVariables.Count >= currentLevel)
        {
            levelUpXpValue = levelXpDatas.levelXpVariables[currentLevel - 1].levelUpValue;
        }
        else
        {
            int dif = currentLevel - levelXpDatas.levelXpVariables.Count;
            levelUpXpValue = levelXpDatas.levelXpVariables[levelXpDatas.levelXpVariables.Count - 1].levelUpValue
                + dif * levelXpDatas.stabilXpIncrease;
        }
    }

    public void XpIncrease(float increaseValue = 0)
    {
        currentXpValue += increaseValue;

        // level text
        levelText.text = "Level " + currentLevel.ToString();

        // xp text
        xpText.text = ((int)currentXpValue).ToString() + "/" + ((int)levelUpXpValue).ToString();

        // fill img
        float fillValue = currentXpValue / levelUpXpValue;
        xpFill_img.fillAmount = fillValue;

        if(fillValue >= 1)
        {
            LevelUp();
        }
    }

    public Vector3 XpEffectTargetPos()
    {
        Vector3 target = GetFillEndPosition();


        return target;
    }

    void LevelUp()
    {
        currentLevel += 1;

        currentXpValue = 0;

        LevelUpXpUpdate();

        XpIncrease(0);

        // select card
        GameVariables.Instance.cardSelectController.CardSelectShow();
    }

    Vector3 GetFillEndPosition()
    {
        RectTransform imageTransform = xpFill_img.rectTransform;

        float fillAmount = xpFill_img.fillAmount; // 0.6 örnek olarak
        Rect rect = imageTransform.rect; // UI Image'ýn dikdörtgen boyutu
        float width = rect.width; // Geniþlik
        float height = rect.height; // Yükseklik

        // Doluluk yönüne göre uç noktanýn local pozisyonunu hesapla
        Vector3 localEndPos = Vector3.zero;

        switch (xpFill_img.fillMethod)
        {
            case UnityEngine.UI.Image.FillMethod.Horizontal:
                localEndPos = new Vector3(-width * 0.5f + (width * fillAmount), 0, 0);
                break;

            case UnityEngine.UI.Image.FillMethod.Vertical:
                localEndPos = new Vector3(0, -height * 0.5f + (height * fillAmount), 0);
                break;

            default:
                Debug.LogWarning("Bu kod sadece Horizontal ve Vertical için çalýþýr.");
                return Vector3.zero;
        }

        // Local pozisyonu dünya koordinatýna çevir
        return imageTransform.TransformPoint(localEndPos);
    }

}
