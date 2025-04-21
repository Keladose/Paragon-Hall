    using Spellect;
    using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SpellBookUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string spellName;
    public string spellDescription;

    [TextArea]
    public string fullDescription;

    private DescriptionBox descriptionBox;
    private RectTransform rectTransform;
    private RectTransform Norm;
    private RectTransform Aura;
    private SpellbookController SBController;
    

    private void Start()
    {
        descriptionBox = FindObjectOfType<DescriptionBox>();
        rectTransform = GetComponent<RectTransform>();
        Norm = transform.Find("Norm").transform.GetComponent<RectTransform>();
        Aura = transform.Find("Aura").transform.GetComponent<RectTransform>();
        SBController = FindObjectOfType<SpellbookController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionBox.Show(spellName, spellDescription);
        
        Aura.gameObject.SetActive(true);
        Aura.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionBox.Hide();
        rectTransform.localScale = Vector3.one;
        Aura.gameObject.SetActive(false);
        Aura.transform.localScale = Vector3.one;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        descriptionBox.Show(spellName, fullDescription);
    }
}
