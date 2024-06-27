using UnityEngine;
using UnityEngine.UI;

public class ReloadSlider : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = gameObject.GetComponentInChildren<Slider>();
    }

    public void SetSlider(float progress)
    {
        if (slider != null)
        {
            slider.value = progress;
        }
    }
}