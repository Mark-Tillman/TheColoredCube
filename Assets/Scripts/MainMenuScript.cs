using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public GameObject HowToPlayPanel;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        highScoreText.text = PlayerPrefs.GetFloat("highScore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenHowToPlay()
    {
        HowToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        HowToPlayPanel.SetActive(false);
    }
}
