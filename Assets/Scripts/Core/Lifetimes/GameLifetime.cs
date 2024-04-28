using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum LooseReason
{
    die,
    redCapDie
}

public class GameLifetime : MonoBehaviour
{
    public UnityEvent<LooseReason> OnLoose;
    public bool Loosed { get; private set; }
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
    public virtual void Loose(LooseReason looseReason)
    {
        OnLoose?.Invoke(looseReason);
        Loosed = true;
        Pause();
    }
    public virtual void Pause()
    {
        Time.timeScale = 0.0f;
    }
    public virtual void Unpause()
    {
        Time.timeScale = 1.0f;
    }
    public virtual void ExitApplication()
    {
        Application.Quit();
    }
    public virtual void ExitMenu()
    {
        Unpause();
        SceneManager.LoadScene(_mainMenu);
    }
    public virtual void EndGame()
    {
        Unpause();
        SceneManager.LoadScene(2);
    }
    public virtual void ReloadGame()
    {
        Unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
