using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static DrawableSpellController;
using static Spellect.SpellbookController;
using static Spellect.SpellcastingController;

namespace Spellect
{
    public class AttackController : MonoBehaviour
    {

        public class AttackSpellEventArgs : EventArgs { public float power; }
        public delegate void OnAttackSpellEvent(object source, AttackSpellEventArgs e);
        public event OnAttackSpellEvent OnAttackSpell;

        public SpellData equippedSpell;
        private bool magicMissleHoming = false;

        private int selectedSpellIndex = 0;
        private float _timeLastFired = 0f;

        void Update()
        {
            if (equippedSpell != null && Input.GetMouseButton(0) && Time.time > _timeLastFired + equippedSpell.cooldown)
            {
                _timeLastFired = Time.time;
                //equippedSpell = spells[selectedSpellIndex];
                FireProjectile();
            }            
        }

        public void OnSpellCast(object o, SpellCastEventArgs e)
        {
            if (e.spell.type == CastedSpell.Type.MagicMissile)
            {

            }
        }

        public void OnDrawingFinished (object o, DrawingFinishEventArgs e)
        {
            if (e.type == CastedSpell.Type.MagicMissile)
            {
                SpawnMagicHomers(e.points);
            }
        }

        private void SpawnMagicHomers(List<Vector2> points)
        {
            for (int i = 0; i < 16; i++)
            {
                float angle =  i/(8f)*360;
                GameObject projectile = Instantiate(equippedSpell.specialPrefab, transform.position,
                Quaternion.Euler(0f, 0f, angle));
                projectile.GetComponent<HomingMissileController>().Init(points, Quaternion.Euler(0f, 0f, angle) * Vector2.right);
            }
        }
        public void ChangeBook(object o, BookChangedEventArgs e)
        {
            if (e.book.type == Spellbook.Type.Attack)
            {
                AttackSpellbook book = (AttackSpellbook)e.book;
                equippedSpell = book.basicAttack;
            }
        }

        public void FireProjectile()
        {
            Vector2 direction = DirectionToMouse();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject projectile = Instantiate(equippedSpell.projectilePrefab, transform.position,
                Quaternion.Euler(0f, 0f, angle));
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * equippedSpell.speed;

            OnAttackSpell?.Invoke(this, new AttackSpellEventArgs { power = 1f });
            

        }

        public Vector2 DirectionToMouse()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector2 direction = (mouseWorldPos - transform.position).normalized;
            return direction;
        }

    }
}