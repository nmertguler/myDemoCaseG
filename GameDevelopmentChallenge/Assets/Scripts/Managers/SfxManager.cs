using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : Singleton<SfxManager>
{
    [SerializeField] List<AudioClip> sounds;
    [SerializeField] AudioSource _oneShotAudioSource;

    public void PlayClipOneShot(string soundName, float volume = 1)
    {
        AudioClip clip = sounds.Find(x => x.name == soundName);
        _oneShotAudioSource.PlayOneShot(clip, volume);
    }

    public void SetHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType preset)
    {
        Lofelt.NiceVibrations.HapticPatterns.PlayPreset(preset);
    }
}
