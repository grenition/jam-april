using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLifetime : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
