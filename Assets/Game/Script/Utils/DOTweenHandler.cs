using DG.Tweening;
using UnityEngine;

public class DOTweenHandler : MonoBehaviour
{
    private void OnDisable()
    {
        DOTween.KillAll();
    }
}
