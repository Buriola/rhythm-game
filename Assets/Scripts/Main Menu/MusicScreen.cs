using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class to handle the music selection screen
/// </summary>
public class MusicScreen : MonoBehaviour
{
    //Prefabs to instantiate in the grid
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private GameObject highScorePrefab;
    [SerializeField]
    private GameObject percentagePrefab;

    //Reference of the transforms
    [SerializeField]
    private RectTransform buttonRoot;
    [SerializeField]
    private RectTransform highScoreRoot;
    [SerializeField]
    private RectTransform percentRoot;

    //List of all songs in the project
    private List<Song> songs;

    private void Awake()
    {
        songs = new List<Song>();

        //Get all songs in the Resources/Data folder
        songs = Resources.LoadAll<Song>("Data/").ToList();
    }

    private void Start()
    {
        SearchSongs();
    }

    /// <summary>
    /// Instantiate the button and texts prefabs in the grids
    /// </summary>
    private void SearchSongs()
    { 
        for (int i = 0; i < songs.Count; i++)
        {
            GameObject o = Instantiate(buttonPrefab, buttonRoot);
            o.GetComponent<MusicButton>().songRef = songs[i];
            o.GetComponentInChildren<UnityEngine.UI.Text>().text = songs[i].artistName + " - " + songs[i].songName;

            GameObject o1 = Instantiate(highScorePrefab, highScoreRoot);
            o1.GetComponent<UnityEngine.UI.Text>().text =  "High score: " + GameController.LoadScore(songs[i].songName).ToString(); //Loading score

            GameObject o2 = Instantiate(percentagePrefab, percentRoot);
            o2.GetComponent<UnityEngine.UI.Text>().text = "Percent: " + GameController.LoadPercent(songs[i].songName).ToString() + "%"; //Loading hit percentage
        }
    }



}
