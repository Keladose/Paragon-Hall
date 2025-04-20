using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        public CastedSpell currentSpell;
        private bool magicMissleHoming = false;

        private int selectedSpellIndex = 0;
        private float _timeLastFired = 0f;

        void Update()
        {
            if (equippedSpell != null && Input.GetMouseButton(0) && Time.time > _timeLastFired + equippedSpell.cooldown)
            {
                _timeLastFired = Time.time;
                //equippedSpell = spells[selectedSpellIndex];
                FireProjectile(DirectionToMouse());
            }            
        }

        public void OnSpellCast(object o, SpellCastEventArgs e)
        {
            if (e.spell.type == CastedSpell.Type.Icicle)
            {
                StartCoroutine(IcicleStorm());
            }
        }

        private IEnumerator IcicleStorm()
        {
            float angleOffset = 0f;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0;j < 8; j++)
                {
                    float angle = angleOffset+(j / 8f) * 2*Mathf.PI;
                    FireProjectile(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
                }
                yield return new WaitForSeconds(0.2f);
                angleOffset += 360 * Mathf.Sqrt(2);
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
                currentSpell = book.castedSpell;
            }
        }

        public void FireProjectile(Vector2 direction)
        {
            float randomizer = 0f;
            if (currentSpell.type == CastedSpell.Type.Icicle)
            {
                randomizer = UnityEngine.Random.Range(-6f,6f);
            }
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + randomizer;

            GameObject projectile = Instantiate(equippedSpell.projectilePrefab, transform.position, Quaternion.Euler(0f, 0f, angle)
                );
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * equippedSpell.speed;
            if (currentSpell.type == CastedSpell.Type.Icicle)
            {
                projectile.GetComponent<ProjectileSpeedup>().Init(equippedSpell.speed, 0.5f, Quaternion.Euler(0,0,randomizer)*direction);
            }

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