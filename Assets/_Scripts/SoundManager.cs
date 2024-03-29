using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource m_AudioSource;

    private void Start()
    {
        if(instance == null) {  instance = this; }
        DontDestroyOnLoad(instance.gameObject);
    }
    public void PlaySong(AudioClip clip)
    {
        m_AudioSource.clip = clip;
        m_AudioSource.Play();
    }
    public void BossStart()
    {
        m_AudioSource.loop = true;
    }
    public void BossEnd()
    {
        m_AudioSource.loop = false;
        float vol = m_AudioSource.volume;
        LeanTween.value(vol, 0, 2f).setOnUpdate((float val) =>
        {
            m_AudioSource.volume = val;
        });
        StartCoroutine(StopPlaying());
    }

    public void SetVolume(float volume)
    {
        m_AudioSource.volume = volume;
    }

    IEnumerator StopPlaying()
    {
        yield return new WaitForSeconds(2f);
        m_AudioSource.Stop();
    }
}
