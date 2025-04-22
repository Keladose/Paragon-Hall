using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace Spellect
{ 
    public class PlayerHealthBarController : HealthBarController
    {
        public Image healthBar;
        public TMP_Text text;
        public bool isCooldownBar = false;
        public bool isInitialised = false;


        private void Awake()
        {
            if (!isCooldownBar)
            {
                if (GameManager.Instance != null)
                {
                    if (GameManager.Instance.playerObject != null)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        DontDestroyOnLoad(this);
                    }
                }

            }
            
        }

        public override void UpdateHealth(object o, HealthController.HealthChangedEventArgs e)
        {
            damageTween.Complete();
            if (!isCooldownBar)
            {
                damageTween = DOTween.To(() => healthBar.fillAmount, x => { healthBar.fillAmount = x; text.text = (x* _maxHealth).ToString("F0"); }, e.newHealth / _maxHealth, 0.5f);
                damageTween.SetEase(Ease.OutCubic);
            }
            else
            {
                damageTween = DOTween.To(() => healthBar.fillAmount, x => { healthBar.fillAmount = x;
                    text.text = ((1 - x) * _maxHealth).ToString("F1");
                },1- e.newHealth / _maxHealth, 0.05f);

                damageTween.SetEase(Ease.Linear);
            }
            _oldHealth = e.newHealth ;
        }
        private void Update()
        {
            if (isCooldownBar && text.text.Equals("0.0"))
            {
                text.text = "";
            }
        }

        protected override void SizeHealthbar(float oldMaxHealth)
        {
            sizeTween.Complete();
            sizeTween = DOTween.To(() => backgroundHealthBar.GetComponent<LayoutElement>().minWidth, x => backgroundHealthBar.GetComponent<LayoutElement>().minWidth = x,  _maxHealth / healthbarScaler, 0.2f);
            if (!isCooldownBar)
            {
                text.text = _maxHealth.ToString("F0");
            }
            else
            {
                text.text = _maxHealth.ToString("F1");
            }
            backgroundHealthBar.GetComponent<LayoutElement>().minWidth = _maxHealth / healthbarScaler;
            sizeTween.SetEase(Ease.OutCubic);

        }

    }
}