using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control the score data/information and combo streaks
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    private int score; //The score
    private int combo; //Combo
    private int greatestCombo; //Greatest Combo

    private int noteStreak; //Notes streak (All notes that the player didnt miss)
    private int totalNotes; //Total notes of the song
    private float notePercentage; //Percentage

    private int multiplier; //Multiplier
    
    //Events for UI Subscription or whoelse is interested to know these events
    [SerializeField]
    private UnityEngine.Events.UnityEvent onScoreChange = default;
    [Space]
    [SerializeField]
    private UnityEngine.Events.UnityEvent onComboChange = default;
    [Space]
    [SerializeField]
    private UnityEngine.Events.UnityEvent onMultiplierChange = default;
    [Space]
    [SerializeField]
    private UnityEngine.Events.UnityEvent onPercentageChange = default;

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Initializes the variables
    /// </summary>
    private void Init()
    {
        notePercentage = 0;
        score = 0;
        combo = 0;
        multiplier = 1;
        totalNotes = 0;
        greatestCombo = 0;
    }

    private void Start()
    {
        totalNotes = NoteManager.Instance.Notes(); //Get notes value from the NoteManager
    }

    /// <summary>
    /// Increases the combo value
    /// </summary>
    private void IncreaseCombo()
    {
        combo++;
        if (greatestCombo < combo) greatestCombo = combo; //Assign greatest combo value

        //Call event
        onComboChange.Invoke();

        //Update mulitplier, since it is based on combos
        UpdateMultiplier();
    }

    /// <summary>
    /// Updates multiplier based in the combo value
    /// </summary>
    private void UpdateMultiplier()
    {
        if (combo < 10)
            multiplier = 1;
        else if (combo < 30)
            multiplier = 2;
        else if (combo < 50)
            multiplier = 4;
        else
            multiplier = 8;
    
        //Call event
        onMultiplierChange.Invoke();
    }

    /// <summary>
    /// This will be called for compute a score. Handles the other calls
    /// </summary>
    /// <param name="streak">Whether you hit the note or not</param>
    public void ComputeScore(bool streak)
    {
        if (streak) //If the player hits a note
        { 
            noteStreak++; //Increase the notes hit
            IncreaseCombo(); //Updates combo
            IncreaseScore(100); //Increase score
        }
        else //If he missed or press the button in a wrong time
            ResetCombo(); //Reset combo

        //Calculates hit percentage
        notePercentage = Mathf.Round(((float)noteStreak / (float)totalNotes * 100f) * 10) / 10f;

        //Call event
        onPercentageChange.Invoke();
    }

    /// <summary>
    /// Resets combo and multiplier if the player misses a note
    /// </summary>
    public void ResetCombo()
    {
        combo = 0;
        multiplier = 1;

        //Call events
        onComboChange.Invoke();
        onMultiplierChange.Invoke();
    }

    /// <summary>
    /// Increases score given an amount
    /// </summary>
    /// <param name="value">The total points to be added</param>
    public void IncreaseScore(int value)
    {
        score += (value * multiplier); //Using multiplier 

        //Call event
        onScoreChange.Invoke();
    }


    //Getters below...
    public int GetScore()
    {
        return score;
    }

    public int GetCombo()
    {
        return combo;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }

    public int GetPercentage()
    {
        if (notePercentage <= 0)
            return 0;

        return Mathf.RoundToInt(notePercentage);
    }

    public int GetTotalNotes()
    {
        return totalNotes;
    }

    public int GetGreatestCombo()
    {
        return greatestCombo;
    }
}
