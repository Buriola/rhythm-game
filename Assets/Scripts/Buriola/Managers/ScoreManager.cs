using UnityEngine;

namespace Buriola.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        private int _score;
        private int _combo;
        private int _greatestCombo;

        private int _noteStreak;
        private int _totalNotes;
        private float _notePercentage;

        private int _multiplier;
        
        [SerializeField] private UnityEngine.Events.UnityEvent onScoreChange = default;
        [Space] [SerializeField] private UnityEngine.Events.UnityEvent onComboChange = default;
        [Space] [SerializeField] private UnityEngine.Events.UnityEvent onMultiplierChange = default;
        [Space] [SerializeField] private UnityEngine.Events.UnityEvent onPercentageChange = default;

        private void Awake()
        {
            Init();
        }
        
        private void Init()
        {
            _notePercentage = 0;
            _score = 0;
            _combo = 0;
            _multiplier = 1;
            _totalNotes = 0;
            _greatestCombo = 0;
        }

        private void Start()
        {
            _totalNotes = NoteManager.Instance.Notes();
        }
        
        private void IncreaseCombo()
        {
            _combo++;
            if (_greatestCombo < _combo) _greatestCombo = _combo;
            
            onComboChange?.Invoke();
            
            UpdateMultiplier();
        }
        
        private void UpdateMultiplier()
        {
            if (_combo < 10)
                _multiplier = 1;
            else if (_combo < 30)
                _multiplier = 2;
            else if (_combo < 50)
                _multiplier = 4;
            else
                _multiplier = 8;
            
            onMultiplierChange?.Invoke();
        }
        
        public void ComputeScore(bool streak)
        {
            if (streak)
            {
                _noteStreak++;
                IncreaseCombo();
                IncreaseScore(100);
            }
            else
                ResetCombo();
            
            _notePercentage = Mathf.Round(((float) _noteStreak / (float) _totalNotes * 100f) * 10) / 10f;
            
            onPercentageChange?.Invoke();
        }
        
        public void ResetCombo()
        {
            _combo = 0;
            _multiplier = 1;
            
            onComboChange?.Invoke();
            onMultiplierChange?.Invoke();
        }
        
        public void IncreaseScore(int value)
        {
            _score += (value * _multiplier);
            
            onScoreChange?.Invoke();
        }
        
        public int GetScore()
        {
            return _score;
        }

        public int GetCombo()
        {
            return _combo;
        }

        public int GetMultiplier()
        {
            return _multiplier;
        }

        public int GetPercentage()
        {
            if (_notePercentage <= 0)
                return 0;

            return Mathf.RoundToInt(_notePercentage);
        }

        public int GetTotalNotes()
        {
            return _totalNotes;
        }

        public int GetGreatestCombo()
        {
            return _greatestCombo;
        }
    }
}
