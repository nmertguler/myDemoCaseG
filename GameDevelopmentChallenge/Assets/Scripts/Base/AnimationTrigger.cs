using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent TriggerEvent;

    public void TriggerFunc()
    {
        TriggerEvent?.Invoke();
    }
}
