using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawableSpellController;
using static Spellect.SpellbookController;
using static Spellect.SpellcastingController;

namespace Spellect
{
    public class AttackController : MonoBehaviour
    {

        public class AttackSpellEventArgs : EventArgs { public float value0; public float value1; public CastedSpell.Type type; }
        public delegate void OnAttackSpellEvent(object source, AttackSpellEventArgs e);
        public event OnAttackSpellEvent OnAttackSpell;

        public SpellData equippedSpell;
        public CastedSpell currentSpell;
        private bool magicMissleHoming = false;
        public Transform LaserRotator;

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
            if (currentSpell.type == CastedSpell.Type.Laser)
            {
                if (LaserRotator != null)
                {
                    LaserFollowPlayer(DirectionToMouse());
                }
            }

        }

        public void OnSpellCast(object o, SpellCastEventArgs e)
        {
            if (e.spell.type == CastedSpell.Type.Icicle)
            {
                StartCoroutine(IcicleStorm());
            }
            if (e.spell.type == CastedSpell.Type.Laser)
            {
                if (LaserRotator != null)
                {
                    GameObject laser = Instantiate(equippedSpell.specialPrefab, LaserRotator.transform.position + LaserRotator.rotation * (Vector3)equippedSpell.spawnOffset,
                                            LaserRotator.rotation, LaserRotator);
                    laser.GetComponent<LaserController>().Charge();
                    OnAttackSpell?.Invoke(this, new AttackSpellEventArgs { value0 = 0f, value1 = 4f, type = currentSpell.type });
                }
            }
        }

        private IEnumerator IcicleStorm()
        {
            float angleOffset = 0f;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0;j < 16; j++)
                {
                    float angle = angleOffset+(j / 16f) * 2*Mathf.PI;
                    GameObject projectile = FireProjectile(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
                    if (j > 0)
                    {
                        projectile.GetComponent<AudioSource>().enabled = false;
                    }
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
            else if (e.type == CastedSpell.Type.Tornado)
            {
                SpawnTornado(e.points);
            }
        }

        private void SpawnMagicHomers(List<Vector2> points)
        {
            StartCoroutine(SpawnHomerDomers(points));
        }

        private IEnumerator SpawnHomerDomers(List<Vector2> points)
        {
            for (int j = 0; j < 60; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    float angle = i / (4f) * 360;
                    GameObject projectile = Instantiate(equippedSpell.specialPrefab, transform.position,
                    Quaternion.Euler(0f, 0f, angle));
                    projectile.GetComponent<HomingMissileController>().Init(points, Quaternion.Euler(0f, 0f, angle) * Vector2.right);
                    projectile.GetComponent<ProjectileCollide>().isPlayerProjectile = true;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }


        private void SpawnTornado(List<Vector2> points)
        {
            GameObject tornado = Instantiate(equippedSpell.specialPrefab, points[0],
                Quaternion.identity);
            float angle = UnityEngine.Random.Range(0, 360f);
            tornado.GetComponent<HomingMissileController>().Init(points, Quaternion.Euler(0f, 0f, angle) * Vector2.right);
            tornado.GetComponent<ProjectileCollide>().isPlayerProjectile = true;
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

        public GameObject FireProjectile(Vector2 direction)
        {
            if (currentSpell.type == CastedSpell.Type.Laser && LaserRotator != null)
            {
                GameObject laser = Instantiate(equippedSpell.projectilePrefab, LaserRotator.transform.position + LaserRotator.rotation*(Vector3)equippedSpell.spawnOffset,
                                            LaserRotator.rotation, LaserRotator);
                laser.GetComponent<LaserController>().Charge();
                OnAttackSpell?.Invoke(this, new AttackSpellEventArgs { value0 = 1f, value1 = 0.5f, type = currentSpell.type });
                return laser;
            }


            float randomizer = 0f;
            if (currentSpell.type == CastedSpell.Type.Icicle)
            {
                randomizer = UnityEngine.Random.Range(-6f,6f);
            }
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + randomizer;

            Vector2 spawnOffset = Quaternion.Euler(0f, 0f, angle) * equippedSpell.spawnOffset;

            if (currentSpell.type == CastedSpell.Type.Tornado)
            {
                angle += 180;
            }
            GameObject projectile = Instantiate(equippedSpell.projectilePrefab, 
                                            transform.position + (Vector3)spawnOffset, 
                                            Quaternion.Euler(0f, 0f, angle));
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * equippedSpell.speed;
            if (currentSpell.type == CastedSpell.Type.Icicle)
            {
                projectile.GetComponent<ProjectileSpeedup>().Init(equippedSpell.speed, 0.5f, Quaternion.Euler(0,0,randomizer)*direction);
            }
            else if (currentSpell.type == CastedSpell.Type.Tornado)
            {
                projectile.GetComponent<ProjectileCollide>().Init(false);
            }
            projectile.GetComponent<ProjectileCollide>().isPlayerProjectile = true;
            OnAttackSpell?.Invoke(this, new AttackSpellEventArgs { value0 = 0, value1 = 0, type = currentSpell.type });
            return projectile;
        }
        public Vector2 DirectionToMouse()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector2 direction = (mouseWorldPos - transform.position).normalized;
            return direction;
        }

        private void LaserFollowPlayer(Vector2 direction)   
        {
            float multiplier = 1f;
            //Debug.Log(LaserRotator.parent.localRotation.eulerAngles.y);
            if (LaserRotator.parent.localRotation.eulerAngles.y > 0)
            {
                multiplier = -1;
            }
            float angle = Mathf.Atan2(direction.y, direction.x* multiplier) * Mathf.Rad2Deg;
            LaserRotator.localRotation = Quaternion.Euler(0, 0, angle);
        }

        
    }
}