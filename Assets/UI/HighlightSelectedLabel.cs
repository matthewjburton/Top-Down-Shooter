using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightSelectedLabel : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    TextMeshProUGUI label;

    [Header("Label Colors")]
    Color selectedColor;
    Color normalColor;

    void OnEnable()
    {
        GetLabel();
        GetColors();
        SetColor();
    }

    void GetLabel()
    {
        label = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
    }

    void GetColors()
    {
        ColorBlock colorBlock = GetComponent<Selectable>().colors;
        selectedColor = colorBlock.selectedColor;
        normalColor = colorBlock.normalColor;
    }

    void SetColor()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            label.color = selectedColor;
        }
        else
        {
            label.color = normalColor;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        label.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        label.color = normalColor;
    }
}