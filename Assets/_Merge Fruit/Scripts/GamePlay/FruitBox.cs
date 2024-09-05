using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FruitBox : Singleton<FruitBox>
{
    [SerializeField] private List<Fruit> fruits;
    public bool isRemovingFruit = false;
    public bool isUpgradingFruit = false;

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Player_Dead, ChangeFruitAnim);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.On_Player_Dead, ChangeFruitAnim);
    }

    public void AddFruit(Fruit fruit)
    {
        fruits.Add(fruit);
    }
    
    public void RemoveFruit(Fruit fruit)
    {
        fruits.Remove(fruit);
    }

    public void RefreshList()
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            if (!fruits[i].gameObject.activeSelf)
            {
                fruits.RemoveAt(i);
            }
        }
    }

    public void RemoveSmallestFruit()
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            if (fruits[i].id == 0 || fruits[i].id == 1)
            {
                PoolingManager.Despawn(fruits[i].gameObject);
                RemoveFruit(fruits[i]);
                i--;
            }
        }
        
        StartCoroutine(UIManager.Instance.DeactiveBlockClick());
    }

    public void RemoveSpecificFruit()
    {
        if (!isRemovingFruit)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Fruit") && !hits[i].collider.isTrigger)
                {
                    var fruit = hits[i].collider.GetComponent<Fruit>();
                    PoolingManager.Despawn(fruit.gameObject);
                    RemoveFruit(fruit);
                    StartCoroutine(UIManager.Instance.DeactiveBlockClick());
                    isRemovingFruit = false;
                    break;
                }
            }
        }
    }

    public void UpgradeFruit()
    {
        if (!isUpgradingFruit)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Fruit") && !hits[i].collider.isTrigger)
                {
                    var fruit = hits[i].collider.GetComponent<Fruit>();
                    int id = fruit.id;
                    Vector3 pos = fruit.transform.position;
                    PoolingManager.Despawn(fruit.gameObject);
                    RemoveFruit(fruit);
                    var newFruit = PoolingManager.Spawn(GameManager.Instance.data.fruitDatas[id + 1].fruitPrefab, 
                        pos, Quaternion.identity);
                    newFruit.rb.gravityScale = 1f;
                    AddFruit(newFruit);
                    StartCoroutine(UIManager.Instance.DeactiveBlockClick());
                    isUpgradingFruit = false;
                    return;
                }
            }
        }
    }

    private void Update()
    {
        RemoveSpecificFruit();
        UpgradeFruit();
    }

    public IEnumerator ShakeBox()
    {
        TreeSlider.Instance.gameObject.SetActive(false);
        Camera mainCamera = Camera.main;
        float shakeDuration = 2f;
        float cameraZoomDuration = 2f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(mainCamera.DOOrthoSize(6.5f, cameraZoomDuration).SetEase(Ease.InOutQuad));

        sequence.AppendCallback(() =>
        {
            Vector3 shakeStrength = new Vector3(0.3f, 0.2f, 0);
            int vibrato = 10;
            float randomness = 90;

            transform.DOShakePosition(shakeDuration, shakeStrength, vibrato, randomness);

            Sequence rotationSequence = DOTween.Sequence();
            rotationSequence
                .Append(transform.DORotate(new Vector3(0, 0, 20), 0.4f).SetEase(Ease.InOutSine))  
                .Append(transform.DORotate(new Vector3(0, 0, -20), 0.4f).SetEase(Ease.InOutSine)) 
                .Append(transform.DORotate(new Vector3(0, 0, 10), 0.4f).SetEase(Ease.InOutSine))  
                .Append(transform.DORotate(new Vector3(0, 0, -10), 0.4f).SetEase(Ease.InOutSine)) 
                .Append(transform.DORotate(Vector3.zero, 0.4f).SetEase(Ease.InOutSine))  
                .OnComplete(() =>
                {
                    StartCoroutine(UIManager.Instance.DeactiveBlockClick());
                    mainCamera.DOOrthoSize(5f, cameraZoomDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
                    {
                        TreeSlider.Instance.gameObject.SetActive(true);
                        UIManager.Instance.SetCanvasSortingLayer("Default");
                    });
                });
            sequence.Join(rotationSequence);
        });

        yield return sequence.WaitForCompletion();
    }

    private void ChangeFruitAnim(object param)
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            fruits[i].anim.Play("Lost");
        }
    }
}
