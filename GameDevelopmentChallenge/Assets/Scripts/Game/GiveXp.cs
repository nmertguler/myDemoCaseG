using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveXp : MonoBehaviour
{
    [SerializeField] XpBarController xpBarController;

    public void GiveXpFunc(Vector3 xpStartPos, float xpValue)
    {
        GameObject xpTemp = PoolController.Instance.Xp;

        xpTemp.transform.position = xpStartPos;
        Vector3 xpTargetPos = xpBarController.XpEffectTargetPos();
        float duration = Vector3.Distance(xpStartPos, xpTargetPos) / 35;

        xpTemp.transform.DOJump(xpTargetPos, 5, 0, duration).SetDelay(.1F).OnComplete(() =>
        {
            // give xp
            xpBarController.XpIncrease(xpValue);

            // return to pool
            PoolController.Instance.Xp = xpTemp;

        });
    }
}
