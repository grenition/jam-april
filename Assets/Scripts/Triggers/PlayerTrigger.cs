using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent<Player> OnPlayerEnter;
    public UnityEvent<Player> OnPlayerExit;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Player player))
            return;
        OnPlayerTriggerEnter(player);
        OnPlayerEnter?.Invoke(player);
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Player player))
            return;
        OnPlayerTriggerExit(player);
        OnPlayerExit?.Invoke(player);
    }

    protected virtual void OnPlayerTriggerEnter(Player player)
    {

    }
    protected virtual void OnPlayerTriggerExit(Player player)
    {

    }
}
