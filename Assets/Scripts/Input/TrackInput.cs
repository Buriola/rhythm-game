using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for tracking the input of a note
/// </summary>
public class TrackInput : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private bool checkForInput = true; // Should it check for inputs?
    [SerializeField]
    private KeyCode inputKey = default; //Which key to use for this object

    // Auxiliar values to change Y position (Press effect)
    private float defaultY;
    private float desiredY = -2.5f;

    private bool isPlaying; //Check if the player is pressing the key
    private bool isValidToPlay; //Check if it is valid to press the note

    private float timer; //Timer to know for how long a note is valid 

    private GameObject lastNote; //Reference of the last object that touched our collider
    private NoteBehaviour note; //Reference of the last note that touched our collider

    #endregion

    private void Start()
    {
        defaultY = transform.position.y; //Get the initial Y position
        timer = 0; // init timer
    }

    private void Update()
    {
        if (checkForInput) //Will only check for inputs if this is true
        {
            PressButton(); //Check
            TrackNoteHits(); //Tracking notes that hit us
        }

        //If it is Game Over, stop checking input
        if (NoteManager.Instance.IsGameOver() && checkForInput)
            checkForInput = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        isValidToPlay = true; //Once a note collides, it is valid to play this note
        timer = 0;
        lastNote = other.gameObject; //Getting our references
        note = lastNote.GetComponent<NoteBehaviour>();
    }

    private void OnTriggerStay(Collider other)
    {
        //In case this is a long note and we still have time to press the note

        if (isValidToPlay && note.Length > 0.2f && IsNotePlaying() && timer < .2f)
        {
            note.SetColor(Color.green); //Show some feedback to know we are holding the note
            ScoreManager.Instance.IncreaseScore(1); //Adding score
        }
        // In case the note is valid to play and we are not holding the button
        else if (isValidToPlay && !IsNotePlaying() && timer < .2f)
        {
            timer += Time.deltaTime; //Increase the timer
            note.SetToOriginalColor(); // Set the note to its original color
        }
        else //If the timer expires, we lose this note
        {
            isValidToPlay = false;
            note.Played = false; //To know that this note wasnt played
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //When the note exits our collider
        // It s not valid to press the button
        isValidToPlay = false;
        
        //Send a message to this note, put it back in the pool
        lastNote.SendMessage("Finish", SendMessageOptions.DontRequireReceiver);

        //In case we missed this note
        if (!note.Played)
        {
            //Decrease out health bar
            HealthManager.Instance.DecreaseHP(10f);
            //Reset combos and multipliers
            ScoreManager.Instance.ComputeScore(false);
        }
    }

    /// <summary>
    /// Check if we are pressing a button
    /// </summary>
    private void PressButton()
    {
        if (Input.GetKey(inputKey) && transform.position.y != desiredY)
        {
            isPlaying = true;
            //Change Y position of the object for feedback
            transform.position = new Vector3(transform.position.x, desiredY, transform.position.z); // Changing Y position
        }
        else if (Input.GetKeyUp(inputKey))
        {
            isPlaying = false;
            transform.position = new Vector3(transform.position.x, defaultY, transform.position.z); // Reseting Y position
        }
    }

    private void TrackNoteHits()
    {
        if (isValidToPlay && IsNotePlaying())
        {
            if (lastNote != null && note.Length <= 0.2f && !note.Played)
            {
                note.Played = true;
                HealthManager.Instance.AddHP(3f);
                ScoreManager.Instance.ComputeScore(true);
                lastNote.SendMessage("Finish", SendMessageOptions.DontRequireReceiver);
            }
            else if (lastNote != null && note.Length > 0.2f && !note.Played)
            {
                note.Played = true;
                HealthManager.Instance.AddHP(3f);
                ScoreManager.Instance.ComputeScore(true);
            }
        }

        if (!isValidToPlay && Input.GetKeyDown(inputKey))
        {
            ScoreManager.Instance.ComputeScore(false);
            HealthManager.Instance.DecreaseHP(5f);
        }
            
    }

    private bool IsNotePlaying()
    {
        return isPlaying;
    }
}
