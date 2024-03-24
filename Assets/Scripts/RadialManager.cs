using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialManager : MonoBehaviour
{
    [SerializeField] private float indicatorTimer = 1.0f;
    [SerializeField] private float maxIndicatorTimer = 1.0f;

    [SerializeField] private Image indicator;
    [SerializeField] private Image dashIcon;

    private bool ShouldRender = false;

    private void Start()
    {
        indicator.enabled = false;
        dashIcon.enabled = false;
    }

    private void Update()
    {
        if(ShouldRender)
        {
            indicator.enabled = true;
            dashIcon.enabled = true;
            indicator.fillAmount = indicatorTimer;
            indicatorTimer -= Time.deltaTime / 5f;

            if (indicatorTimer <= 0)
            {
                indicatorTimer = maxIndicatorTimer;
                indicator.fillAmount = maxIndicatorTimer;
                indicator.enabled = false;
                dashIcon.enabled = false;
                ShouldRender = false;
            }
        }
    }

    public void RunRadial()
    {
        ShouldRender = true;
    }

}
