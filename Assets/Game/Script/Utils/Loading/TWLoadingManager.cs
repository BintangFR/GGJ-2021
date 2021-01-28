/* TWLoading version 1.0
 * Update Date : 29/08/2020
 * Created by TomWill
 */
using UnityEngine;

namespace TomWill
{
    [RequireComponent(typeof(TWLoading), typeof(TWLoadingImageLib))]
    public class TWLoadingManager : MonoBehaviour
    {
        [SerializeField] private TWLoading loadingController;
        [SerializeField] private TWLoadingImageLib loadingImageLib;

        [SerializeField] private Sprite[] loadingFrames;
        [SerializeField] private int currentFrame;
        [SerializeField] private int fps;

        void Awake()
        {
            loadingController = GetComponent<TWLoading>();

            loadingController.CreateInstance();
            loadingController.SetLoadingAnimation(loadingFrames, currentFrame, fps);
            TWLoading.SetupController(loadingImageLib);
            //TWLoading.ChangeImageLoadAspectFitter(TWLoading.ImageLoadType.AUTO_FIT);
        }

        public int FrameLength()
        {
            if (loadingFrames == null) return 0;
            return loadingFrames.Length;
        }
    }
}