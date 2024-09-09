using System;
using DG.Tweening;
using UnityEngine;

public class BoomExplosion : MonoBehaviour
{
    public SpriteRenderer sr;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.one * 0.75f;
    }

    public void Explode(Action action = null)
    {
        transform.DOScale(Vector3.one * 1.75f, 0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            action?.Invoke();
        });
    }
}
