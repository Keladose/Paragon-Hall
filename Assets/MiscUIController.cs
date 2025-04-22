using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

public class MiscUIController : MonoBehaviour
{
    public GameObject mapInterface;
    public SpellbookController spellbookController;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            mapInterface.SetActive(true);
        }
        else
        {
            mapInterface.SetActive(false);
        }
        
    }

    void UpdateCollectedBook()
    {
        for (int i = 0; i < spellbookController.AttackSpellbooks.Count; i++)
        {
            
        }
    }
}
