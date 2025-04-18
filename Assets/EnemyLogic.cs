using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;

    public float moveSpeed = 2f;
    private float waitTime = 2f;
    private bool isWaiting = false;

    public Transform player;
    public float chaseRange = 2f;

    private bool isChasing;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            chasePlayer();
        }
        else if (!isWaiting)
        {
            patrol();
        }
    }

    void increaseTargetInt()
    {
        targetPoint++;
        if(targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        increaseTargetInt();
    }

    void chasePlayer()
    {
        if (isChasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
    
    void patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, moveSpeed * Time.deltaTime);
        if (transform.position == patrolPoints[targetPoint].position)
        {
            StartCoroutine(WaitAtPoint());
        }
    }
}
