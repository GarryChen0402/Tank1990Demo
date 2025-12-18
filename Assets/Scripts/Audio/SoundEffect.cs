using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundEffect : ScriptableObject
{
    public AudioClip clip;
    public AudioClip GetAudioClip() => clip;
}
