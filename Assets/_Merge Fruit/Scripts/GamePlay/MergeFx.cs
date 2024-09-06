using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MergeFx : MonoBehaviour
{
    public int id;
    public SpriteRenderer sr;
    public List<MergeFxData> mergeFxData;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ShowMergeFx(int fruitID)
    {
        id = fruitID;
        transform.localScale = Vector3.one * (mergeFxData[id].fxScale * 0.8f);
        sr.color = mergeFxData[id].fxColor;
        transform.DOScale(Vector3.one * mergeFxData[id].fxScale, 0.5f).OnComplete(() =>
        {
            PoolingManager.Despawn(gameObject);
        });
    }
}

[Serializable]
public class MergeFxData
{
    public float fxScale;
    public Color fxColor;
}
