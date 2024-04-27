using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(UIShowHideFade))]
public class UITaskTab : MonoBehaviour
{
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _descriptionText;

    private UIShowHideFade _fade;
    private void Awake()
    {
        _fade = GetComponent<UIShowHideFade>();
    }
    public void HideAndDestroy()
    {
        _fade.Hide(true);
    }
    public void SetupTab(string label, string description)
    {
        _labelText.SetText(label);
        _descriptionText.SetText(description); 
    }
}
