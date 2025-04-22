using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DescriptionBox : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI largeDescriptionText;
    public GameObject sprite;
    public GameObject panel;

    public void Show(string name, string description, string largeDescription, Sprite spriteInput)
    {
        
        nameText.text = name;
        descriptionText.text = description;
        largeDescriptionText.text = largeDescription;
        if (sprite != null)
        {
            Image image = sprite.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = spriteInput;
            }
        }
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void Hide()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}