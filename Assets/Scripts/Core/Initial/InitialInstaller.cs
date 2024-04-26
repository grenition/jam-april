using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialInstaller
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        //setup root components
        Debug.Log("Initial load");  
    }
}
