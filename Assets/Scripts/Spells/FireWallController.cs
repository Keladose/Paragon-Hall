using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class FireWallController : DrawableObject
    {
        public float FireDamage;
        public float FireDuration;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<EffectsController>() != null)
            {
                collision.gameObject.GetComponent<EffectsController>().SetOnFire(FireDamage, FireDuration);
            }
        }
    }

}