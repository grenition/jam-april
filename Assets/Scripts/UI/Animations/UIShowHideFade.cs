using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class UIShowHideFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _showAnimationOnAwake = true;
    [SerializeField] private bool _disableAfterHide = true;
    [SerializeField] private float _fadeTime = 0.5f;

    private void OnEnable()
    {
        if (_showAnimationOnAwake)
            Show();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, _fadeTime);
    }
    public void Hide(bool destroyAfteerFade = false, Action onComplete = default)
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, _fadeTime)
            .OnComplete(() =>
            {
                if (_disableAfterHide)
                    gameObject.SetActive(false);
                if(destroyAfteerFade)
                    Destroy(gameObject);
                onComplete?.Invoke();
            });
    }
}
