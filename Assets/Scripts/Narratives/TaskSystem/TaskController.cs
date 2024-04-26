using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    private Dictionary<string, Task> _registeredTasks;

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
    }
    public Task GetTask(string name)
    {
        if(_registeredTasks.ContainsKey(name))
            return _registeredTasks[name];
        return null;
    }
}
