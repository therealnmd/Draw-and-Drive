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
        if (inkBar != null)
            inkBar.fillAmount = Mathf.Clamp01(fillAmount);
    }
}
