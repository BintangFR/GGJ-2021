using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class BGM_Ambient : MonoBehaviour
{
    [SerializeField] private string audioSourceName, clipName;
    [SerializeField] private float delay_play;

    private void Start()
    {
        PlayBGMAmbient();
    }
    private void PlayBGMAmbient()
    {
        DOTween.Sequence()
            .AppendCallback(() => { TWAudioController.PlaySFX(audioSourceName, clipName); })
            .AppendInterval(delay_play)
            .SetLoops(-1, LoopType.Restart)
            .SetId("ambient");
    }

    private void OnDisable()
    {
        DOTween.Kill("ambient");
    }
}
