using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectileUI : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float offset;
    public float rotation;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Random.Range(0, 200) == 2)
        {
            Vector2 newPos = new Vector2(offset, 0f) + (Vector2)transform.position;
            Instantiate(projectilePrefab, newPos, Quaternion.Euler(0f, 0f, rotation) , transform);
        }
    }
}
