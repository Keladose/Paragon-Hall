using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Spellect
{
    public abstract class HealthBarController : MonoBehaviour
    {

        // Start is called before the first frame update
        public Transform backgroundHealthBar;
        protected float _oldHealth = 0f;
        protected float _maxHealth = 0f;
        protected Tween damageTween;
        protected Tween sizeTween;
        public float healthbarScaler = 150f;
        public void Init(float maxHealth)
        {
            _maxHealth = maxHealth;
            SizeHealthbar(0);
        }
        public abstract void UpdateHealth(object o, HealthController.HealthChangedEventArgs e);

        public void UpdateMaxHealth(object o, HealthController.HealthChangedEventArgs e)
        {
            Debug.Log("Updated max health to" + e.newHealth);
            _maxHealth = e.newHealth;
            SizeHealthbar(e.oldHealth);
        }
        protected abstract void SizeHealthbar(float oldMaxHealth);
    }



}
