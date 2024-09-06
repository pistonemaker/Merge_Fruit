using System;
using DG.Tweening;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public void ActiveBoom(Transform target, Action action)
    {
        transform.position = new Vector3(0, -3, 0);
        transform.localScale = Vector3.one * 3;
        transform.DOMoveY(target.position.y + 0.2f, 0.5f).OnComplete(() =>
        {
            transform.DOMoveY(target.position.y, 0.25f).OnComplete(() =>
            {
                action?.Invoke();
            });
        });
        transform.DOScale(1.5f, 0.5f).OnComplete(() =>
        {
            transform.DOScale(target.transform.localScale, 0.25f);
        });
    }
}
