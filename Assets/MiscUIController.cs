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
        
        for (int i = 1; i < 9; i++)
        {
            mapInterface.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (GameManager.Instance != null && GameManager.Instance.currentRoom != null)
        {
            int currentRoomId = GameManager.Instance.currentRoom.roomId;
            Debug.Log(currentRoomId);
            if (currentRoomId >= 0 && currentRoomId < 9)
            {
                mapInterface.transform.Find("Room" + currentRoomId).gameObject.SetActive(true);
            }
        }

        
    }

    void UpdateCollectedBook()
    {
        for (int i = 0; i < spellbookController.AttackSpellbooks.Count; i++)
        {
            
        }
    }
}
