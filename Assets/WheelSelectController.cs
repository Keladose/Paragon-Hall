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
                closestSectorSlot.transform.localScale = new Vector3(2f, 2f, 2f);
                Transform hoverAura = closestSectorSlot.transform.Find("Aura");
                hoverAura.gameObject.SetActive(true);
                previousSectorSlot = closestSectorSlot;
            }

        }
        else
        {
            wheelImage.gameObject.SetActive(false);
        }
    }
    
    void HighlightSector(int index)
    {
        for (int i = 0; i < sectorSlots.Count; i++)
        {
            Transform normal = sectorSlots[i].transform.Find("Normal");
            Transform aura = sectorSlots[i].transform.Find("Aura");

            if (normal != null) normal.gameObject.SetActive(i != index);
            if (aura != null) aura.gameObject.SetActive(i == index);
        }
    }
    public void UpdateWheelSlots()
    {
        foreach (Transform child in wheel.transform)
        {
            child.gameObject.SetActive(false);
            Transform normal = child.Find("Normal");
            if (normal != null)
                normal.gameObject.SetActive(false);
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
