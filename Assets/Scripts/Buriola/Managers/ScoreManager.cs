using UnityEngine;

namespace Buriola.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public int Score { get; private set; }
        public int Combo { get; private set; }
        public int GreatestCombo { get; private set; }

        public int NotePercentage { get; private set; }
        public int Multiplier { get; private set; }

        private int _noteStreak;
        private int _totalNotes;
        
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
            NotePercentage = 0;
            Score = 0;
            Combo = 0;
            Multiplier = 1;
            _totalNotes = 0;
            GreatestCombo = 0;
        }

        private void Start()
        {
            _totalNotes = NoteManager.Instance.Notes;
        }
        
        private void IncreaseCombo()
        {
            Combo++;
            if (GreatestCombo < Combo) GreatestCombo = Combo;
            
            onComboChange?.Invoke();
            
            UpdateMultiplier();
        }
        
        private void UpdateMultiplier()
        {
            if (Combo < 10)
                Multiplier = 1;
            else if (Combo < 30)
                Multiplier = 2;
            else if (Combo < 50)
                Multiplier = 4;
            else
                Multiplier = 8;
            
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
            
            NotePercentage = (int) (Mathf.Round((_noteStreak / (float) _totalNotes * 100f) * 10) / 10f);
            
            onPercentageChange?.Invoke();
        }
        
        private void ResetCombo()
        {
            Combo = 0;
            Multiplier = 1;
            
            onComboChange?.Invoke();
            onMultiplierChange?.Invoke();
        }
        
        public void IncreaseScore(int value)
        {
            Score += (value * Multiplier);
            
            onScoreChange?.Invoke();
        }
    }
}
