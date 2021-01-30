using System;
using UnityEngine;
using UnityEngine.Events;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float timeToFade;
    [SerializeField] private Color baseColor;

    private float timeElapsed;
    private bool inFading;
    private bool fadeIn;
    private Color colorFading;

    public static TransitionManager Instance;
    private UnityEvent onComplete;

    public float TimeToFade { get => timeToFade; set => timeToFade = value; }

    private void Awake()
    {
        timeElapsed = 0;
        Instance = this;
        renderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
        onComplete = new UnityEvent();
    }

    private void Update()
    {
        if (inFading)
        {
            Transition();
        }
    }

    public void FadeIn(UnityAction action)
    {
        if (!inFading)
        {
            colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
            timeElapsed = 0;
            fadeIn = true;
            inFading = true;
            onComplete.AddListener(action == null ? NullHandler : action);
        }
    }

    public void FadeOut(UnityAction action)
    {
        if (!inFading)
        {
            colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
            timeElapsed = timeToFade;
            fadeIn = false;
            inFading = true;
            onComplete.AddListener(action == null ? NullHandler : action);
        }
    }

    void Transition()
    {
        if (fadeIn)
        {
            timeElapsed += Time.timeScale == 0 ? Time.unscaledDeltaTime : Time.deltaTime;

            if (timeElapsed >= timeToFade)
            {
                timeElapsed = timeToFade;
                inFading = false;
                colorFading.a = timeElapsed / timeToFade;

                if (onComplete != null) onComplete.Invoke();
            }
            else
            {
                colorFading.a = timeElapsed / timeToFade;
            }
        }
        else
        {
            timeElapsed -= Time.timeScale == 0 ? Time.unscaledDeltaTime : Time.deltaTime;

            if (timeElapsed <= 0)
            {
                timeElapsed = 0;
                inFading = false;
                colorFading.a = timeElapsed / timeToFade;

                if (onComplete != null) onComplete.Invoke();

            }
            else
            {
                colorFading.a = timeElapsed / timeToFade;
            }
        }

        renderer.color = colorFading;
    }

    void NullHandler()
    {

    }

}