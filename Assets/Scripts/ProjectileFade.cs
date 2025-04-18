using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFade : MonoBehaviour
{
    public float scaleDuration = 0.3f;
    public float finalScale = 0.1f;

    private float timer = 0f;

    void Start()
    {
        transform.localScale = Vector3.zero; // start at 0 size
    }

    void Update()
    {
        if (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / scaleDuration);
            float scale = Mathf.SmoothStep(0f, finalScale, t);
            transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
