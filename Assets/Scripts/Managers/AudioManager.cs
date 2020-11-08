using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle the sounds of the game
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    //Two audio sources
    [SerializeField]
    private AudioSource musicAudio;
    [SerializeField]
    private AudioSource sfxAudio;

    private static float musicVolume;
    private static float sfxVolume;

    private void Start()
    {
        musicVolume = musicAudio.volume;
        sfxVolume = sfxAudio.volume;
    }

    #region Static API

    /// <summary>
    /// Plays audio
    /// </summary>
    public static void PlayMusic()
    {
        Instance.musicAudio.Play();
    }

    /// <summary>
    /// Initializes a coroutine to fade the audio given the amount of time
    /// </summary>
    /// <param name="fadeOutTime">Time in seconds to fade</param>
    public static void FadeOutMusic(float fadeOutTime)
    {
        Instance.StartCoroutine(Instance.FadeOut(Instance.musicAudio, fadeOutTime));
    }

    /// <summary>
    /// Plays SFX
    /// </summary>
    public static void PlaySFX()
    {
        Instance.sfxAudio.Play();
    }

    /// <summary>
    /// Pause audio
    /// </summary>
    public static void PauseMusic()
    {
        Instance.musicAudio.Pause();
    }

    /// <summary>
    /// Stops Audio
    /// </summary>
    public static void StopMusic()
    {
        Instance.musicAudio.Stop();
    }

    /// <summary>
    /// Returns the length of the current AudioClip in the Audio Source
    /// </summary>
    /// <returns></returns>
    public static float GetMusicLength()
    {
        return Instance.musicAudio.clip.length;
    }

    /// <summary>
    /// Assign an audio clip to the Audio Source and plays it
    /// </summary>
    /// <param name="clip"></param>
    public static void SetMusic(AudioClip clip)
    {
        Instance.musicAudio.clip = clip;
        Instance.musicAudio.Play();
        Instance.musicAudio.volume = musicVolume;
    }

    #endregion
    
    /// <summary>
    /// Fades the audio
    /// </summary>
    /// <param name="audioS"></param>
    /// <param name="fadeOutTime"></param>
    /// <returns></returns>
    private IEnumerator FadeOut(AudioSource audioS, float fadeOutTime)
    {
        float startVolume = audioS.volume;

        while(audioS.volume > 0)
        {
            audioS.volume -= startVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }

        audioS.Stop();
        audioS.volume = startVolume;
    }
}
