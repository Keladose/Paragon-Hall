using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class HealthController : MonoBehaviour
    {
        private float _maxHealth;
        private float _health;
        // TODO: private gameobject
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(float maxHealth, float health)
        {
            _maxHealth = maxHealth;
            _health = health;
        }
        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(0, _health - damage);
            
        }
    }
}