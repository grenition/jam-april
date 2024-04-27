using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class StartTaskOnAwake : MonoBehaviour
{
    [SerializeField] private Task _task;
    [SerializeField] private float _delay = 5f;

    private async void Awake()
    {
        await UniTask.WaitForSeconds(_delay);

        _task.StartTask();
    }
}
