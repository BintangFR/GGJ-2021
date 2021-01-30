using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InGameUI : MonoBehaviour
{
    public static InGameUI instance;
    [SerializeField] private GameObject pausePanel, winPanel, losePanel;
    [SerializeField] private Text retryText; 
    [SerializeField] private string sceneName;

    void Start()
    {
        instance = this;
        TransitionManager.Instance.FadeOut(null);
        Time.timeScale = 1;
        GameVariables.GAME_PAUSE = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameVariables.GAME_PAUSE)
            {
                Pause();
            }
            else if (GameVariables.GAME_PAUSE || GameVariables.GAME_WIN || GameVariables.GAME_OVER)
            {
                Exit();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameVariables.GAME_OVER)
            {
                Retry();
            }

            if (GameVariables.GAME_WIN)
            {
                ContinueLevel();
            }

            if (GameVariables.GAME_PAUSE)
            {
                Resume();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !GameVariables.GAME_OVER && !GameVariables.GAME_WIN && !GameVariables.GAME_PAUSE)
        {
            Retry();
        }
    }

    public void ShowLoseMenu()
    {
        retryText.gameObject.SetActive(false);
        losePanel.SetActive(true);
        GameData.instance.ChickCollect = 0;
    }

    public void ShowWinMenu()
    {
        retryText.gameObject.SetActive(false);
        winPanel.SetActive(true);
    }

    public void Pause()
    {
        retryText.gameObject.SetActive(false);
        pausePanel.SetActive(true);
        GameVariables.GAME_PAUSE = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        GameVariables.GAME_PAUSE = false;
        GameVariables.GAME_OVER = false;
        GameVariables.GAME_WIN = false;
        Time.timeScale = 1;
    }

    public void Retry()
    {
        GameVariables.GAME_PAUSE = false;
        GameVariables.GAME_OVER = false;
        GameVariables.GAME_WIN = false;
        TransitionManager.Instance.FadeIn(MoveToGame);
    }

    public void MoveToGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        TransitionManager.Instance.FadeIn(MoveToMenu);
    }

    public void MoveToMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ContinueLevel()
    {
        TransitionManager.Instance.FadeIn(ContinueGame);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
