using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TomWill;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Range(0f, 2f)] private float scaleValue = 1.1f;
    [SerializeField] UnityEvent onEnterEvent, onExitEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * scaleValue, 0.1f).SetEase(Ease.InBounce);
        TWAudioController.PlaySFX("UI", "hover");

        onEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InBounce);

        onExitEvent?.Invoke();
    }
}
