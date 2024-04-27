using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private float _fadeTime = 0.5f;

    private bool _isPaused = false;
    private GameLifetime _lifetime;
    private void Start()
    {
        _lifetime = ServiceLocator.Get<GameLifetime>();
        _panel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;

            if (!_isPaused)
               Unpause();
            else
               Pause();
        }
    }
    public void Pause()
    {
        _isPaused = true;

        _panel.gameObject.SetActive(true);
        _panel.alpha = 0f;

        _panel.DOKill();
        _panel.DOFade(1f, _fadeTime).SetUpdate(true);

        _lifetime.Pause();
    }
    public void Unpause()
    {
        _isPaused = false;

        _panel.DOKill();
        _panel.DOFade(0f, _fadeTime)
            .OnComplete(()=>_panel.gameObject.SetActive(false));

        _lifetime.Unpause();
    }
    public void ExitMenu()
    {
        _lifetime.ExitMenu();
    }
    public void ExitGame()
    {
        _lifetime.ExitApplication();
    }
}
