using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject pivot;
    [SerializeField] SpriteRenderer hpFillBar;
    [SerializeField] float sleepTime;

    [SerializeField] float maxWidth;

    float timer = 0;
    Transform mainCameraTransform;

    private void Update()
    {
        if(!pivot.activeInHierarchy)
        {
            // sleeping hp bar
            return;
        }

        if(mainCameraTransform == null)
        {
            mainCameraTransform = GameVariables.Instance.mainCamera.transform;
            return;
        }

        transform.LookAt(mainCameraTransform.position);
        timer += Time.deltaTime;

        if(timer >= sleepTime)
        {
            // sleep hp bar
            pivot.SetActive(false);
            timer = 0;
        }

    }

    public void HpBarUpdate(float fillValue)
    {
        timer = 0;
        pivot.SetActive(true);

        var spriteSize = hpFillBar.size;
        spriteSize.x = fillValue * maxWidth;

        hpFillBar.size = spriteSize;
    }
}
