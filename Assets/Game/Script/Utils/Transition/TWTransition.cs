using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace TomWill
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TWTransition : MonoBehaviour
    {      
        public enum TransitionType { DEFAULT_IN, DEFAULT_OUT, UP_IN, UP_OUT, DOWN_IN, DOWN_OUT}
        [SerializeField] private float baseTimeToFade = 1;
        [SerializeField] private Color baseColor;

        private float offsetMove = 10f;
        private float timeToFade;
        private SpriteRenderer rendererSprite;
        private bool inFading;
        private Color colorFading;
        private UnityEvent onComplete;

        private static TWTransition instance;

        #region Setting
        private static TWTransition Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Instance of TWTransition is not yet created");
                }

                return instance;
            }
        }
        #endregion

        #region Unity Function
        private void Awake()
        {
            rendererSprite = GetComponent<SpriteRenderer>();
            instance = this;
            onComplete = new UnityEvent();

            initializeSpriteConstruct();
            adjustingScreen();
            ChangeToBaseColor();
        }
        #endregion

        #region TWTransition

        public static void ScreenTransition(TransitionType type, float duration = -1f, UnityAction action = null)
        {
            Instance?.screenTransition(type, duration, action);
        }

        public static void ScreenFlash(int flashCount = 1, float duration = 0.1f, UnityAction action = null)
        {
            Instance?.screenFlash(flashCount, duration, action);
        }
        #endregion

        #region Internal Function
        private void initializeSpriteConstruct()
        {
            Texture2D tex = new Texture2D(2,2);
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    tex.SetPixel(x, y, Color.white);
                }
            }
            tex.Apply();
            Sprite overlay = Sprite.Create(tex, new Rect(Vector2.one, Vector2.one), Vector2.one/2, 1);
            rendererSprite.sprite = overlay;
        }

        private void screenFlash(int flashCount, float duration, UnityAction action)
        {
            ChangeColor(Color.white);
            transform.localPosition = new Vector3(0f, 0f, 11f);
            DOTween.Sequence()
                .Append(rendererSprite.DOFade(1,duration/2f))
                .Append(rendererSprite.DOFade(0, duration/2f))
                .SetLoops(flashCount)
                .OnComplete(()=> { if (action != null) action.Invoke(); });
        }

        private void adjustingScreen()
        {
            //reference http://answers.unity.com/answers/620736/view.html
            transform.localScale = new Vector3(1, 1, 1);

            float width = rendererSprite.sprite.bounds.size.x;
            float height = rendererSprite.sprite.bounds.size.y;

            float worldScreenHeight = Camera.main.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
        }

        private void screenTransition(TransitionType type, float duration, UnityAction action)
        {
            switch (type)
            {
                case TransitionType.DEFAULT_IN:
                    Instance?.screenTransitionFadeIn(duration, action);
                    break;
                case TransitionType.DEFAULT_OUT:
                    Instance?.screenTransitionFadeOut(duration, action);
                    break;
                case TransitionType.UP_IN:
                    Instance?.screenTransitionUpIn(duration, action);
                    break;
                case TransitionType.UP_OUT:
                    Instance?.screenTransitionUpOut(duration, action);
                    break;
                case TransitionType.DOWN_IN:
                    Instance?.screenTransitionDownIn(duration, action);
                    break;
                case TransitionType.DOWN_OUT:
                    Instance?.screenTransitionDownOut(duration, action);
                    break;
            }
        }

        private void screenTransitionUpIn(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3( 0f, -offsetMove, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                rendererSprite.color = colorFading;

                transform.DOLocalMoveY(0, timeToFade).OnComplete(()=>
                {
                    inFading = false;
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionUpOut(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                rendererSprite.color = colorFading;

                transform.DOLocalMoveY(offsetMove, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    rendererSprite.DOFade(0f, 0f);
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionDownIn(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, offsetMove, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                rendererSprite.color = colorFading;

                transform.DOLocalMoveY(0, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionDownOut(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                rendererSprite.color = colorFading;

                transform.DOLocalMoveY(-offsetMove, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    rendererSprite.DOFade(0f, 0f);
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionFadeIn(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
                rendererSprite.color = colorFading;

                rendererSprite.DOFade(1f, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionFadeOut(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                rendererSprite.color = colorFading;

                rendererSprite.DOFade(0f, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    rendererSprite.DOFade(0f, 0f);
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void NullHandler()
        {

        }

        private void ChangeColor(Color color)
        {
            rendererSprite.color = color;
        }

        private void ChangeToBaseColor()
        {
            rendererSprite.color = baseColor;
        }
        #endregion
    }
}