using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WalkCommandManager : MonoBehaviour
{
    [SerializeField] LayerMask clickLayerMask;

    [Space(5)]
    [SerializeField] Camera mainCam;

    [Space(5)]
    [SerializeField] GameObject targetFlag;
    [SerializeField] Animator targetFlagAnimator;

    // privates
    bool clickBlock = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && clickBlock == false)
        {
            if(Time.timeScale == 0)
            {
                return;
            }

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, clickLayerMask))
            {
                // todo: move to hit.point
                LevelHolder.Instance.playerTower.MoveToPoint(hit.point);
                targetFlag.transform.position = hit.point;
                targetFlagAnimator.Play("FlagMove");
                SfxManager.Instance.SetHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType.LightImpact);
            }
        }
    }
}
