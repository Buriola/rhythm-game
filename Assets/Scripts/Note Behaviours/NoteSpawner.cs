using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for spawning the notes prefabs
/// </summary>
public class NoteSpawner : MonoBehaviour
{
    //Reference of the notes to spawn - the Note class stores the start time and length of this note
    private List<Note> notesToSpawn;

    [SerializeField]
    private GameObject notePrefab; //The prefab of the Note

    [SerializeField]
    private TrackInput noteTrack; //The track it belongs to

    [Range(10, 30)]
    [SerializeField]
    private int poolSize; //Value to determine the Pool Size 

    //Reference to the Manager, we receive this.
    private NoteManager noteManager;

    private void Start()
    {
        //Since we dont want to keep instantiating new object during the whole music
        //I created a pool manager that can pool any prefabs we might need. 
        PoolManager.WarmPool(notePrefab, poolSize);
    }

    /// <summary>
    /// Initializes this spawner and start spawning
    /// </summary>
    /// <param name="notes">The list of notes we want to spawn</param>
    /// <param name="manager">The manager</param>
    public void Init(List<Note> notes, NoteManager manager)
    {
        notesToSpawn = notes;
        noteManager = manager;
        //Start spawning
        StartCoroutine(SpawnNotes());
    }

    /// <summary>
    /// Will start spawning the prefab in the correct time
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnNotes()
    {
        foreach (Note n in notesToSpawn)
        {
            if (noteManager.IsGameOver()) //If it is game over, stop this
                yield break;

            yield return StartCoroutine(SpawnNote(n.Start / 1000f, n.Length / 1000f)); //Since the values of the list are in
            //miliseconds, we divide them by 1000 to get the correct value in seconds
            //This will wait for each note to be spawned individually, with no overlap
        }
            
        yield return null;
    }

    private IEnumerator SpawnNote(float start, float length)
    {
        yield return new WaitForSeconds(start - GetTime() - 1.5f); //Wait an offset time 

        //Calculates the Z pos this object will be spawned
        float z = length / 0.2f;
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + (z - 1) / 2);

        //Instead of instantiating , we get an object from our pool
        GameObject obj = PoolManager.SpawnObject(notePrefab, transform.position, transform.rotation);
//            Instantiate(notePrefab, startPos, Quaternion.identity);

        //Change the local scale based on the length of the note
        obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, 0.5f * (length / .2f));

        //Passes the Length value to the Behaviour
        obj.GetComponent<NoteBehaviour>().Length = length;

        //Wait the length time of this note before continue.
        yield return new WaitForSeconds(length);
    }

    //Aux Value to calculate the offset time
    private float GetTime()
    {
        return (Time.timeSinceLevelLoad - noteManager.Beginning);
    }

}
