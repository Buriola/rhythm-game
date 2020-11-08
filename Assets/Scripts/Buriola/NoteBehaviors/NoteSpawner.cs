using System.Collections;
using System.Collections.Generic;
using Buriola.Data;
using Buriola.Managers;
using Buriola.Pooler;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.NoteBehaviors
{
    public class NoteSpawner : MonoBehaviour
    {
        private List<Note> _notesToSpawn;

        [FormerlySerializedAs("notePrefab")] 
        [SerializeField] 
        private GameObject _notePrefab = null;

        [FormerlySerializedAs("poolSize")] 
        [SerializeField, Range(10, 30)] 
        private int _poolSize = 0;
        
        private NoteManager _noteManager;

        private void Start()
        {
            PoolManager.WarmPool(_notePrefab, _poolSize);
        }
        
        public void Init(List<Note> notes, NoteManager manager)
        {
            _notesToSpawn = notes;
            _noteManager = manager;
            
            StartCoroutine(SpawnNotes());
        }
        
        private IEnumerator SpawnNotes()
        {
            foreach (Note n in _notesToSpawn)
            {
                if (_noteManager.IsGameOver())
                    yield break;

                yield return StartCoroutine(SpawnNote(n.Start / 1000f, n.Length / 1000f));
            }

            yield return null;
        }

        private IEnumerator SpawnNote(float start, float length)
        {
            yield return new WaitForSeconds(start - GetTime() - 1.5f);
            
            GameObject obj = PoolManager.SpawnObject(_notePrefab, transform.position, transform.rotation);
            
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y,
                0.5f * (length / .2f));
            
            obj.GetComponent<NoteBehaviour>().Length = length;
            
            yield return new WaitForSeconds(length);
        }
        
        private float GetTime()
        {
            return (Time.timeSinceLevelLoad - _noteManager.Beginning);
        }
    }
}
