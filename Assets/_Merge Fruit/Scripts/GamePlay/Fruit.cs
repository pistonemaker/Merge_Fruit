using System;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D coll;
    public int id;
    public bool hasCollided;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        rb.gravityScale = 0f;
        hasCollided = false;
    }

    public void Fall()
    {
        rb.gravityScale = 1f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Fruit"))
        {
            EventDispatcher.Instance.PostEvent(EventID.On_Check_Danger);
            Fruit otherFruit = other.gameObject.GetComponent<Fruit>();

            if (!hasCollided && !otherFruit.hasCollided)
            {
                if (otherFruit != null && otherFruit.id == this.id)
                {
                    hasCollided = otherFruit.hasCollided = true;
                    MergeFruit(other.gameObject);
                }
            }
        }
    }

    private void MergeFruit(GameObject otherFruit)
    {
        var collisionPosition = (transform.position + otherFruit.transform.position) / 2;

        PoolingManager.Despawn(gameObject);
        PoolingManager.Despawn(otherFruit);

        int newId = id + 1;
        var newFruit = PoolingManager.Spawn(GameManager.Instance.data.fruitDatas[newId].fruitPrefab,
            collisionPosition, Quaternion.identity);
        newFruit.Fall();
    }
}