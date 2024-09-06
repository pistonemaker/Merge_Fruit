using DG.Tweening;
using TMPro;
using UnityEngine;

public class FailNotice : MonoBehaviour
{
    public SpriteRenderer sr;
    public TextMeshPro text;

    public void ShowNotice(string str)
    {
        text.text = str;
        transform.position = new Vector3(0f, -0.5f, 0f);
        var scale = transform.localScale;
        sr.DOFade(0.5f, 0f);
        
        transform.DOLocalMoveY(0.5f, 1.5f).OnComplete(() =>
        {
            PoolingManager.Despawn(gameObject);
        });
        
        sr.DOFade(1f, 0.75f).OnComplete(() =>
        {
            sr.DOFade(0.5f, 0.75f);
        });
        
        transform.DOScale(scale * 1.1f, 0.75f).OnComplete(() =>
        {
            transform.DOScale(scale, 0.75f);
        });
    }
}
