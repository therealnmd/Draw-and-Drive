using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject drawLineObject;
    public GameObject playButton;
    public CarController car;

    private DrawLine drawLine;

    private bool isGameWin = false;

    // Start is called before the first frame update
    void Start()
    {
        drawLine = drawLineObject.GetComponent<DrawLine>();
        ShowStartUI();
    }

    public void ShowStartUI()
    {
        startPanel.SetActive(true);
        winPanel.SetActive(false);
        playButton.SetActive(false);
        //car.StopMovement();
        car.ResetPosition();      // xe đứng yên
        car.SetKinematic();
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        winPanel.SetActive(false);
        playButton.SetActive(false);

        car.ResetPosition();
        //car.StartMovement();

        drawLine.StartDrawing();
    }

    public void OnFinishDrawing()
    {
        
        // 👉 gọi từ script DrawLine khi người chơi vẽ xong
        playButton.SetActive(true);
    }

    public void PlayCar()
    {
        car.StartMovement();
        playButton.SetActive(false); // ẩn Play sau khi bấm
    }


    public void RestartGame()
    {
        drawLineObject.SetActive(false);
        drawLineObject.SetActive(true); // reload line
        ShowStartUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
        isGameWin = true;
        car.StopMovement();
        winPanel.SetActive(true);
    }

    public void HitEnd()
    {
        WinGame();
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public bool IsGameWin()
    {
        return isGameWin;
    }
}
