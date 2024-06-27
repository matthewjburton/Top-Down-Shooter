using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TryGetComponent(out Selectable selectable))
        {
            selectable.Select();
        }
        else
        {
            selectable = gameObject.GetComponentInChildren<Selectable>();
            if (selectable != null)
            {
                selectable.Select();
            }
            else
                Debug.LogWarning(gameObject.name + " does not have a Selectable component on hover");
        }
    }
}
