using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public HealthController healthController;
    private float _fireDamage = 0f;
    private float _fireDuration = 0f;
    public float _fireFrequency = 2f;
    bool burning = false;
    public void SetOnFire(float strength, float duration)
    {
        _fireDuration = duration;
        _fireDamage = strength;
        Debug.Log("Caught on fire");
        if (!burning)
        {
            StartCoroutine(TakeFireDamage());
        }
    }
    
    IEnumerator TakeFireDamage()
    {
        burning = true;
        float startTime = Time.time;
        while (Time.time < startTime + _fireDuration)
        {

            Debug.Log("Taking damage");
            healthController.TakeDamage(_fireDamage);
            yield return new WaitForSeconds(_fireFrequency);
        }
        burning = false;
    }
}
