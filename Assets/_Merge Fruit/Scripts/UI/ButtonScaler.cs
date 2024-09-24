using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float scale = 0.85f;
    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(BlockClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(BlockClick);
    }

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

    private IEnumerator BlockContinuousClick()
    {
        button.interactable = false;
        yield return new WaitForSeconds(0.75f);
        button.interactable = true;
    }

    private void BlockClick()
    {
        StartCoroutine(BlockContinuousClick());
    }
}
