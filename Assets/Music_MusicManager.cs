using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    public List<AudioClip> audioClip = new List<AudioClip>();
    private int musicIndex;
    private int currentMusicIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        musicIndex = 0;
        currentMusicIndex = 0;
    }

    public void UpdateMusic(List<AudioClip> clip)
    {
        audioClip = clip;
        ResetMusic();
    }


    private void ResetMusic()
    {
        audioSource.Stop();
    }

    /*
    private IEnumerator turnDownVolume()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.01f;
            yield return null;
        }
    }
    */

    private void Update()
    {
        if (audioClip == null)
            return;

        if (!audioSource.isPlaying && currentMusicIndex == musicIndex)
        {
            audioSource.clip = audioClip[musicIndex];
            audioSource.Play();
        }
    }
}
