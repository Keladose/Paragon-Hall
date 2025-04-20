using System.Collections.Generic;
using UnityEngine;

public class SpellWheelHighlighter : MonoBehaviour
{
    public Transform wheelCenter; // Assign the WheelSelect GameObject here

    private List<Transform> spellSlots = new();

    void Start()
    {
        // Populate spellSlots with all children of the wheel (Fireball, Icicle, etc.)
        foreach (Transform child in wheelCenter)
        {
            spellSlots.Add(child);
        }
    }

    void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mousePos = Input.mousePosition;
        Vector2 dir = (mousePos - screenCenter).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        int selectedIndex = Mathf.FloorToInt(angle / (360f / spellSlots.Count)) % spellSlots.Count;

        for (int i = 0; i < spellSlots.Count; i++)
        {
            Transform normal = spellSlots[i].Find("Normal");
            Transform aura = spellSlots[i].Find("Aura");

            if (normal && aura)
            {
                bool isSelected = (i == selectedIndex);
                normal.gameObject.SetActive(!isSelected);
                aura.gameObject.SetActive(isSelected);
            }
        }
    }
}