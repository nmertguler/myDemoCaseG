using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WalkCommandManager : MonoBehaviour
{
    [SerializeField] LayerMask clickLayerMask;

    bool clickBlock = false;

    [SerializeField] Camera mainCam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && clickBlock == false)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, clickLayerMask))
            {
                // todo: move to hit.point
                LevelHolder.Instance.playerTower.MoveToPoint(hit.point);
            }
        }
    }
}
