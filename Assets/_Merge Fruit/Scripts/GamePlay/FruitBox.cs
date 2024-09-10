using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FruitBox : Singleton<FruitBox>
{
    public SpriteRenderer sr;
    [SerializeField] private List<Fruit> fruits;
    public bool isRemovingFruit = false;
    public bool isUpgradingFruit = false;

    private void OnEnable()
    {
        sr = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        EventDispatcher.Instance.RegisterListener(EventID.On_Player_Dead, ChangeFruitAnim);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.On_Player_Dead, ChangeFruitAnim);
    }

    public int GetFruitNumber()
    {
        return fruits.Count;
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

    public void ShowFruitsTarget()
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            fruits[i].target.SetActive(true);
        }
    }

    public void HideFruitsTarget()
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            fruits[i].target.SetActive(false);
        }
    }

    public bool HasSuitableFruit()
    {
        bool hasSuitableFruit = false;

        for (int i = 0; i < fruits.Count; i++)
        {
            if (fruits[i].id == 0 || fruits[i].id == 1)
            {
                hasSuitableFruit = true;
                break;
            }
        }

        return hasSuitableFruit;
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
                    var boom = GameManager.Instance.boom;
                    boom.gameObject.SetActive(true);
                    boom.ActiveBoom(fruit.transform, () =>
                    {
                        var boomExplode = GameManager.Instance.boomExplosion;
                        boomExplode.gameObject.SetActive(true);
                        boomExplode.transform.position = fruit.transform.position;
                        boomExplode.Explode(() =>
                        {
                            PoolingManager.Despawn(fruit.gameObject);
                            RemoveFruit(fruit);
                            StartCoroutine(UIManager.Instance.DeactiveBlockClick());
                            HideFruitsTarget();
                            isRemovingFruit = false;
                            UIManager.Instance.UITop.SetActive(true);
                            TreeSlider.Instance.lines.gameObject.SetActive(true);
                            TreeSlider.Instance.gameObject.SetActive(true);
                            BoosterSignpost.Instance.gameObject.SetActive(false);
                        });
                    });
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

                    var mergeFx1 = PoolingManager.Spawn(GameManager.Instance.mergeFx, newFruit.transform.position, Quaternion.identity);
                    mergeFx1.ShowMergeFx(newFruit.id);
                    var mergeFx2 = PoolingManager.Spawn(GameManager.Instance.mergeFx, newFruit.transform.position, Quaternion.identity);
                    mergeFx2.transform.Rotate(new Vector3(0f, 0f, -135f));
                    mergeFx2.ShowMergeFx(newFruit.id);

                    AddFruit(newFruit);
                    StartCoroutine(UIManager.Instance.DeactiveBlockClick());
                    HideFruitsTarget();
                    isUpgradingFruit = false;
                    UIManager.Instance.UITop.SetActive(true);
                    TreeSlider.Instance.lines.gameObject.SetActive(true);
                    TreeSlider.Instance.gameObject.SetActive(true);
                    BoosterSignpost.Instance.gameObject.SetActive(false);
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
        TreeSlider.Instance.lines.gameObject.SetActive(false);
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
                    mainCamera.DOOrthoSize(5f, cameraZoomDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
                    {
                        StartCoroutine(UIManager.Instance.DeactiveBlockClick());
                        TreeSlider.Instance.gameObject.SetActive(true);
                        TreeSlider.Instance.lines.gameObject.SetActive(true);
                        UIManager.Instance.SetCanvasSortingLayer("UI");
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