using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    public UnityEvent OnTaskStart;
    public UnityEvent OnTaskFinish;
    public UnityEvent OnTaskStoppped;

    [field: SerializeField] public string TaskName { get; protected set; } 

    public virtual void StartTask()
    {

    }
    public virtual void CompleteTask()
    {

    }
    public virtual void StopTask()
    {

    }
}
