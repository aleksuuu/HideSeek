using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


public class SFXBehavior : MonoBehaviour
{
    public static SFXBehavior Instance;
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

    public AudioClip EnemyLaugh;
    public AudioClip EnemyGroan;
    public AudioClip EnemyDeath;
    [SerializeField] AudioClip[] playerGroans;
    public AudioClip PlayerDeath;

    public AudioClip PlayerGroan
    {
        get
        {
            return playerGroans[Random.Range(0, playerGroans.Length)];
        }
        private set
        {
        }
    }
}
