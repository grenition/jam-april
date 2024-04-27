using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialInstaller
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var rootLifetime = Resources.Load<RootLifetime>("Lifetimes/RootLifetime");
        ServiceLocator.Register<RootLifetime>(Object.Instantiate(rootLifetime));
    }
}
