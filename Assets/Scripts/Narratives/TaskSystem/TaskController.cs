using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

public class TaskController : MonoBehaviour
{
    public UnityEvent<Task> OnTaskStarted;
    public UnityEvent<Task> OnTaskCompleted;
    public UnityEvent<Task> OnTaskForceStopped;

    public IReadOnlyList<Task> RegisteredTasks => _registeredTasks.Values.ToList();

    private Dictionary<string, Task> _registeredTasks = new();
    private void Awake()
    {
        ServiceLocator.Register<TaskController>(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<TaskController>();
    }
    public void RegisterTask(Task task)
    {
        if (task == null)
            return;
        if (_registeredTasks.Keys.Contains(task.name))
        {
            Debug.LogError($"Task {task.name} already registered");
            return;
        }

        _registeredTasks.Add(task.name, task);

        task.OnTaskStart.Bind(() =>
        {
            OnTaskStarted?.Invoke(task);
        }).AddTo(this);

        task.OnForceTaskStoppped.Bind(() =>
        {
            OnTaskForceStopped?.Invoke(task);
        }).AddTo(this);

        task.OnTaskCompleted.Bind(() =>
        {
            OnTaskCompleted?.Invoke(task);
        }).AddTo(this);
    }
    public Task GetTask(string name)
    {
        if(_registeredTasks.ContainsKey(name))
            return _registeredTasks[name];
        return null;
    }
}
