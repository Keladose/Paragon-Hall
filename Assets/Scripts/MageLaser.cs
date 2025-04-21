using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageLaser : MonoBehaviour
{
    private Transform player;
    public Transform rotator;
    public Transform laser;

    public float castTime = 3.0f;
    public float castTimeLineScale = 0.05f; // this is the intitial line width while mage is charging
    public float finalScale = 1.0f;
    public float allowedDodgeTime = 1.0f;
    public float attackSpeed = 5.0f;
    public float attackRange = 10.0f;
    public float RandomStartCastTimeRange = 10.0f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(LaserEvery3Seconds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LaserFollowPlayer(Vector2 direction)
    {
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotator.localRotation = Quaternion.Euler(0, 0, angle);
        laser.localScale = new Vector3(0.05f, attackRange, 1f);
        laser.localPosition = new Vector3(attackRange + 1f , 0f, 0f);
    }

    IEnumerator LaserEvery3Seconds()
    {
        
        while (true)
        {
            laser.gameObject.SetActive(false);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, RandomStartCastTimeRange));
            laser.gameObject.SetActive(true);
            
            
            float timer = 0f;
            
            while (timer < castTime)
            {
                timer += Time.deltaTime;
                Vector2 direction = player.position - transform.position;
                LaserFollowPlayer(direction);
                yield return null;
            }
            yield return new WaitForSeconds(allowedDodgeTime);
            
            laser.transform.localScale = new Vector3(finalScale, laser.transform.localScale.y, 
                laser.transform.localScale.z);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
