using System;
using DG.Tweening;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public void ActiveBoom(Transform target, Action action = null)
    {
        transform.position = new Vector3(0, -3, 0);
        transform.localScale = Vector3.one * 3;
        
        transform.DOMove(transform.position, 0.25f).OnComplete(() =>
        {
            transform.DOMove(new Vector2(target.position.x, target.position.y + 1f), 0.5f).SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    transform.DOMove(new Vector2(target.position.x, target.position.y), 0.25f).SetEase(Ease.OutSine)
                        .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        action?.Invoke();
                    });
                });
        });

        transform.DOScale(3f, 0.25f).OnComplete(() =>
        {
            transform.DOScale(1f, 0.75f).SetEase(Ease.OutSine);
        });
    }
}
