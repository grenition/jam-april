using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedCapTrigger : MonoBehaviour
{
    public UnityEvent<RedCap> OnRedCapEnter;
    public UnityEvent<RedCap> OnRedCapExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out RedCap redCap))
            return;
        OnRedCapEnter?.Invoke(redCap);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out RedCap redCap))
            return;
        OnRedCapExit?.Invoke(redCap);
    }
}
