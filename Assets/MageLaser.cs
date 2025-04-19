using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageLaser : MonoBehaviour
{
    private Transform player;
    public Transform rotator;
    public Transform laser;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = player.position - transform.position;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        rotator.localRotation = Quaternion.Euler(0, 0, angle);

        laser.localScale = new Vector3(0.05f, direction.magnitude, 1f);
        laser.localPosition = new Vector3(direction.magnitude + 1f , 0f, 0f);
    }
}
