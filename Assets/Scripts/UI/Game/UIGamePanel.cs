using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePanel : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register<UIGamePanel>(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<UIGamePanel>();
    }
}
