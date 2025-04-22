using System;
using UnityEngine;

namespace Spellect
{
    public class HealthController : MonoBehaviour
    {
        private float _maxHealth;
        private float _health;
        private bool _isDead = false;

        public class HealthChangedEventArgs : EventArgs { public float oldHealth; public float newHealth; }
        public delegate void OnHealthChangedEvent(object source, HealthChangedEventArgs e);
        public event OnHealthChangedEvent OnDamageTaken;
        public event OnHealthChangedEvent OnHealed;
        public event OnHealthChangedEvent OnMaxHealthChanged;
        public delegate void OnDeathEvent(object source, EventArgs e);
        public event OnDeathEvent OnDeath;
        private bool _isInvincible = false;
        // TODO: private gameobject
        // Start is called before the first frame update
        public void Init(float maxHealth, float health)
        {
            _maxHealth = maxHealth;
            _health = health;
        }

        public void MakeInvincible()
        {
            _isInvincible = true;
        }

        public void CancelInvincible()
        {
            _isInvincible = false;
        }

        public void Init(float maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        public float GetMaxHealth()
        {
            return _maxHealth;
        }

        public float GetHealth()
        {
            return _health;
        }

        public void AddMaxHealth(float healthChange)
        {
            float oldMaxHealth = _maxHealth;
            float healthFraction = _health / _maxHealth;
            _maxHealth = _maxHealth + healthChange;
            _health = _maxHealth * healthFraction;
            OnMaxHealthChanged?.Invoke(this, new HealthChangedEventArgs { oldHealth = oldMaxHealth, newHealth = _maxHealth });
        }
        public void TakeDamage(float damage)
        {
            if (!_isInvincible)
            {
                float oldHealth = _health;
                _health = Mathf.Max(0, _health - damage);
                Debug.Log($"{gameObject.name} took {damage} damage! Remaining HP: {_health}");
                OnDamageTaken?.Invoke(this, new HealthChangedEventArgs { oldHealth = oldHealth, newHealth = _health });
                if (_health == 0 && !_isDead)
                {
                    _isDead = true;
                    OnDeath?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Heal(float healAmount)
        {
            _isDead = false;
            float oldHealth = _health;
            _health = Mathf.Min(_maxHealth, _health + healAmount);
            OnHealed?.Invoke(this, new HealthChangedEventArgs { oldHealth = oldHealth, newHealth = _health });
        }

    }
}