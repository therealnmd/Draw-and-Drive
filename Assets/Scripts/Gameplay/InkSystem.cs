using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkSystem : MonoBehaviour
{
    public static InkSystem Instance;

    [Header("Ink Settings")]
    public float maxInk = 10f;          // Tổng mực có thể vẽ
    public float currentInk;           // Mực còn lại

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ResetInk();
    }

    public void ResetInk()
    {
        currentInk = maxInk;
        InkUIController.Instance.UpdateInkBar(currentInk / maxInk);
    }

    public bool UseInk(float amount)
    {
        if (currentInk - amount >= 0)
        {
            currentInk -= amount;
            InkUIController.Instance.UpdateInkBar(currentInk / maxInk);
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.UpdateStarsInGame(currentInk / maxInk);
            }
            return true;
        }
        return false; // hết mực
    }

    public float GetInkPercent()
    {
        return currentInk / maxInk;
    }
}
