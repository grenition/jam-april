using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLifetime : MonoBehaviour
{ 
    public Player Player => _player;

    [SerializeField] protected Player _player;
    [SerializeField, Scene] private string _mainMenu;

    protected virtual void Awake()
    {
        ServiceLocator.Register<GameLifetime>(this);
    }
    protected virtual void OnDestroy()
    {
        ServiceLocator.Unregister<GameLifetime>();
    }

    public virtual void Play()
    {

    }
    public virtual void Stop()
    {

    }
    public virtual void ExitMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }
}
