using System.Collections;
using Spellect;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileCollide : MonoBehaviour
{
    private Animator animator;
    private EnemyHealthBarController barController;
    public SpellData spellData;
    private bool healthNotInit;
    private bool _hasHitAnimation = true;
    public bool isPlayerProjectile = false;
    public bool hasAudio = false;
    private bool dieOnAudioFinish = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        barController = GetComponentInChildren<EnemyHealthBarController>();
        if (GetComponent<AudioSource>() != null)
        {
            hasAudio = true;
            GetComponent<AudioSource>().pitch *= Random.Range(0.8f, 1.2f);
        }

        if (!isPlayerProjectile)
        {
            GetComponent<AudioSource>().enabled = false;
        }

    }
    public void Init(bool hasHisAnimation)
    {
        _hasHitAnimation = hasHisAnimation;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") && isPlayerProjectile) || (other.CompareTag("Player") && !isPlayerProjectile) )
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 knockbackDirection = (-transform.position + other.transform.position).normalized;


            
            if (!spellData.goesThroughEnemies)
            {
                rb.velocity = Vector2.zero;
            }
            other.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * spellData.knockBack);
                var enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.healthController.TakeDamage(spellData.damage); // Uses Phantom�s override
            }

            if (other.CompareTag("Player"))
            {
                other.GetComponent<HealthController>().TakeDamage(spellData.damage / 5);
            }
            

            if (_hasHitAnimation)
            {
                animator.Play("Hit");
            }
            if (!spellData.goesThroughEnemies)
            {
                StartCoroutine(WaitBeforeDestroy());
            }
        }

        if (other.CompareTag("Map Bounds"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero; 
            if (_hasHitAnimation)
            {

                animator.Play("Hit");
            }
            StartCoroutine(WaitBeforeDestroy());
        }
    }
    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(0.4f);
        if (GetComponent<AudioSource>() != null && !spellData.spellName.Equals("Tornado"))
        {

            while (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                if (GetComponentInChildren<Light2D>() != null)
                {
                    GetComponentInChildren<Light2D>().enabled = false;
                }

                yield return new WaitForSeconds(0.4f);
            }
        }
        Destroy(gameObject);
    }
}
