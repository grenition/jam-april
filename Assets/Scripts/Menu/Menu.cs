using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;

public class Menu : MonoBehaviour
{
    [SerializeField] private Transform _menuObject;
    [SerializeField] private Color _btnHoveredColor, _btnDefaultColor;
    [SerializeField] private AudioSource[] _audioSources;
    [SerializeField] private AudioClip _buttonHoveredClip, _buttonPressedClip, _openMenuClip;

    private int _audioSourceIndex;
    private KeyCode _openMenuKey = KeyCode.Escape;
    private Coroutine _hideMenuCor;

    public bool IsOpened { get; private set; }

    public void OnPointerEnterOnButton(Graphic btn)
    {
        btn.color = _btnHoveredColor;
        PlaySound(_buttonHoveredClip);
    }

    public void OnPointerExitFromButton(Graphic btn)
    {
        btn.color = _btnDefaultColor;
    }

    public void SettingsBtnPressed()
    {
        PlaySound(_buttonPressedClip);
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSourceIndex = (_audioSourceIndex + 1) % _audioSources.Length;
        _audioSources[_audioSourceIndex].Stop();
        _audioSources[_audioSourceIndex].clip = clip;
        _audioSources[_audioSourceIndex].Play();
    }

    public void OpenMenu()
    {
        PlaySound(_openMenuClip);
        if(_hideMenuCor != null)
        {
            StopCoroutine(_hideMenuCor);
        }
        _menuObject.gameObject.SetActive(true);
        _menuObject.DOScale(Vector3.one, .2f);
        IsOpened = true;
    }

    public void CloseMenu()
    {
        PlaySound(_openMenuClip);
        _menuObject.DOScale(new Vector3(0, 1, 1), .2f);
        IsOpened = false;
        if(_hideMenuCor != null)
        {
            StopCoroutine(_hideMenuCor);
        }
        _hideMenuCor = StartCoroutine(HideMenuIE());
    }

    private IEnumerator HideMenuIE()
    {
        yield return new WaitForSeconds(.3f);
        _menuObject.gameObject.SetActive(false);
    }

    public void QuitBtnPressed()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(_openMenuKey))
        {
            if(IsOpened)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }
}
