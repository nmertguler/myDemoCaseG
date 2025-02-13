using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Performance Settings")]
    public int targetFrameRate = 60;
    public int vSyncCount = 0;
    public bool runInBackground = true;

    private void Awake()
    {
        ApplySettings();
    }
    private void ApplySettings()
    {
        Application.targetFrameRate = targetFrameRate; // stabil fps rate
        QualitySettings.vSyncCount = vSyncCount; // V-Sync off
        Application.runInBackground = runInBackground;
    }
}
