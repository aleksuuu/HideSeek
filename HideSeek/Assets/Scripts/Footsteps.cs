using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    // Modified from https://www.youtube.com/watch?v=Bnm8mzxnwP8

    [SerializeField] AudioClip[] clips;
    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Step()
    {
        audioSource.PlayOneShot(GetRandomClip());
    }

    AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
