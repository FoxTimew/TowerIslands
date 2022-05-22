using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource[] sources;
    [SerializeField] private AudioMixerGroup[] mixers;

    [SerializeField] private bool doNotDestroyOnLoad;
    private void Awake()
    {
        if (AudioManager.instance != null)
        {
            Destroy(this);
            return;
        }
        if(doNotDestroyOnLoad) DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        AudioManager.instance = null;
    }

    #region Play
    public void Play(int soundIndex)
    {
        Play(soundIndex, 1, false);
    }

    public void Play(int soundIndex, float volume)
    {
        Play(soundIndex, volume, false);
    }

    public void Play(int soundIndex, bool loop)
    {
        Play(soundIndex, 1, loop);
    }

    public void Play(int soundIndex, float volume, bool loop)
    {
        AudioSource sourceSelected = null;
        if (loop)
        {
            sourceSelected = sources[sources.Length - 1];
            sourceSelected.outputAudioMixerGroup = mixers[0];
        }
        else
        {
            foreach (AudioSource a in sources)
            {
                if (!a.isPlaying && a != sources[sources.Length - 1]) sourceSelected = a;
            }

            if (sourceSelected == null)
            {
                Debug.LogWarning("Too much FX sounds playing. Cut some sounds or add AudioSource on AudioManager");
                return;
            }
            sourceSelected.outputAudioMixerGroup = mixers[1];
        }

        sourceSelected.clip = clips[soundIndex];
        sourceSelected.volume = volume;
        sourceSelected.Play();
    }
#endregion

public bool IsPlaying()
{
    foreach (AudioSource a in sources)
    {
        if (a.isPlaying) return true;
    }
    return false;
}

public void SetMusicVolume(float volume)
{
    sources[sources.Length - 1].volume = volume;
}

public void StopMusic()
{
    sources[sources.Length - 1].Stop();
}

public void StopSFX(int soundIndex)
{
    foreach (AudioSource s in sources)
    {
        if (s.clip == clips[soundIndex])
        {
            s.Stop();
            return;
        }
    }
    Debug.Log(clips[soundIndex].name + " SFX not found in any AudioSource components in AudioManager. Impossible to stop it.");
}
}
