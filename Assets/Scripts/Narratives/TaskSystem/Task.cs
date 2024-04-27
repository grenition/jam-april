using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    public UnityEvent OnTaskStart;
    public UnityEvent OnTaskCompleted;
    public UnityEvent OnForceTaskStoppped;

    [field: SerializeField] public string TaskName { get; protected set; }
    [field: SerializeField] public string TaskDescription { get; protected set; }
    public bool IsTaskActive { get; protected set; }

    private void Start()
    {
        var taskController = ServiceLocator.Get<TaskController>();
        taskController?.RegisterTask(this);
    }

    public virtual void StartTask()
    {
        IsTaskActive = true;
        OnTaskStart?.Invoke();
    }
    public virtual void CompleteTask()
    {
        IsTaskActive = false;
        OnTaskCompleted?.Invoke();
    }
    public virtual void ForceStopTask()
    {
        IsTaskActive = false;
        OnForceTaskStoppped?.Invoke();
    }
}
