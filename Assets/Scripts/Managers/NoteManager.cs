using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

/// <summary>
/// Class to handle the notes and check if the game is over
/// </summary>
public class NoteManager : Singleton<NoteManager>
{
    [SerializeField]
    private Song song = null; //Reference of the current song being played

    [SerializeField]
    private NoteSpawner[] noteSpawners = null; //Reference of the spawners

    //Stores the period of time a song started
    public float Beginning { get; set; }

    //Length of the audioClip
    private float songLength;

    //List of all notes that will be ripped from the chart file
    private List<Note> redNotes;
    private List<Note> blueNotes;
    private List<Note> yellowNotes;

    private bool started; //Bool to ask if the song started
    private int notes; //Number of total notes in the file
    private bool songEnded; //Ask if all songs reached the end
    private bool gameOver; //Check if the game is over

    //Number of notes that were played
    public static int notesPlayed;

    private void Start()
    {
        Beginning = Time.time;
        songEnded = false;
        song = GameController.selectedSong; //Get reference of the selected song
        notesPlayed = 0;
        
        if (song != null && song.chartFile != null)
        {
            ReadSongChart(); //Read file and populate the lists
            InitSpawners(); //Initialize the note spawners
        }

        //Counts all notes
        notes = redNotes.Count + blueNotes.Count + yellowNotes.Count;
    }

    private void Update()
    {
        if(started)
            songEnded = notesPlayed == notes ? true : false; //Keep checking if the song has ended

        //If the song ended, we handle the game over, but if the game is over before the song ended... handle game over too.
        if ((songEnded && !gameOver) || gameOver)
            HandleGameOver();
    }

    /// <summary>
    /// Initializes the note spawners
    /// </summary>
    private void InitSpawners()
    {
        //Pass to each spawner the list of notes it will instantiate and a reference of this manager
        noteSpawners[0].Init(redNotes, this);
        noteSpawners[1].Init(blueNotes, this);
        noteSpawners[2].Init(yellowNotes, this);

        //Start song after spawners are initialized
        //At this point, the spawners will begin to spawn the notes, we need only to start the music
        StartCoroutine(StartSong());
    }

    /// <summary>
    /// Starts the gameplay after 1s
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartSong()
    {
        yield return new WaitForSeconds(1f);
        started = true;
        AudioManager.SetMusic(song.musicFile); //Set music
        songLength = AudioManager.GetMusicLength();
        yield return null;
    }

    /// <summary>
    /// Handle game over. It will show the game over menu after 0.5s
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(.5f);
        HUDManager.Instance.ShowGameOverMenu(); //Calls menu
    }

    /// <summary>
    /// This method will read the song chart file and rip all notes into separate lists
    /// </summary>
    private void ReadSongChart()
    {
        if (song.chartFile == null)
            return;

        System.String json = song.chartFile.text;

        IDictionary search = (IDictionary)Json.Deserialize(json); //Deserializes

        //Looking for the red notes
        IList red = (IList)search["A"];
        redNotes = new List<Note>(); //Populating red note list
        foreach (IDictionary note in red)
            redNotes.Add(new Note(System.Int32.Parse(note["start"].ToString()), System.Int32.Parse(note["length"].ToString())));

        //Blue Notes
        IList blue = (IList)search["B"];
        blueNotes = new List<Note>(); //Populating blue note list
        foreach (IDictionary note in blue)
            blueNotes.Add(new Note(System.Int32.Parse(note["start"].ToString()), System.Int32.Parse(note["length"].ToString())));

        //Yellow Notes
        IList yellow = (IList)search["C"];
        yellowNotes = new List<Note>(); //Populating yellow note list
        foreach (IDictionary note in yellow)
            yellowNotes.Add(new Note(System.Int32.Parse(note["start"].ToString()), System.Int32.Parse(note["length"].ToString())));
    }

    //Getters and setters
    public bool SongEnded()
    {
        return songEnded;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }

    /// <summary>
    /// Handles Game Over state
    /// </summary>
    public void HandleGameOver()
    {
        StartCoroutine(GameOver()); //Calls menu after a while
        gameOver = true;
        started = false;
        //Save Score and Percentage
        GameController.SaveScore(song.songName, ScoreManager.Instance.GetScore(), ScoreManager.Instance.GetPercentage());
        //Fades music
        AudioManager.FadeOutMusic(10f);
    }

    public int Notes()
    {
        return notes;
    }
}
