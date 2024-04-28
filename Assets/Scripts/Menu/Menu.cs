using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private Transform _menuObject;
    [SerializeField] private Color _btnHoveredColor, _btnDefaultColor;
    [SerializeField] private AudioSource[] _audioSources;
    [SerializeField] private AudioClip _buttonHoveredClip, _buttonPressedClip, _openMenuClip;
    [SerializeField] private GameObject[] _screens;
    [SerializeField] private GameObject _blackScreen;

    private int _audioSourceIndex;

    public bool IsOpened { get; private set; }

    private void Start()
    {
        foreach(var screen in _screens)
        {
            screen.SetActive(false);
        }
        _screens[0].SetActive(true);
        StartCoroutine(StartIE());
    }

    private IEnumerator StartIE()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        _blackScreen.SetActive(false);
    }

    public void OnPointerEnterOnButton(Graphic btn)
    {
        btn.color = _btnHoveredColor;
        PlaySound(_buttonHoveredClip);
    }

    public void OnPointerExitFromButton(Graphic btn)
    {
        btn.color = _btnDefaultColor;
    }

    public void OpenScreenBtnPressed(GameObject screen)
    {
        foreach(var scrn in _screens)
        {
            scrn.SetActive(false);
        }
        screen.SetActive(true);
        PlaySound(_buttonPressedClip);
    }

    public void ChooseDifficulty(int index)
    {
        Difficulty.SetDifficulty(index);
        SceneManager.LoadScene(1);
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSourceIndex = (_audioSourceIndex + 1) % _audioSources.Length;
        _audioSources[_audioSourceIndex].Stop();
        _audioSources[_audioSourceIndex].clip = clip;
        _audioSources[_audioSourceIndex].Play();
    }

    public void QuitBtnPressed()
    {
        Application.Quit();
    }
}
