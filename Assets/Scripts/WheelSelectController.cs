using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;
using UnityEngine.UI;

public class WheelSelectController : MonoBehaviour
{
    public Image wheelImage;
    private SpellbookController spellbookController;
    public GameObject wheel;
    
    public List<GameObject> sectorSlots = new(); // drag your spell slot UI elements here

    private GameObject closestSectorSlot;
    private GameObject previousSectorSlot;
    // Start is called before the first frame update
    void Start()
    {
        spellbookController = GameObject.FindObjectOfType<SpellbookController>();
        closestSectorSlot = sectorSlots[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            wheelImage.gameObject.SetActive(true);
            float closestDistance = float.MaxValue;
            
            for (int i = 0; i < sectorSlots.Count; i++)
            {
                Vector2 slotPos = ((RectTransform)sectorSlots[i].transform).position;
                Vector2 mousePos = Input.mousePosition;
                float distance = Vector2.Distance(slotPos, mousePos);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSectorSlot = sectorSlots[i];
                }
            }

            if (previousSectorSlot != null && previousSectorSlot != closestSectorSlot)
            {
                previousSectorSlot.transform.localScale = Vector3.one;
                Transform hoverAura = previousSectorSlot.transform.Find("Aura");
                hoverAura.gameObject.SetActive(false);
            }
            
            if (closestSectorSlot != null)
            {
                
                Transform hoverAura = closestSectorSlot.transform.Find("Aura");
                bool collectedBook = false;
                for (int i = 0; i < spellbookController.AttackSpellbooks.Count; i++)
                {
                    if (closestSectorSlot.name == spellbookController.AttackSpellbooks[i].basicAttack.name)
                    {
                        collectedBook = true;
                    }
                }

                if (collectedBook)
                {
                    closestSectorSlot.transform.localScale = new Vector3(2f, 2f, 2f);
                    hoverAura.gameObject.SetActive(true);
                }
                
                previousSectorSlot = closestSectorSlot;
            }

        }
        else
        {
            wheelImage.gameObject.SetActive(false);
            Spellbook selectedBook = null;
            for (int i = 0; i < spellbookController.AttackSpellbooks.Count; i++)
            {
                if (closestSectorSlot.name == spellbookController.AttackSpellbooks[i].basicAttack.name)
                {
                    selectedBook = spellbookController.AttackSpellbooks[i];
                }
            }

            if (selectedBook != null)
            {
                spellbookController.ChangeBook(selectedBook);
            }
            
        }
    }
    
    
    public void UpdateWheelSlots()
    {
        foreach (Transform child in wheel.transform)
        {
            Transform normal = child.Find("Normal");
            Transform locked = child.Find("Locked");
            if (normal != null) { normal.gameObject.SetActive(false); }
            if (locked != null) { locked.gameObject.SetActive(true); }

        }

        foreach (AttackSpellbook att in spellbookController.AttackSpellbooks)
        {
            foreach (Transform child in wheel.transform)
            {
                if (child.name == att.basicAttack.name)
                {
                    child.gameObject.SetActive(true); 
                    Transform normal = child.Find("Normal");
                    if (normal != null)
                        normal.gameObject.SetActive(true); 
                }
            }
        }
    }
}

