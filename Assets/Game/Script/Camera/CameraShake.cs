using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraShake : MonoBehaviour
{
    public Camera main;

    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;

    public static CameraShake instance;
    void Start()
    {
        instance = this;
    }

    public void Shake(float duration, float strength, int vibrato)
    {
        this.duration = duration;
        this.strength = strength;
        this.vibrato = vibrato;
        DOTween.Complete("Shake");
        main.DOShakeRotation(this.duration, this.strength, this.vibrato).SetId("Shake");
        //main.DOShakeRotation(this.duration, Vector3.zero, this.vibrato);
    }

    public void Shaked()
    {
        main.DOShakeRotation(this.duration, this.strength, this.vibrato);
    }

    public void GoBackRotate()
    {
        main.transform.localEulerAngles = Vector3.zero;
    }
}
