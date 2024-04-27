using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RedCap : MonoBehaviour
{
    public static RedCap Instance { get; private set; }
    public NavMeshAgent Agent => _agent;

    
    private NavMeshAgent _agent;
    
    private void Awake()
    {
        Instance = this;
        _agent = GetComponent<NavMeshAgent>();
    }
}
