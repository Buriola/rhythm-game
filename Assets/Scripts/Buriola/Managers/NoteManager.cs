using System.Collections;
using System.Collections.Generic;
using Buriola.Data;
using Buriola.NoteBehaviors;
using Buriola.UI;
using UnityEngine;
using MiniJSON;
using UnityEngine.Serialization;

namespace Buriola.Managers
{
    public class NoteManager : Singleton<NoteManager>
    {
        [SerializeField] private Song _song = null;

        [FormerlySerializedAs("noteSpawners")] 
        [SerializeField] 
        private NoteSpawner[] _noteSpawners = null;
        
        public float Beginning { get; set; }
        
        private List<Note> _redNotes;
        private List<Note> _blueNotes;
        private List<Note> _yellowNotes;

        private bool _started;
        private int _notes;
        private bool _songEnded;
        private bool _gameOver;
        
        public static int NotesPlayed;

        private void Start()
        {
            Beginning = Time.time;
            _songEnded = false;
            _song = GameController.selectedSong;
            NotesPlayed = 0;

            if (_song != null && _song.ChartFile != null)
            {
                ReadSongChart();
                InitSpawners();
            }
            
            _notes = _redNotes.Count + _blueNotes.Count + _yellowNotes.Count;
        }

        private void Update()
        {
            if (_started)
                _songEnded = NotesPlayed == _notes;
            
            if ((_songEnded && !_gameOver) || _gameOver)
                HandleGameOver();
        }
        
        private void InitSpawners()
        {
            _noteSpawners[0].Init(_redNotes, this);
            _noteSpawners[1].Init(_blueNotes, this);
            _noteSpawners[2].Init(_yellowNotes, this);
            
            StartCoroutine(StartSong());
        }
        
        private IEnumerator StartSong()
        {
            yield return new WaitForSeconds(1f);
            _started = true;
            AudioManager.SetMusic(_song.MusicFile); //Set music
            AudioManager.GetMusicLength();
            yield return null;
        }
        
        private IEnumerator GameOver()
        {
            yield return new WaitForSeconds(.5f);
            HUDManager.Instance.ShowGameOverMenu(); //Calls menu
        }
        
        private void ReadSongChart()
        {
            if (_song.ChartFile == null)
                return;

            System.String json = _song.ChartFile.text;

            IDictionary search = (IDictionary) Json.Deserialize(json); //Deserializes

            //Looking for the red notes
            IList red = (IList) search["A"];
            _redNotes = new List<Note>(); //Populating red note list
            foreach (IDictionary note in red)
                _redNotes.Add(new Note(System.Int32.Parse(note["start"].ToString()),
                    System.Int32.Parse(note["length"].ToString())));

            //Blue Notes
            IList blue = (IList) search["B"];
            _blueNotes = new List<Note>(); //Populating blue note list
            foreach (IDictionary note in blue)
                _blueNotes.Add(new Note(System.Int32.Parse(note["start"].ToString()),
                    System.Int32.Parse(note["length"].ToString())));

            //Yellow Notes
            IList yellow = (IList) search["C"];
            _yellowNotes = new List<Note>(); //Populating yellow note list
            foreach (IDictionary note in yellow)
                _yellowNotes.Add(new Note(System.Int32.Parse(note["start"].ToString()),
                    System.Int32.Parse(note["length"].ToString())));
        }
        
        public bool SongEnded()
        {
            return _songEnded;
        }

        public bool IsGameOver()
        {
            return _gameOver;
        }

        public void SetGameOver(bool value)
        {
            _gameOver = value;
        }
        
        public void HandleGameOver()
        {
            StartCoroutine(GameOver()); //Calls menu after a while
            _gameOver = true;
            _started = false;
            //Save Score and Percentage
            GameController.SaveScore(_song.SongName, ScoreManager.Instance.GetScore(),
                ScoreManager.Instance.GetPercentage());
            //Fades music
            AudioManager.FadeOutMusic(10f);
        }

        public int Notes()
        {
            return _notes;
        }
    }
}
