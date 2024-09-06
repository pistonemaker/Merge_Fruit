using DG.Tweening;
using UnityEngine;

public class PanelScaler : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.05f, 0.25f).SetUpdate(true))
            .Append(transform.DOScale(0.95f, 0.1f).SetUpdate(true))
            .Append(transform.DOScale(1f, 0.1f).SetUpdate(true));
    }

    private void OnDisable()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.05f, 0.15f).SetUpdate(true))
            .Append(transform.DOScale(0f, 0.25f).SetUpdate(true));
    }
}
