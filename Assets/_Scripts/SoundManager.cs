using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource m_AudioSource;

    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(instance.gameObject);
    }
    public void PlaySong(AudioClip clip)
    {
        m_AudioSource.clip = clip;
        m_AudioSource.Play();
    }
}
