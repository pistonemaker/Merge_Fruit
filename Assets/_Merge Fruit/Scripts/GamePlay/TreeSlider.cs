using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreeSlider : Singleton<TreeSlider>
{
    public Transform spawnPos;
    public Fruit curFruit;
    public bool isholding = false;
    public bool canhold = true;
    private float firstSpawnPosY;
    private float bound = 2.25f;
    
    private void Start()
    {
        firstSpawnPosY = spawnPos.position.y;
        InitFruit();
    }

    private void InitFruit()
    {
        var fruit = PoolingManager.Spawn(GameManager.Instance.GetFirstFruit(), transform.position, 
            Quaternion.identity);
        curFruit = fruit;
        AttachFruit();
    }
    
    private void AttachFruit()
    {
        transform.position = new Vector3(transform.position.x, firstSpawnPosY + curFruit.coll.radius * 2, 
            transform.position.z);
        spawnPos.transform.position = new Vector3(spawnPos.position.x, firstSpawnPosY, spawnPos.position.z);
        curFruit.transform.position = new Vector3(curFruit.transform.position.x, spawnPos.position.y, curFruit.transform.position.z);
        curFruit.transform.SetParent(transform);
        curFruit.coll.isTrigger = true;
    }

    private IEnumerator DetachFruit()
    {
        canhold = false;
        curFruit.coll.isTrigger = false;
        curFruit.transform.SetParent(null);
        curFruit.Fall();
        yield return new WaitForSeconds(0.5f);
        curFruit = PoolingManager.Spawn(GameManager.Instance.GetRandomFruit(), transform.position, 
            Quaternion.identity);
        AttachFruit();
        canhold = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUIElement())
        {
            isholding = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, transform.position.y, 0);
        }

        if (isholding)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, transform.position.y, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isholding = false;
            if (canhold)
            {
                StartCoroutine(DetachFruit());
            }
        }

        if (transform.position.x < -bound + curFruit.coll.radius + 0.1f)
        {
            transform.position = new Vector3(-bound + curFruit.coll.radius, transform.position.y, transform.position.z);
        }

        if (transform.position.x > bound - curFruit.coll.radius - 0.1f)
        {
            transform.position = new Vector3(bound - curFruit.coll.radius, transform.position.y, transform.position.z);
        }
    }
    
    private bool IsMouseOverUIElement()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
