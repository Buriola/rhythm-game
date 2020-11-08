using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Scriptable object to represent a song
/// </summary>
[CreateAssetMenu(menuName = "Create Song", fileName = "New Song Asset")]
public class Song : ScriptableObject
{
    public string songName; //Song name
    public string artistName; //Artist
    public AudioClip musicFile; //The Audio clip for this music
    public TextAsset chartFile; //The chart file to instantiate notes
	
}
