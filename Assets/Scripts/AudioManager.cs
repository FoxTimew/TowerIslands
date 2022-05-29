using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private List<AudioSource> sources = new List<AudioSource>();
    [SerializeField] private AudioMixerGroup[] mixers;

    [SerializeField] private bool doNotDestroyOnLoad;

    private Coroutine coroutine;
    private void Awake()
    {
        if (AudioManager.instance != null)
        {
            Destroy(this);
            return;
        }
        if(doNotDestroyOnLoad) DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    private void OnDestroy()
    {
        AudioManager.instance = null;
    }

    #region Play
    public void Play(int soundIndex)
    {
        Play(soundIndex, 1, false, false);
    }

    public void Play(int soundIndex, float volume)
    {
        Play(soundIndex, volume, false, false);
    }

    public void Play(int soundIndex, bool loop)
    {
        Play(soundIndex, 1, loop, false);
    }
    
    public void Play(int soundIndex, bool loop, bool onlyOnce)
    {
        Play(soundIndex, 1, loop, onlyOnce);
    }

    public void Play(int soundIndex, float volume, bool loop, bool onlyOnce)
    {
        if (onlyOnce)
        {
            foreach (AudioSource a in sources)
            {
                if (a.clip == clips[soundIndex] && a.isPlaying) return;
                if ((a.clip == clips[5] || a.clip == clips[6] || a.clip == clips[7]) && a.isPlaying) return;
            }
        }

        AudioSource sourceSelected = null;
        if (loop)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            sourceSelected = sources[0];
            sourceSelected.outputAudioMixerGroup = mixers[0];
            if (soundIndex == 1) sourceSelected.loop = false;
            else sourceSelected.loop = true;
        }
        else
        {
            foreach (AudioSource a in sources)
            {
                if (!a.isPlaying && a != sources[0]) sourceSelected = a;
            }

            if (sourceSelected == null)
            {
                sourceSelected = this.AddComponent<AudioSource>();
                sources.Add(sourceSelected);
                
                Debug.LogWarning("Too much FX sounds playing.");
                //return;
            }
            sourceSelected.outputAudioMixerGroup = mixers[1];
        }

        sourceSelected.clip = clips[soundIndex];
        sourceSelected.volume = volume;
        sourceSelected.Play();
        if (soundIndex == 1) coroutine = StartCoroutine(MusicTransition());
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
    mixers[0].audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
}

public void SetEffectsVolume(float volume)
{
    mixers[0].audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
}

public void SetMasterVolume(float volume)
{
    mixers[0].audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
}

public void StopMusic()
{
    sources[0].Stop();
    if (coroutine != null)
    {
        StopCoroutine(coroutine);
        coroutine = null;
    }
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

IEnumerator MusicTransition()
{
    yield return new WaitUntil(() => !sources[0].isPlaying);
    sources[0].clip = clips[0];
    sources[0].loop = true;
    sources[0].Play();
    coroutine = null;
}
}
