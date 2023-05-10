using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
public class SoundtrackBehavior : MonoBehaviour
{
    [SerializeField] float fadeInTime = 5f;
    [SerializeField] float fadeOutTime = 5f;
    [SerializeField] AudioMixerSnapshot FullVolume;
    [SerializeField] AudioMixerSnapshot NoVolume;
    [SerializeField] AudioMixerSnapshot LowPassed;
    [SerializeField] AudioMixerSnapshot HiPassed;


    public static SoundtrackBehavior Instance;
    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void NormalSoundtrack()
    {
        FullVolume.TransitionTo(0.25f);
    }

    public void FadeInSoundtrack()
    {
        FullVolume.TransitionTo(fadeInTime);
    }

    public void FadeOutSoundtrack()
    {
        NoVolume.TransitionTo(fadeOutTime);
    }

    public void LowPassSoundtrack()
    {
        LowPassed.TransitionTo(0.25f);
    }

    public void HiPassSoundtrack()
    {
        HiPassed.TransitionTo(0.25f);
    }
}
