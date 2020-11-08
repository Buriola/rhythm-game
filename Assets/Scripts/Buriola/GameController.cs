using System.Collections;
using System.Collections.Generic;
using Buriola.Data;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// The Game Controller handles Scene Flow and Save System
/// </summary>
public class GameController : Singleton<GameController>
{
    //Reference of the selected song
    public static Song selectedSong;

    private void Awake()
    {
        selectedSong = null;

        if (Instance != null && Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Clears PlayerPrefs data, Utility script
    /// </summary>
    #if UNITY_EDITOR
    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    #endif

    /// <summary>
    /// Saves the highest score of the song played
    /// </summary>
    /// <param name="songName">Song name</param>
    /// <param name="score">The score to save</param>
    /// <param name="percentage">The hit percent to save</param>
    public static void SaveScore(string songName, int score, int percentage)
    {
        if(PlayerPrefs.HasKey(songName + "Score")) //Check if there is data saved
        {
            if(PlayerPrefs.GetInt(songName + "Score") < score) 
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

    /// <summary>
    /// Loads the score
    /// </summary>
    /// <param name="songName">The song name</param>
    /// <returns>The saved Score</returns>
    public static int LoadScore(string songName)
    {
        if(PlayerPrefs.HasKey(songName + "Score"))
        {
            return PlayerPrefs.GetInt(songName + "Score");
        }

        return 0;
    }

    /// <summary>
    /// Loads the hit percentage
    /// </summary>
    /// <param name="songName">Song name</param>
    /// <returns>The saved hit percentage</returns>
    public static int LoadPercent(string songName)
    {
        if (PlayerPrefs.HasKey(songName + "Percent"))
        {
            return PlayerPrefs.GetInt(songName + "Percent");
        }

        return 0;
    }

    /// <summary>
    /// Assign the selected song
    /// </summary>
    /// <param name="song">The song to select</param>
    public static void SetSelectedSong(Song song)
    {
        selectedSong = song;
    }

    /// <summary>
    /// Loads a level async
    /// </summary>
    /// <param name="levelName">Level ID</param>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
