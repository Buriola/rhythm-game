using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the behaviour of a note
/// </summary>
public class NoteBehaviour : MonoBehaviour
{
    private float speed = 5.3f; //Speed the note will move
    public float Length { get; set; } //The length of the note (for long notes)

    private Material mat; //Reference to the material
    private Color originalColor; //Reference of the color

    private bool initialized; //Check if it initialized
    public bool Played { get; set; } //Check if the note was played

    private float timeAlive; //Value to know for how long the object has been active

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        originalColor = mat.color;
        initialized = true;
    }

    //Since OnEnable is called before Start, there is no need to keep getting reference for more than once
    // Thats why we use the initialized bool. Start will only be called once
    // We are always reusing these objects, thats why we can do this and make sure Start will only be called once
    private void OnEnable ()
    {
        if(initialized)
            SetToOriginalColor(); // In case the color is changed

        //Every time this object is enabled, we reset these variables
        timeAlive = 0f;
        Played = false;
    }

    private void Update ()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime); //Will start to move backwards...
        timeAlive += Time.deltaTime; //Updates the time alive

        //If it is greater than 5s, we release the object
        if (timeAlive > 5f)
            Finish();
	}

    /// <summary>
    /// Releases this object - Send back to the object pool
    /// </summary>
    private void Finish()
    {
        NoteManager.notesPlayed++; //Updates the manager
        PoolManager.ReleaseObject(this.gameObject); //Releasing
    }

    /// <summary>
    /// Change color if this a long note
    /// </summary>
    /// <param name="c"></param>
    public void SetColor(Color c)
    {
        if (mat.color == c)
            return; // If it has the same color, we dont do anything

        mat.color = c;
    }

    /// <summary>
    /// Returns to original color
    /// </summary>
    public void SetToOriginalColor()
    {
        mat.color = originalColor;
    } 
}
