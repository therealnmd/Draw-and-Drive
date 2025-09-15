using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject drawLineObject;
    public GameObject playButton;
    public GameObject[] guides;
    public GameObject starScoreUI;

    [Header("References")]
    public CarController car;
    private DrawLine drawLine;

    [Header("Effects")]
    public ParticleSystem[] confettiEffect;

    [Header("Stars")]
    public Image[] starsUnderInk;
    public Image[] starsWinPanel;

    [Header("Total Stars UI")]
    public TMP_Text totalStarsText;
    private static int totalStars = 0;

    private bool isGameWin = false;

    // Start is called before the first frame update
    void Start()
    {
        drawLine = drawLineObject.GetComponent<DrawLine>();
        ShowStartUI();
        AudioManager.Instance.PlayMusic("bgm");

        totalStars = PlayerPrefs.GetInt("TotalStars", 0);
        UpdateTotalStarsUI();
    }

    public void ShowStartUI()
    {
        startPanel.SetActive(true);
        winPanel.SetActive(false);
        playButton.SetActive(false);
        starScoreUI.SetActive(false);

        car.ResetPosition();     
        car.SetKinematic();

        foreach (var g in guides)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        winPanel.SetActive(false);
        playButton.SetActive(false);
        starScoreUI.SetActive(true);

        car.ResetPosition();

        InkSystem.Instance.ResetInk();

        drawLine.StartDrawing();

        foreach (var g in guides)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
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
        starScoreUI.SetActive(true);
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
        starScoreUI.SetActive(false);
        AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.PlaySFX("win");

        float inkPercent = InkSystem.Instance.GetInkPercent();
        int starsEarned = CalculateStars(inkPercent);

        SaveStarsForLevel(starsEarned);

        for (int i = 0; i < starsWinPanel.Length; i++)
        {
            starsWinPanel[i].enabled = (i < starsEarned);
        }

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

        foreach (var g in guides)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }

    }

    private int CalculateStars(float inkPercent)
    {
        if (inkPercent > 0.7f) return 3;
        else if (inkPercent > 0.4f) return 2;
        else if (inkPercent > 0.1f) return 1;
        return 0;
    }

    public void UpdateStarsInGame(float inkPercent)
    {
        int starsEarned = CalculateStars(inkPercent);

        for (int i = 0; i < starsUnderInk.Length; i++)
        {
            starsUnderInk[i].color = (i < starsEarned) ? Color.red : Color.gray;
        }
    }
    private void UpdateTotalStarsUI()
    {
        if (totalStarsText != null)
        {
            totalStarsText.text = ": " + totalStars.ToString();
        }
    }

    private void SaveStarsForLevel(int starsEarned)
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        string key = "LevelStars_" + currentLevel;

        int oldStars = PlayerPrefs.GetInt(key, 0);

        if (starsEarned > oldStars)
        {
            PlayerPrefs.SetInt(key, starsEarned);
        }

        // Tính lại tổng từ đầu
        totalStars = 0;
        int totalLevels = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < totalLevels; i++)
        {
            totalStars += PlayerPrefs.GetInt("LevelStars_" + i, 0);
        }

        PlayerPrefs.SetInt("TotalStars", totalStars);
        PlayerPrefs.Save();

        UpdateTotalStarsUI();
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
