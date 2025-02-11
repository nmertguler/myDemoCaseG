using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrowAttack : MonoBehaviour
{
    [SerializeField] Transform arrowStartTransform;
    [SerializeField] float arrowHeight;
    

    public void ArrowAttackStart(Vector3 endPoint , float duration)
    {
        

        GameObject arrowTemp = PoolController.Instance.Arrow;
        arrowTemp.transform.position = arrowStartTransform.position;

        arrowTemp.GetComponent<SendArrow>().MoveArrow(arrowStartTransform.position,endPoint, arrowHeight, duration);
    }
}
