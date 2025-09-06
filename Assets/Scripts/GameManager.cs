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

    public ParticleSystem[] confettiEffect;

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
        car.ResetPosition();     
        car.SetKinematic();
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        winPanel.SetActive(false);
        playButton.SetActive(false);

        car.ResetPosition();

        InkSystem.Instance.ResetInk();

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
        drawLine.StopDrawing();
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

        float inkPercent = InkSystem.Instance.GetInkPercent();
        InkUIController.Instance.UpdateStars(inkPercent);

        if (confettiEffect != null && confettiEffect.Length > 0)
        {
            foreach (ParticleSystem confetti in confettiEffect)
            {
                if (confetti != null)
                {
                    confetti.Play();
                    winPanel.SetActive(true);
                }
            }
        }
        
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
    public void Continue()
    {
        int sceneHienTai = SceneManager.GetActiveScene().buildIndex;

        // Check if there is a next scene
        if (sceneHienTai < SceneManager.sceneCountInBuildSettings - 1)
        {
            // Load the next scene
            SceneManager.LoadScene(sceneHienTai + 1);
        }
        else
        {
            Debug.Log("Chưa có màn mới, cảm ơn đã chơi hết!");
        }

        Time.timeScale = 1; // Unfreeze the game
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }
}
