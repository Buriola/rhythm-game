using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to a the music button in the menu
/// </summary>
public class MusicButton : MonoBehaviour
{
    public Song songRef; // Stores a reference of a song

    /// <summary>
    /// Passes this reference to the controller when clicked
    /// </summary>
    public void SelectSong()
    {
        GameController.SetSelectedSong(songRef);
        GameController.Instance.LoadLevel("Game");
    }
}
