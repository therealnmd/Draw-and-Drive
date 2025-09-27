using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkUIController : MonoBehaviour
{
    public static InkUIController Instance;

    [Header("UI Elements")]
    public Image inkBar;     // Drag InkBar

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateInkBar(float fillAmount)
    {
        //if (inkBar != null)
        //    inkBar.fillAmount = Mathf.Clamp01(fillAmount);
        if (inkBar != null)
        {
            float percent = Mathf.Clamp01(fillAmount);
            inkBar.fillAmount = percent;

            // ⭐ Đổi màu khi mực trong khoảng 1 sao (0.1f – 0.4f)
            if (percent <= 0.4f && percent > 0.1f)
            {
                inkBar.color = Color.red;
            }
            else
            {
                inkBar.color = Color.black;
            }
        }
    }
}
