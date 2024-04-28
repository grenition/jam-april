using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIComebackToRedCapPanel : MonoBehaviour
{
    public bool Active { get; set; }

    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private float _returnTime = 30f;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private float _distance = 35f;

    private bool _outOfZone = false;
    private RedCapLifetime _lifetime;

    private void Start()
    {
        _panel.alpha = 0f;
        _lifetime = ServiceLocator.Get<RedCapLifetime>();
    }
    private void Awake()
    {
        ServiceLocator.Register<UIComebackToRedCapPanel>(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<UIComebackToRedCapPanel>();
    }
    private void Update()
    {
        bool outOfZone = (Vector3.Distance(_lifetime.Player.transform.position, 
            _lifetime.RedCap.transform.position) > _distance) && Active;

        if(outOfZone != _outOfZone)
        {
            _outOfZone = outOfZone;

            if (_outOfZone)
            {
                StartCoroutine(LooseCorutine());
                Show();
            }
            else
            {
                StopAllCoroutines();
                Hide();
            }
        }
    }
    private IEnumerator LooseCorutine()
    {
        yield return new WaitForSeconds(_returnTime);
        _lifetime.Loose(LooseReason.redCapDie);
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
}
