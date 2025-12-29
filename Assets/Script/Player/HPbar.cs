using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public Slider hpSlider;
    public PlayerState playerState;

    void Start()
    {
        hpSlider.minValue = 0f;
        hpSlider.maxValue = 1f;
        hpSlider.value = 1f;
    }

    void Update()
    {
        if (playerState == null) return;

        hpSlider.value = playerState.GetHpRatio();
    }
}