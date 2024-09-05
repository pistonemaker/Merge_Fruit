using UnityEngine;

public class DeadLine : MonoBehaviour
{
    public Transform left;
    public Transform right;
    public Transform spawnPos;

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Check_Dead, CheckDead);
    }
    
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.On_Check_Dead, CheckDead);
    }

    private void CheckDead(object param)
    {
        Vector2 direction = (right.position - left.position).normalized;
        float distance = Vector2.Distance(left.position, right.position);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(left.position, direction, distance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("Fruit"))
            {
                if (hits[i].collider.transform.position.y > spawnPos.position.y)
                {
                    EventDispatcher.Instance.PostEvent(EventID.On_Player_Dead);
                }
            }
        }
    }
}
