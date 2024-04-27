using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
public class UILoosePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private TMP_Text _looseReasonText;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private string _dieReasonLine;
    [SerializeField] private string _redCapDieReasonLine;

    private GameLifetime _lifetime;
    private void Start()
    {
        _lifetime = ServiceLocator.Get<GameLifetime>();
        _panel.gameObject.SetActive(false);
        _lifetime.OnLoose.Bind(reason =>
        {
            switch (reason)
            {
                case LooseReason.die:
                    _looseReasonText.text = _dieReasonLine;
                    break;
                case LooseReason.redCapDie:
                    _looseReasonText.text = _redCapDieReasonLine;
                    break;
            }
            Show();
        }).AddTo(this);
    }

    public void Show()
    {
        _panel.gameObject.SetActive(true);
        _panel.alpha = 0f;

        _panel.DOKill();
        _panel.DOFade(1f, _fadeTime).SetUpdate(true);
    }
    public void Hide()
    {
        _panel.DOKill();
        _panel.DOFade(0f, _fadeTime)
            .OnComplete(() => _panel.gameObject.SetActive(false));
    }
    public void ReloadGame()
    {
        _lifetime.ReloadGame();
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
