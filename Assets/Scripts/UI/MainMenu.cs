using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject menuCanvas;
    public GameObject settingsCanvas;
    public GameObject creditsCanvas;

    [Header("Language buttons")]
    public Image frButton;
    public Image enButton;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMenu()
    {
        menuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void ToSettings()
    {
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void ToCredits()
    {
        menuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void SetLanguage(string language)
    {
        if (language == "fr")
        {
            enButton.color = new Vector4(1, 1, 1, 0.5f);
            frButton.color = new Vector4(1, 1, 1, 1f);
        }
        else
        {
            enButton.color = new Vector4(1, 1, 1, 1f);
            frButton.color = new Vector4(1, 1, 1, 0.5f);
        }
    }
}
