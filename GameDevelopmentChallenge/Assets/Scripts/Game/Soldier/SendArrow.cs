using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SendArrow : MonoBehaviour
{
    public void MoveArrow(Vector3 startPoint , Vector3 endPoint , float height , float duration)
    {
        Vector3 midPoint = (startPoint + endPoint) / 2;
        midPoint.y += height; // for parabolic

        Vector3[] path = new Vector3[] { startPoint, midPoint, endPoint };

        transform.DOPath(path, duration, PathType.CatmullRom)
                 .SetEase(Ease.Linear)
                 .SetLookAt(0.01f).OnComplete( ()=>
                 {
                     // complete
                     PoolController.Instance.Arrow = gameObject;
                 }); 
    }
}
