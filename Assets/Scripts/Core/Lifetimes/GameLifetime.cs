using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLifetime : MonoBehaviour
{ 
    public static GameLifetime Instance { get; protected set; }

    [SerializeField, Scene] private string _mainMenu;

    protected virtual void Awake()
    {
        Instance = this;
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
