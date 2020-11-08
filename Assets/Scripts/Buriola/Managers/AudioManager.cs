using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [FormerlySerializedAs("musicAudio")] 
        [SerializeField] 
        private AudioSource _musicAudio = null;

        private static float _musicVolume;

        private void Start()
        {
            _musicVolume = _musicAudio.volume;
        }

        #region Static API
        
        public static void FadeOutMusic(float fadeOutTime)
        {
            Instance.StartCoroutine(Instance.FadeOut(Instance._musicAudio, fadeOutTime));
        }
        
        public static void SetMusic(AudioClip clip)
        {
            Instance._musicAudio.clip = clip;
            Instance._musicAudio.Play();
            Instance._musicAudio.volume = _musicVolume;
        }

        #endregion
        
        private IEnumerator FadeOut(AudioSource audioS, float fadeOutTime)
        {
            float startVolume = audioS.volume;

            while (audioS.volume > 0)
            {
                audioS.volume -= startVolume * Time.deltaTime / fadeOutTime;
                yield return null;
            }

            audioS.Stop();
            audioS.volume = startVolume;
        }
    }
}
