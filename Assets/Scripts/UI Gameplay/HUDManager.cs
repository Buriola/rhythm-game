using UnityEngine;

/// <summary>
/// This class manages the UI elements during gameplay
/// </summary>
public class HUDManager : Singleton<HUDManager>
{
    [SerializeField]
    private Canvas gameOverMenu; //Reference of the Game Over Menu

    //UI Components references beloe
    [SerializeField]
    private UnityEngine.UI.Text hudText;
    [SerializeField]
    private UnityEngine.UI.Slider healthSlider;
    [SerializeField]
    private UnityEngine.UI.Image healthImage;

    [Space]
    
    [SerializeField]
    private UnityEngine.UI.Text titleText;
    [SerializeField]
    private UnityEngine.UI.Text scoreText;
    [SerializeField]
    private UnityEngine.UI.Text comboText;
    [SerializeField]
    private UnityEngine.UI.Text percentText;


    private void Start()
    {
        UpdateHUDText();
        UpdateHealth();
    }

    /// <summary>
    /// Updates the HUD information
    /// This function subscribes to the events of the Score Manager, we dont need an Update function here
    /// </summary>
    public void UpdateHUDText()
    {
        if (hudText != null)
        {
            //Gets all the values from the Score Manager
            string s = ScoreManager.Instance.GetMultiplier().ToString() + "X\n" + ScoreManager.Instance.GetScore().ToString() + "\n" +
            ScoreManager.Instance.GetCombo().ToString() + "HITS!";
            s = s.Replace("\n", System.Environment.NewLine);

            hudText.text = s;
        }   
    }

    /// <summary>
    /// Updates the Game Over Menu Information before showing it.
    /// </summary>
    private void UpdateGameOverMenu()
    {
        titleText.text = HealthManager.Instance.Failed ? "Game Over!" : "You Rock!"; //Check if the player lost all his health
        titleText.color = HealthManager.Instance.Failed ? Color.red : Color.green; //Change color of text

        //Get values from the Score Manager
        scoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString(); 
        comboText.text = "Greatest Combo: " + ScoreManager.Instance.GetGreatestCombo().ToString();
        percentText.text = "Hit Percentage: " + ScoreManager.Instance.GetPercentage().ToString() + "%";

    }

    /// <summary>
    /// Updates the Health Meter
    /// </summary>
    public void UpdateHealth()
    {
        //Gets values from the Health Manager
        healthSlider.maxValue = HealthManager.Instance.GetMaxHealth();
        healthSlider.value = HealthManager.Instance.GetCurrentHealth();

        float percentage = HealthManager.Instance.GetCurrentHealth() / HealthManager.Instance.GetMaxHealth();

        //Little juice to change color of bar

        if (percentage >= .75f)
            healthImage.color = Color.green;
        else if (percentage < .75f && percentage >= .5f)
            healthImage.color = Color.yellow;
        else if (percentage < .5f && percentage >= .25f)
            healthImage.color = new Color(255, 127, 0, 255);
        else
            healthImage.color = Color.red;
    }

    /// <summary>
    /// Function to the back button
    /// </summary>
    public void BackButton()
    {
        //Loads the Main Menu
        GameController.Instance.LoadLevel("MainMenu");
    }

    /// <summary>
    /// Shows Game Over Menu, called by NoteManager when the game is over
    /// </summary>
    public void ShowGameOverMenu()
    {
        UpdateGameOverMenu(); //Updates the info.

        gameOverMenu.enabled = true; //Enables it
        GetComponent<Canvas>().enabled = false; //Disable the HUD
    }
}
