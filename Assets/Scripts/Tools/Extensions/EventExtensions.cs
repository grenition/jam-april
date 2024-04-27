using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Events;
using UnityEngine.UI;

public static class EventExtensions
{
    public static IDisposable Bind(this UnityEvent unityEvent, UnityAction actions)
    {
        unityEvent.AddListener(actions);

        var disposable = new CancellationDisposable();
        disposable.Token.Register(() =>
        {
            unityEvent.RemoveListener(actions);
        });

        return disposable;
    }
        
    public static IDisposable Bind<T>(this UnityEvent<T> unityEvent, UnityAction<T> actions)
    {
        unityEvent.AddListener(actions);

        var disposable = new CancellationDisposable();
        disposable.Token.Register(() =>
        {
            unityEvent.RemoveListener(actions);
        });

        return disposable;
    }
    public static IDisposable Bind<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> actions)
    {
        unityEvent.AddListener(actions);

        var disposable = new CancellationDisposable();
        disposable.Token.Register(() =>
        {
            unityEvent.RemoveListener(actions);
        });

        return disposable;
    }

    public static IDisposable Bind(this Button button, UnityAction actions)
    {
        button.onClick.AddListener(actions);

        var disposable = new CancellationDisposable();
        disposable.Token.Register(() =>
        {
            button.onClick.RemoveListener(actions);
        });

        return disposable;
    }
}