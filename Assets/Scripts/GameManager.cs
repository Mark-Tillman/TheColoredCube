using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject EndGameMenu;
    public GameObject LeftButton;
    public GameObject RightButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI fpsText;
    public float deltaTime;
    public float score = 0;
    public bool gameActive = true;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        LeftButton.SetActive(true);
        RightButton.SetActive(true);
        EndGameMenu.SetActive(false);
        score = 0;

        PlayerPrefs.GetFloat("highScore", 0);
        gameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        //Display FPS
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil (fps).ToString ();
    }

    public void LoseGame()
    {
        gameActive = false;
        if(score > PlayerPrefs.GetFloat("highScore", 0))
        {
            PlayerPrefs.SetFloat("highScore", score);
        }

        highScoreText.text = PlayerPrefs.GetFloat("highScore", 0).ToString();

        LeftButton.SetActive(false);
        RightButton.SetActive(false);
        EndGameMenu.SetActive(true);

    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ScorePoint(float pointsToScore)
    {
        score += pointsToScore;
    }

    public float GetScore()
    {
        return score;
    }
}
