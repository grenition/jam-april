using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project/DialogueSystem/Speech")]
public class Speech : ScriptableObject
{
    [Serializable]
    public struct SpeechData
    {
        public float startTime;
        public SpeechClip speechClip;
    }

    public List<SpeechData> speeches;
}
