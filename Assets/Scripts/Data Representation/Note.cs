using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note
{
    private int start; //Time in miliseconds that this note is to be played
    private int length; //The length of this note

    /// <summary>
    /// General Constructor
    /// </summary>
    /// <param name="start"></param>
    /// <param name="length"></param>
    public Note(int start, int length)
    {
        this.start = start;
        this.length = length;
    }

    //Properties
    public int Start { get { return start; } set { start = value; } }
    public int Length { get { return length; } set { length = value; } }
}
