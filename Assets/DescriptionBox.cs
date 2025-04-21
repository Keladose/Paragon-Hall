using TMPro;
using UnityEngine;

public class DescriptionBox : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject panel;

    public void Show(string name, string description)
    {
        nameText.text = name;
        descriptionText.text = description;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}