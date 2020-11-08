using Buriola.Managers;
using Buriola.NoteBehaviors;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.NoteBehaviors
{
    public class TrackInput : MonoBehaviour
    {
        #region Variables

        [SerializeField] private bool _checkForInput = true;
        
        [FormerlySerializedAs("inputKey")] 
        [SerializeField] 
        private KeyCode _inputKey = default;
        
        private float _defaultY;
        private const float DESIRED_Y = -2.5f;

        private bool _isPlaying;
        private bool _isValidToPlay;

        private float _timer;

        private GameObject _lastNote;
        private NoteBehaviour _note;

        #endregion

        private void Start()
        {
            _defaultY = transform.position.y;
            _timer = 0;
        }

        private void Update()
        {
            if (_checkForInput)
            {
                PressButton();
                TrackNoteHits();
            }
            
            if (NoteManager.Instance.IsGameOver() && _checkForInput)
                _checkForInput = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            _isValidToPlay = true;
            _timer = 0;
            _lastNote = other.gameObject;
            _note = _lastNote.GetComponent<NoteBehaviour>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (_isValidToPlay && _note.Length > 0.2f && IsNotePlaying() && _timer < .2f)
            {
                _note.SetColor(Color.green);
                ScoreManager.Instance.IncreaseScore(1);
            }
            else if (_isValidToPlay && !IsNotePlaying() && _timer < .2f)
            {
                _timer += Time.deltaTime;
                _note.SetToOriginalColor();
            }
            else
            {
                _isValidToPlay = false;
                _note.Played = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _isValidToPlay = false;
            _lastNote.SendMessage("Finish", SendMessageOptions.DontRequireReceiver);
            
            if (!_note.Played)
            {
                HealthManager.Instance.DecreaseHP(10f);
                ScoreManager.Instance.ComputeScore(false);
            }
        }
        
        
        private void PressButton()
        {
            if (Input.GetKey(_inputKey) && transform.position.y != DESIRED_Y)
            {
                _isPlaying = true;
                //Change Y position of the object for feedback
                transform.position =
                    new Vector3(transform.position.x, DESIRED_Y, transform.position.z);
            }
            else if (Input.GetKeyUp(_inputKey))
            {
                _isPlaying = false;
                transform.position =
                    new Vector3(transform.position.x, _defaultY, transform.position.z);
            }
        }

        private void TrackNoteHits()
        {
            if (_isValidToPlay && IsNotePlaying())
            {
                if (_lastNote != null && _note.Length <= 0.2f && !_note.Played)
                {
                    _note.Played = true;
                    HealthManager.Instance.AddHP(3f);
                    ScoreManager.Instance.ComputeScore(true);
                    _lastNote.SendMessage("Finish", SendMessageOptions.DontRequireReceiver);
                }
                else if (_lastNote != null && _note.Length > 0.2f && !_note.Played)
                {
                    _note.Played = true;
                    HealthManager.Instance.AddHP(3f);
                    ScoreManager.Instance.ComputeScore(true);
                }
            }

            if (!_isValidToPlay && Input.GetKeyDown(_inputKey))
            {
                ScoreManager.Instance.ComputeScore(false);
                HealthManager.Instance.DecreaseHP(5f);
            }

        }

        private bool IsNotePlaying()
        {
            return _isPlaying;
        }
    }
}
