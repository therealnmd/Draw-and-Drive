using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button playButton;
    public DrawLine drawLine;
    public GameManager gameManager;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    void OnPlayClicked()
    {
        //drawLine.StartDrawing();
        gameManager.PlayCar();
    }
}
