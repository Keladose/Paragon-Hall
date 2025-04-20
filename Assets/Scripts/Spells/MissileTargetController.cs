using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class MissileTargetController : DrawableObject
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("MagicMissle"))
            {
                Destroy(gameObject);
            }
        }
    }

}