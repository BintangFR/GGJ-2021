using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Playables;

public class CreditUI : MonoBehaviour
{
    [SerializeField] private bool active;
    [SerializeField] private GameObject text;
    [SerializeField] private string sceneName;
    [SerializeField] private PlayableDirector timeline;

    private void Start()
    {
        TransitionManager.Instance.FadeOut(null);
        Time.timeScale = 1f;
        DOVirtual.DelayedCall(TransitionManager.Instance.TimeToFade, () => timeline.Play());
    }

    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                active = false;
                NewGame();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                active = false;
                Application.Quit();
            }
        }
    }

    public void Activated(bool flag)
    {
        text.SetActive(flag);
        active = flag;
    }

    public void NewGame()
    {
        TransitionManager.Instance.FadeIn(GoToNewGame);
    }

    public void GoToNewGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
