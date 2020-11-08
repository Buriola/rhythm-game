using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.Managers
{
    public class HealthManager : Singleton<HealthManager>
    {
        public bool Failed { get; private set; }

        [FormerlySerializedAs("maxHealth")] 
        [SerializeField, Range(100, 200)] 
        private float _maxHealth = 100;
        
        private float _currentHealth;
        
        [SerializeField] private UnityEngine.Events.UnityEvent onHealthChange = default;
        [SerializeField] private UnityEngine.Events.UnityEvent onHealthReachingZero = default;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }
        
        public void AddHP(float amount)
        {
            if (Failed) return;
            
            _currentHealth += amount;
            if (_currentHealth >= _maxHealth)
                _currentHealth = _maxHealth;

            onHealthChange?.Invoke();
        }
        
        public void DecreaseHP(float amount)
        {
            if (Failed) return;
            
            if (_currentHealth > 0)
                _currentHealth -= amount;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Failed = true;
                onHealthReachingZero?.Invoke();
            }

            onHealthChange?.Invoke();
        }
        
        public float GetMaxHealth()
        {
            return _maxHealth;
        }

        public float GetCurrentHealth()
        {
            return _currentHealth;
        }
    }
}
