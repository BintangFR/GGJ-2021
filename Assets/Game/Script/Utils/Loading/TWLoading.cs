/* TWLoading version 1.0
 * Update Date : 29/08/2020
 * Created by TomWill
 */
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TomWill
{
    public class TWLoading : MonoBehaviour
    {
        public enum LoadType
        {
            AUTO_IN,
            MANUAL_IN
        }

        public enum ImageLoadType
        {
            FILL,
            AUTO_FIT,
            WIDTH_PARENT,
            HEIGHT_PARENT
        }

        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject panelLoad;
        [SerializeField] private Image loadImage;
        [SerializeField] private GameObject loadingAnimation;

        private AspectRatioFitter aspectRatioFitter;
        private TWAnimateLoadingImage animateLoadingImage;

        private static TWLoading instance;

        private TWLoadingImageLib loadImageLibrary;

        private AsyncOperation currentOperation;
        private LoadType currentLoadType;
        private bool isLoading, completeLoad;
        private float waitingTime = 2f;

        #region Setting TWLoading
        public static TWLoading Instance
        {
            get
            {
                if (instance == null)
                {
                    // Create a variable TWLoading and call CreateInstance method directly from that class
                    Debug.Log("Please call CreateInstance method before using this TWLoading");
                }
                return instance;
            }
        }

        public void CreateInstance()
        {
            createInstance();
        }

        public void SetLoadingAnimation(Sprite[] frames, int currentFrame, int fps, bool frameSkippingSupport = true)
        {
            setLoadingAnimation(frames, currentFrame, fps, frameSkippingSupport);
        }

        public static void SetupController(TWLoadingImageLib backgroundLibrary, ImageLoadType imageLoadType = ImageLoadType.AUTO_FIT)
        {
            Instance?.setupController(backgroundLibrary, imageLoadType);
        }
        #endregion

        #region TWLoading

        public static bool InLoading
        {
            get
            {
                return Instance ? Instance.isLoading : false;
            }
        }

        public static void LoadScene(string sceneName, string loadImageLibrary = "Default", LoadType loadType = LoadType.AUTO_IN)
        {
            Instance?.loadScene(sceneName, loadImageLibrary, loadType);
        }

        public static void CompleteTheLoad(UnityAction action = null)
        {
            Instance?.completeTheLoad(action);
        }

        public static void ChangeImageLoadAspectFitter(ImageLoadType imageLoadType)
        {
            Instance?.changeImageLoadAspectFitter(imageLoadType);
        }

        public static void OnSuccessLoad(UnityAction action)
        {
            Instance?.onSuccessLoad(action);
        }

        #endregion

        #region Internal Function

        private void activatePanelLoad(bool active)
        {
            panelLoad.SetActive(active);
        }

        private void adjustingCanvas()
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
        }

        private void changeImageLoad(Sprite sprite)
        {
            loadImage.sprite = sprite;
        }

        private void changeAspectRatio(Sprite sprite)
        {
            aspectRatioFitter.aspectRatio = (float) sprite.bounds.size.x/ sprite.bounds.size.y;
        }

        private void changeImageLoadAspectFitter(ImageLoadType imageLoadType)
        {
            switch (imageLoadType)
            {
                case ImageLoadType.WIDTH_PARENT:
                    aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                    break;
                case ImageLoadType.HEIGHT_PARENT:
                    aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                    break;
                case ImageLoadType.FILL:
                    aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                    break;
                case ImageLoadType.AUTO_FIT:
                    aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                    break;
            }
        }

        private void completeTheLoad(UnityAction action)
        {
            if (currentLoadType == LoadType.MANUAL_IN)
            {
                StartCoroutine(onCompleteLoadCoroutine(action));
            }
        }

        private void createInstance()
        {
            if (instance == null)
            {
                instance = this;
                adjustingCanvas();
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }

        private void loadScene(string sceneName, string loadingImage, LoadType loadType)
        {
            currentLoadType = loadType;

            resetRectLoadImage();
            changeAspectRatio(loadImageLibrary.GetImage(loadingImage));

            switch (loadType)
            {
                case LoadType.AUTO_IN:
                    loadSceneAuto(sceneName, loadingImage);
                    break;
                case LoadType.MANUAL_IN:
                    loadSceneManual(sceneName, loadingImage);
                    break;
            }
        }

        private void loadSceneAuto(string sceneName, string loadingImage)
        {
            changeImageLoad(loadImageLibrary.GetImage(loadingImage));
            activatePanelLoad(true);

            currentOperation = SceneManager.LoadSceneAsync(sceneName);
            isLoading = true;
            completeLoad = false;
            StartCoroutine(loadSceneCoroutineAuto());
        }

        private void loadSceneManual(string sceneName, string loadingImage)
        {
            changeImageLoad(loadImageLibrary.GetImage(loadingImage));
            activatePanelLoad(true);

            currentOperation = SceneManager.LoadSceneAsync(sceneName);
            isLoading = true;
            completeLoad = false;
            StartCoroutine(loadSceneCoroutineManual());
        }

        private void onCompleteLoad()
        {
            activatePanelLoad(false);
        }

        private void onSuccessLoad(UnityAction action)
        {
            StartCoroutine(waitingSuccessLoad(action));
        }

        private void resetRectLoadImage()
        {
            loadImage.rectTransform.offsetMax = Vector2.zero;
            loadImage.rectTransform.offsetMin = Vector2.zero;
        }

        private void setupController(TWLoadingImageLib loadImageLibrary, ImageLoadType imageLoadType)
        {
            aspectRatioFitter = loadImage.gameObject.GetComponent<AspectRatioFitter>();

            this.loadImageLibrary = loadImageLibrary;

            changeImageLoadAspectFitter(imageLoadType);
        }

        private void setLoadingAnimation(Sprite[] frames, int currentFrame, int fps, bool frameSkippingSupport)
        {
            if (animateLoadingImage == null)
            {
                animateLoadingImage = loadingAnimation.GetComponent<TWAnimateLoadingImage>();
                if (animateLoadingImage == null) animateLoadingImage = loadingAnimation.AddComponent<TWAnimateLoadingImage>();
            }

            animateLoadingImage.Init(frames, currentFrame, fps, frameSkippingSupport);
        }

        private IEnumerator loadSceneCoroutineAuto()
        {
            while (currentOperation != null && !currentOperation.isDone)
            {
                yield return null;
            }

            adjustingCanvas();

            currentOperation = null;
            completeLoad = true;

            yield return new WaitForSeconds(waitingTime);
            isLoading = false;

            onCompleteLoad();
        }

        private IEnumerator loadSceneCoroutineManual()
        {
            while (currentOperation != null && !currentOperation.isDone)
            {
                yield return null;
            }

            adjustingCanvas();

            currentOperation = null;
            completeLoad = true;
        }

        private IEnumerator onCompleteLoadCoroutine(UnityAction action)
        {
            yield return new WaitForSeconds(waitingTime);
            isLoading = false;
            onCompleteLoad();
            action?.Invoke();
        }

        private IEnumerator waitingSuccessLoad(UnityAction action)
        {
            yield return new WaitUntil(() => !isLoading);
            action.Invoke();
        }

        #endregion
    }
}