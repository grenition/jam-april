using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project/DialogueSystem/SpeechClip")]
public class SpeechClip : ScriptableObject
{
    public AudioClip[] clips;
    [TextArea] public string subtitles;
}
