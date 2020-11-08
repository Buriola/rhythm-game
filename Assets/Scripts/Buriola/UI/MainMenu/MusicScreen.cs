using UnityEngine;
using Buriola.Data;
using UnityEngine.Serialization;

namespace Buriola.UI.MainMenu
{
    public class MusicScreen : MonoBehaviour
    {
        [FormerlySerializedAs("buttonPrefab")] 
        [SerializeField] 
        private GameObject _buttonPrefab = null;
        
        [FormerlySerializedAs("highScorePrefab")]
        [SerializeField] 
        private GameObject _highScorePrefab = null;
        
        [FormerlySerializedAs("percentagePrefab")]
        [SerializeField]
        private GameObject _percentagePrefab = null;
        
        [FormerlySerializedAs("buttonRoot")]
        [SerializeField]
        private RectTransform _buttonRoot = null;
        
        [FormerlySerializedAs("highScoreRoot")]
        [SerializeField]
        private RectTransform _highScoreRoot = null;
        
        [FormerlySerializedAs("percentRoot")]
        [SerializeField] 
        private RectTransform _percentRoot = null;
        
        private Song[] _songs;

        private void Awake()
        {
            _songs = Resources.LoadAll<Song>("Data/");
        }

        private void Start()
        {
            SearchSongs();
        }
        
        private void SearchSongs()
        {
            foreach (Song t in _songs)
            {
                GameObject o = Instantiate(_buttonPrefab, _buttonRoot);
                o.GetComponent<MusicButton>().SongReference = t;
                o.GetComponentInChildren<UnityEngine.UI.Text>().text = t.ArtistName + " - " + t.SongName;

                GameObject o1 = Instantiate(_highScorePrefab, _highScoreRoot);
                o1.GetComponent<UnityEngine.UI.Text>().text =
                    "High score: " + GameController.LoadScore(t.SongName).ToString(); //Loading score

                GameObject o2 = Instantiate(_percentagePrefab, _percentRoot);
                o2.GetComponent<UnityEngine.UI.Text>().text =
                    "Percent: " + GameController.LoadPercent(t.SongName).ToString() +
                    "%"; //Loading hit percentage
            }
        }
    }
}
