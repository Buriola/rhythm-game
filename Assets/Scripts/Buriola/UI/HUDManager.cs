using Buriola.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Buriola.UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        [FormerlySerializedAs("gameOverMenu")]
        [SerializeField]
        private Canvas _gameOverMenu = null;
        
        [FormerlySerializedAs("hudText")]
        [SerializeField]
        private Text _hudText = null;
        
        [FormerlySerializedAs("healthSlider")]
        [SerializeField] 
        private Slider _healthSlider = null;
        
        [FormerlySerializedAs("healthImage")]
        [SerializeField]
        private Image _healthImage = null;

        [FormerlySerializedAs("titleText")]
        [Space] 
        [SerializeField] private Text _titleText = null;
        
        [FormerlySerializedAs("scoreText")]
        [SerializeField] 
        private Text _scoreText = null;
        
        [FormerlySerializedAs("comboText")]
        [SerializeField]
        private Text _comboText = null;
        
        [FormerlySerializedAs("percentText")] 
        [SerializeField]
        private Text _percentText = null;


        private void Start()
        {
            UpdateHUDText();
            UpdateHealth();
        }
        
        public void UpdateHUDText()
        {
            if (_hudText != null)
            {
                string s = ScoreManager.Instance.GetMultiplier().ToString() + "X\n" +
                           ScoreManager.Instance.GetScore().ToString() + "\n" +
                           ScoreManager.Instance.GetCombo().ToString() + "HITS!";
                s = s.Replace("\n", System.Environment.NewLine);

                _hudText.text = s;
            }
        }
        
        private void UpdateGameOverMenu()
        {
            _titleText.text =
                HealthManager.Instance.Failed ? "Game Over!" : "You Rock!";
            _titleText.color = HealthManager.Instance.Failed ? Color.red : Color.green;
            
            _scoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
            _comboText.text = "Greatest Combo: " + ScoreManager.Instance.GetGreatestCombo().ToString();
            _percentText.text = "Hit Percentage: " + ScoreManager.Instance.GetPercentage().ToString() + "%";

        }
        
        public void UpdateHealth()
        {
            _healthSlider.maxValue = HealthManager.Instance.GetMaxHealth();
            _healthSlider.value = HealthManager.Instance.GetCurrentHealth();

            float percentage = HealthManager.Instance.GetCurrentHealth() / HealthManager.Instance.GetMaxHealth();
            
            if (percentage >= .75f)
                _healthImage.color = Color.green;
            else if (percentage < .75f && percentage >= .5f)
                _healthImage.color = Color.yellow;
            else if (percentage < .5f && percentage >= .25f)
                _healthImage.color = new Color(255, 127, 0, 255);
            else
                _healthImage.color = Color.red;
        }
        
        public void BackButton()
        {
            GameController.Instance.LoadLevel("MainMenu");
        }
        
        public void ShowGameOverMenu()
        {
            UpdateGameOverMenu();

            _gameOverMenu.enabled = true;
            GetComponent<Canvas>().enabled = false;
        }
    }
}
