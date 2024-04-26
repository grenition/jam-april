using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class UISubtitles : MonoBehaviour
{
    [SerializeField] private TMP_Text _subtitleText;
    [SerializeField] private CanvasGroup _subtitleGroup;
    [SerializeField] private float _charsPerSecond = 10f;
    [SerializeField] private float _subtitlesLifetime = 10f;
    [SerializeField] private float _fadeTime = 3f;

    private void Awake()
    {
        ServiceLocator.Register<UISubtitles>(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<UISubtitles>();
    }

    public void DrawSubtitles(string text)
    {
        ResetSubtitles();
        StartCoroutine(SubtitleLifetime(text));
    }
    public void ResetSubtitles()
    {
        _subtitleGroup.DOKill();
        _subtitleGroup.alpha = 0f;
        _subtitleGroup.gameObject.SetActive(false);

        StopAllCoroutines();
        _subtitleText.text = null;
    }

    private IEnumerator SubtitleLifetime(string text)
    {
        _subtitleGroup.DOKill();
        _subtitleGroup.alpha = 1f;
        _subtitleGroup.gameObject.SetActive(true);

        float delay = 1f / _charsPerSecond;
        string curText = "";

        foreach(var ch in text)
        {
            curText += ch;
            SetTextBackgrounded(curText);
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(_subtitlesLifetime);
        _subtitleGroup.DOFade(0f, _fadeTime).
            OnComplete(() => _subtitleGroup.gameObject.SetActive(false));
    }
    private void SetTextBackgrounded(string text)
    {
        _subtitleText.text = $"<mark=#202020A0>{text}</mark>";
    }
}
