using System.Collections;
using UnityEngine;
namespace Spellect
{
    public class Phantom : BaseEnemy
    {
        private bool isWaiting;
        private float waitTime = 1.5f;

        public float phaseDuration = 3f;
        public float materializeDuration = 2f;
        private Coroutine phaseCycleCoroutine;

        private Renderer phantomRenderer;
        private Collider phantomCollider;

        private bool isMaterialized = false;

        protected override void Start()
        {
            base.Start();

            phantomRenderer = GetComponentInChildren<Renderer>();
            phantomCollider = GetComponent<Collider>();

            phaseCycleCoroutine = StartCoroutine(PhaseCycle());
        }

        protected override void Update()
        {
            base.Update();
        }

        private IEnumerator PhaseCycle()
        {
            while (true)
            {
                // Phase (intangible, can't be damaged)
                SetPhasedState(true);
                yield return new WaitForSeconds(phaseDuration);

                // Materialize (vulnerable)
                SetPhasedState(false);
                yield return new WaitForSeconds(materializeDuration);
            }
        }

        private void SetPhasedState(bool phased)
        {
            isMaterialized = !phased;
            if (isMaterialized)
            {
                healthController.MakeInvincible();
            }
            else
            {
                healthController.CancelInvincible();
            }
                Debug.Log("Phantom is now " + (isMaterialized ? "Materialized" : "Phased"));

            // Set collider behavior
            if (phantomCollider != null)
            {
                phantomCollider.enabled = !phased;
            }

            // Visual: transparent when phased
            if (phantomRenderer != null)
            {
                Color color = phantomRenderer.material.color;
                color.a = phased ? 0.3f : 1f;
                phantomRenderer.material.color = color;
            }
        }

        protected override void Patrol()
        {
            if (patrolPoints.Length == 0 || isWaiting) return;

            AddForceTowardsTarget(patrolPoints[targetPoint].position, moveSpeed);

            if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.1f)
            {
                StartCoroutine(WaitAtPoint());
            }
        }

        private IEnumerator WaitAtPoint()
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            targetPoint = (targetPoint + 1) % patrolPoints.Length;
            isWaiting = false;
        }

        protected override void AttackPlayer()
        {
            if (!isMaterialized) return;

            var playerHealth = player.GetComponent<HealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(15); // Adjust as needed
            }
        }

    }
}