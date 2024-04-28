using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _graphicsDropDown;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider, _sfxSlider, _musicSlider, _voiceSlider;

    public const string MASTER_VOLUME = "MasterVolume";
    public const string SFX_VOLUME = "SFXVolume";
    public const string MUSIC_VOLUME = "MusicVolume";
    public const string VOICE_VOLUME = "VoiceVolume";

    public void GraphicsDropdownUpdated(int index)
    {
        if(QualitySettings.GetQualityLevel() != index)
        {
            QualitySettings.SetQualityLevel(index);
        }
    }

    private void Start()
    {
        _graphicsDropDown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        if (_audioMixer.GetFloat(MASTER_VOLUME, out float value))
        {
            _masterSlider.value = value;
        }
        if (_audioMixer.GetFloat(SFX_VOLUME, out value))
        {
            _sfxSlider.value = value;
        }
        if (_audioMixer.GetFloat(MUSIC_VOLUME, out value))
        {
            _musicSlider.value = value;
        }
        if(_audioMixer.GetFloat(VOICE_VOLUME, out value))
        {
            _voiceSlider.value = value;
        }
    }

    public void OnDropdownClick()
    {
        StartCoroutine(DropdownIE());
    }

    private IEnumerator DropdownIE()
    {
        yield return null;
        var go = GameObject.Find("Dropdown List");
        if(go != null)
        {
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 140);
        }
    }

    public void MasterVolumeChanged(float value)
    {
        _audioMixer.SetFloat(MASTER_VOLUME, value);
    }

    public void SFXVolumeChanged(float value)
    {
        _audioMixer.SetFloat(SFX_VOLUME, value);
    }

    public void MusicVolumeChanged(float value)
    {
        _audioMixer.SetFloat(MUSIC_VOLUME, value);
    }

    public void VoiceVolumeChanged(float value)
    {
        _audioMixer.SetFloat(VOICE_VOLUME, value);
    }
}
