using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public CircleCollider2D coll;
    public int id;
    public bool hasCollided;
    public GameObject target;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        target = transform.GetChild(0).gameObject;
        target.SetActive(false);
        anim.Play("Fruit " + (id + 1));
        coll.isTrigger = false;
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
                    MergeFruit(otherFruit);
                }
            }
        }
    }

    private void MergeFruit(Fruit otherFruit)
    {
        var collisionPosition = (transform.position + otherFruit.transform.position) / 2;

        PoolingManager.Despawn(gameObject);
        PoolingManager.Despawn(otherFruit.gameObject);
        FruitBox.Instance.RemoveFruit(this);
        FruitBox.Instance.RemoveFruit(otherFruit);

        this.PostEvent(EventID.On_Change_Score, GameManager.Instance.data.fruitDatas[id].scrore);
        int newId = id + 1;
        var newFruit = PoolingManager.Spawn(GameManager.Instance.data.fruitDatas[newId].fruitPrefab,
            collisionPosition, Quaternion.identity);
        
        var mergeFx1 = PoolingManager.Spawn(GameManager.Instance.mergeFx, newFruit.transform.position, Quaternion.identity);
        mergeFx1.ShowMergeFx(newFruit.id);
        var mergeFx2 = PoolingManager.Spawn(GameManager.Instance.mergeFx, newFruit.transform.position, Quaternion.identity);
        mergeFx2.transform.Rotate(new Vector3(0f, 0f, -135f));
        mergeFx2.ShowMergeFx(newFruit.id);
        
        newFruit.Fall();
        FruitBox.Instance.AddFruit(newFruit);
        FruitBox.Instance.RefreshList();
    }
}