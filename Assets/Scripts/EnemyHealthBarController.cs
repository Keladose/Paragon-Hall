using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Spellect
{ 
    public class EnemyHealthBarController : HealthBarController
    {
        public SpriteRenderer healthBar;
        

        private void Awake()
        {
            healthbarScaler = 150f; 
        }
        
        
        private void Start()
        {
            var health = GetComponent<HealthController>();
            if (health != null)
                health.OnDamageTaken += UpdateHealth;
        }
        
        public override void UpdateHealth(object o, HealthController.HealthChangedEventArgs e)
        {
            Debug.Log("Took" +  (e.oldHealth - e.newHealth).ToString() + " damage");
            Debug.Log($"UpdateHealth called! {e.oldHealth} → {e.newHealth}");
            damageTween.Complete();
            damageTween = DOTween.To(() => healthBar.size.x, x => healthBar.size = new Vector2(x, healthBar.size.y), e.newHealth/_maxHealth, 0.5f);
            damageTween.SetEase(Ease.OutCubic);
            _oldHealth = e.newHealth ;
        }


        protected override void SizeHealthbar(float oldMaxHealth)
        {
            sizeTween.Complete();
            sizeTween = DOTween.To(() => backgroundHealthBar.transform.localScale.x, x => backgroundHealthBar.transform.localScale = new Vector3(x, backgroundHealthBar.transform.localScale.y, backgroundHealthBar.transform.localScale.z), _maxHealth / healthbarScaler, 0.2f);
            sizeTween.SetEase(Ease.OutCubic);
        }
    }
}