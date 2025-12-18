using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonClass<AudioManager>
{
    public SoundEffect[] effects;
    private Dictionary<string, SoundEffect> _dictionaryEffects;

    //public AudioSource currentBgm;
    private AudioListener _lintener;
    protected override void Awake()
    {
        base.Awake();
        _dictionaryEffects = new Dictionary<string, SoundEffect>();
        foreach (SoundEffect soundEffect in effects)
        {
            _dictionaryEffects[soundEffect.name] = soundEffect;
        }
    }

    public void PlayFx(string name, Vector3 position, float volume)
    {
        if (!_dictionaryEffects.ContainsKey(name))
        {
            Debug.LogError("Do not have effect named " + name);
            return;
        }
        SoundEffect effect = _dictionaryEffects[name];
        AudioSource.PlayClipAtPoint(effect.GetAudioClip(), position, volume);
    }

    public void PlayFx(string name, float volume = 0.5f)
    {
        if (_lintener == null) _lintener = FindFirstObjectByType<AudioListener>();
        PlayFx(name, _lintener.transform.position, volume);
    }
}
