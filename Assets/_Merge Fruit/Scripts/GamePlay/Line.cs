using UnityEngine;

public class Line : MonoBehaviour
{
    private float speed = 0.75f;
    private float yStart = 2.5f;
    private float yActive = 2.5f;
    private float yDeactive = -3f;
    [SerializeField] private SpriteRenderer sr;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += Vector3.down * (speed * Time.deltaTime);

        if (transform.position.y <= yActive)
        {
            sr.enabled = true;
        }

        if (transform.position.y <= yDeactive)
        {
            sr.enabled = false;
            transform.position = new Vector3(transform.position.x, yStart, transform.position.z);
        }
    }
}
