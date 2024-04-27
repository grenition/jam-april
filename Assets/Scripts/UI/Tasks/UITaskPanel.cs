using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

public class UITaskPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private UITaskTab _taskTabPrefab;
    [SerializeField] private Transform _taskTabsRoot;
    [SerializeField] private float _fadeTime = 0.5f;

    private TaskController _taskController;

    private void Start()
    {
        _taskController = ServiceLocator.Get<TaskController>();
        if (_taskController == null)
            return;

        _canvasGroup.alpha = 0f;
        _taskController.OnTaskStarted.Bind(task => AddTask(task)).AddTo(this);
    }
    private async void AddTask(Task task)
    {
        _canvasGroup.DOFade(1f, _fadeTime);
        var taskTab = Instantiate(_taskTabPrefab, _taskTabsRoot);
        taskTab.SetupTab(task.TaskName, task.TaskDescription);

        void DestroyTab()
        {
            if(_taskController.RegisteredTasks.Where(task => task.IsTaskActive).Count() == 0)
            {
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(0f, _fadeTime);
            }
            taskTab.HideAndDestroy(async () =>
            {
                await UniTask.NextFrame();
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            });
        }

        task.OnTaskCompleted.Bind(() => DestroyTab()).AddTo(this);
        task.OnForceTaskStoppped.Bind(() => DestroyTab()).AddTo(this);

        await UniTask.NextFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
