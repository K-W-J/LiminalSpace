using UnityEngine;

namespace Code.Entities
{
    public class EntityHealth : MonoBehaviour, IEntityComponent
    {
        public int CurrentHealth => _currentHealth;
        private int _currentHealth;

        public int MaxHealth => _maxHealth;
        protected int _maxHealth;
        public bool IsCurrentHealthMax { get; private set; }
        
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
            IsCurrentHealthMax = true;
        }
        
        public void TakeDamage(int damage)
        {
            IsCurrentHealthMax = false;
            
            if (_currentHealth > 0)
            {
                _currentHealth -= damage;
                _entity.OnTakeDamage?.Invoke();
            }

            if(_currentHealth <= 0)
                _entity.OnDeath?.Invoke();
        }
        
        public void HealHealth(int heel)
        {
            _currentHealth += heel;
            
            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;

            if (_currentHealth == _maxHealth)
                IsCurrentHealthMax = true;
        }
        
    }
}