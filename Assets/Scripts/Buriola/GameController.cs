using Buriola.Data;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Buriola
{
    public class GameController : Singleton<GameController>
    {
        public static Song selectedSong;

        private void Awake()
        {
            selectedSong = null;

            if (Instance != null && Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
        
        public static void SaveScore(string songName, int score, int percentage)
        {
            if (PlayerPrefs.HasKey(songName + "Score")) //Check if there is data saved
            {
                if (PlayerPrefs.GetInt(songName + "Score") < score)
                {
                    PlayerPrefs.SetInt(songName + "Score", score);
                    PlayerPrefs.SetInt(songName + "Percent", percentage);
                    PlayerPrefs.Save(); //Saves
                }
            }
            else
            {
                PlayerPrefs.SetInt(songName + "Score", score);
                PlayerPrefs.SetInt(songName + "Percent", percentage);
                PlayerPrefs.Save(); // Saves
            }

        }
        
        public static int LoadScore(string songName)
        {
            if (PlayerPrefs.HasKey(songName + "Score"))
            {
                return PlayerPrefs.GetInt(songName + "Score");
            }

            return 0;
        }
        
        public static int LoadPercent(string songName)
        {
            if (PlayerPrefs.HasKey(songName + "Percent"))
            {
                return PlayerPrefs.GetInt(songName + "Percent");
            }

            return 0;
        }
        
        public static void SetSelectedSong(Song song)
        {
            selectedSong = song;
        }
        
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadSceneAsync(levelName);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
