using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject defaultSelectedOption;

    void OnEnable()
    {
        defaultSelectedOption.GetComponent<Selectable>().Select();
    }
}