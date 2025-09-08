using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkUIController : MonoBehaviour
{
    public static InkUIController Instance;

    [Header("UI Elements")]
    public Image inkBar;     // Drag InkBar
    public Image[] stars;    // Drag Star1, Star2, Star3 (array size = 3)

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

    public void UpdateStars(float inkPercent)
    {
        int starsEarned = 0;

        if (inkPercent > 0.7f) starsEarned = 3;
        else if (inkPercent > 0.4f) starsEarned = 2;
        else if (inkPercent > 0.1f) starsEarned = 1;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = (i < starsEarned) ? Color.yellow : Color.gray;
        }
    }
}
