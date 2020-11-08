using Buriola.Managers;
using Buriola.Pooler;
using UnityEngine;

namespace Buriola.NoteBehaviors
{
    public class NoteBehaviour : MonoBehaviour
    {
        private const float SPEED = 5.3f;
        public float Length { get; set; }

        private Material _mat;
        private Color _originalColor;

        private bool _initialized;
        public bool Played { get; set; }

        private float _timeAlive;

        private void Start()
        {
            _mat = GetComponent<MeshRenderer>().material;
            _originalColor = _mat.color;
            _initialized = true;
        }

        
        private void OnEnable()
        {
            if (_initialized)
                SetToOriginalColor();
            
            _timeAlive = 0f;
            Played = false;
        }

        private void Update()
        {
            transform.Translate(Vector3.back * (SPEED * Time.deltaTime));
            _timeAlive += Time.deltaTime;
            
            if (_timeAlive > 5f)
                Finish();
        }
        
        private void Finish()
        {
            NoteManager.NotesPlayed++;
            PoolManager.ReleaseObject(this.gameObject);
        }

        
        public void SetColor(Color c)
        {
            if (_mat.color == c)
                return;

            _mat.color = c;
        }
        
        public void SetToOriginalColor()
        {
            _mat.color = _originalColor;
        }
    }
}