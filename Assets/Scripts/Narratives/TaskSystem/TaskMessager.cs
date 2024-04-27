using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMessager : MonoBehaviour
{
    private TaskController _controller;
    private void Start()
    {
        _controller = ServiceLocator.Get<TaskController>(); 
    }
}
