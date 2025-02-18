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
        if (Time.timeScale == 0 || clickBlock || !Input.GetKeyDown(KeyCode.Mouse0))
            return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 500, clickLayerMask))
        {
            HandleMoveAction(hit.point);
        }


    }

    private void HandleMoveAction(Vector3 targetPosition)
    {
        // move to position
        LevelHolder.Instance.GetPlayerTower().MoveToPoint(targetPosition);

        // flag animation
        targetFlag.transform.position = targetPosition;
        targetFlagAnimator.Play("FlagMove");

        // haptic
        HandleHapticFeedback(); 
    }

    private void HandleHapticFeedback()
    {
        SfxManager.Instance.SetHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType.LightImpact);
    }


}
