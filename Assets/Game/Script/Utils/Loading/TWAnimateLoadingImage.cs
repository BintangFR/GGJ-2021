/* TWLoading version 1.0
 * Update Date : 29/08/2020
 * Created by TomWill
 */
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TWAnimateLoadingImage : MonoBehaviour
{
    private Image image;

    private Sprite[] frames;
    private int currentFrame;
    private int fps;
    private bool frameSkippingSupport;
    private float timeDelay;

    private float timeElapse;

    public void Init(Sprite[] frames, int currentFrame, int fps, bool frameSkippingSupport = true)
    {
        image = GetComponent<Image>();
        this.frames = frames;
        this.currentFrame = currentFrame;
        this.fps = fps;
        this.frameSkippingSupport = frameSkippingSupport;
        timeDelay = 1f / fps;

        image.sprite = frames[currentFrame];
    }

    private void Animate()
    {
        if (frames.Length > 0)
        {
            if (timeElapse >= timeDelay)
            {
                currentFrame += frameSkippingSupport ? Mathf.FloorToInt(timeElapse / timeDelay) : 1;

                timeElapse = 0;

                if (currentFrame >= frames.Length) currentFrame -= frames.Length;

                image.sprite = frames[currentFrame];
            }
            else timeElapse += Time.deltaTime;
        }
    }

    private void Update()
    {
        Animate();
    }
}
