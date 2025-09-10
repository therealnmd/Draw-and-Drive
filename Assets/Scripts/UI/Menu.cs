using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject levelPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene("Lv1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Close()
    {
        levelPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
