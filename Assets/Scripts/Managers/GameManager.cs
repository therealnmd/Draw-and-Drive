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

    [Header("References")]
    public CarController car;
    private DrawLine drawLine;

    [Header("Effects")]
    public ParticleSystem[] confettiEffect;

    [Header("Stars")]
    public Image[] starsUnderInk;
    public Image[] starsWinPanel;

    [Header("Ink UI")]
    public TMP_Text inkPercentText;

    private bool isGameWin = false;

    // Start is called before the first frame update
    void Start()
    {
        drawLine = drawLineObject.GetComponent<DrawLine>();
        ShowStartUI();
        AudioManager.Instance.PlayMusic("bgm");
    }

    public void ShowStartUI()
    {
        startPanel.SetActive(true);
        winPanel.SetActive(false);
        playButton.SetActive(false);

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

    //sau khi vẽ xong
    public void OnFinishDrawing()
    {
        playButton.SetActive(true);
    }

    //xe chạy
    public void PlayCar()
    {
        car.StartMovement();
        playButton.SetActive(false);
        drawLine.StopDrawing();
    }

    public void RestartGame()
    {
        ShowStartUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
        isGameWin = true;
        car.StopMovement();
        AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.PlaySFX("win");

        float inkPercent = InkSystem.Instance.GetInkPercent();
        int starsEarned = CalculateStars(inkPercent);

        float inkUsedPercent = (1f - inkPercent) * 100f;

        for (int i = 0; i < starsWinPanel.Length; i++)
        {
            starsWinPanel[i].enabled = (i < starsEarned);
        }

        if (inkPercentText != null)
        {
            inkPercentText.text = $"{inkUsedPercent:F1}%";
        }

        //hiệu ứng pháo hoa
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

        //gợi ý
        foreach (var g in guides)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }

    }

    //tính sao
    private int CalculateStars(float inkPercent)
    {
        if (inkPercent > 0.7f) return 3;
        else if (inkPercent > 0.4f) return 2;
        else if (inkPercent > 0.1f) return 1;
        return 0;
    }

    //hiển thị sao dưới thanh mực
    public void UpdateStarsInGame(float inkPercent)
    {
        int starsEarned = CalculateStars(inkPercent);

        for (int i = 0; i < starsUnderInk.Length; i++)
        {
            if (i < starsEarned)
            {
                starsUnderInk[i].color = Color.yellow;
            }
            else
            {
                starsUnderInk[i].color = Color.gray;
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

        if (sceneHienTai < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(sceneHienTai + 1);
        }
        else
        {
            Debug.Log("Chưa có màn mới, cảm ơn đã chơi hết!");
        }

        Time.timeScale = 1; 
    }

    //review màn chơi
    public void Review()
    {
        winPanel.SetActive(false);
        if (car != null)
        {
            car.StopMovement();
        }
        StartCoroutine(EnableWinPanelAgain());
    }

    private IEnumerator EnableWinPanelAgain()
    {
        yield return new WaitForSeconds(5f);
        winPanel.SetActive(true);
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }
}
