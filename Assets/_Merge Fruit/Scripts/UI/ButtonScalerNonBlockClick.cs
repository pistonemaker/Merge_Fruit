using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScalerNonBlockClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float scale = 0.85f;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(scale, 0.15f).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.05f, 0.1f).SetUpdate(true))
            .Append(transform.DOScale(0.95f, 0.05f).SetUpdate(true))
            .Append(transform.DOScale(1f, 0.05f).SetUpdate(true));
        AudioManager.Instance.PlaySFX("Button_Click");
    }
}