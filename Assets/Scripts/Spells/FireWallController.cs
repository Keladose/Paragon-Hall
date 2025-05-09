using UnityEngine;

namespace Spellect
{
    public class FireWallController : DrawableObject
    {
        public float FireDamage;
        public float FireDuration;



        private void Start()
        {
            _startTime = Time.time;
            if (Random.Range(0,30) > 1)
            {
                GetComponent<AudioSource>().enabled = false;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<EffectsController>() != null)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<EffectsController>().SetOnFire(FireDamage/8, FireDuration);
                }
                else
                {
                    collision.gameObject.GetComponent<EffectsController>().SetOnFire(FireDamage, FireDuration);
                }
            }
        }
    }

}