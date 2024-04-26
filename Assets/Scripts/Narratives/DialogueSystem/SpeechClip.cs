using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project/DialogueSystem/SpeechClip")]
public class SpeechClip : ScriptableObject
{
    public AudioClip clip;
    [TextArea] public string subtitles;
}
