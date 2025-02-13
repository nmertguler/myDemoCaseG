using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    public float refreshRate = 0.5f; // fps rate
    private int frameCount;
    private float deltaTime;
    private float fps;

    public TextMeshProUGUI fpsText; // text

    private void Start()
    {
        InvokeRepeating(nameof(UpdateFPS), refreshRate, refreshRate);
    }

    private void Update()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;
    }

    private void UpdateFPS()
    {
        fps = frameCount / deltaTime;
        frameCount = 0;
        deltaTime = 0;

        if (fpsText != null)
            fpsText.text = $"FPS: {fps:F1}";
    }

}
