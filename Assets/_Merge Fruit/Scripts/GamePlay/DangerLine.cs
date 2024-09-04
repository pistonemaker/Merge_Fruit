using UnityEngine;

public class DangerLine : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator anim;
    public Transform left;
    public Transform right;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
        EventDispatcher.Instance.RegisterListener(EventID.On_Check_Danger, CheckDanger);
    }

    private void CheckDanger(object param)
    {
        Vector2 direction = (right.position - left.position).normalized;
        float distance = Vector2.Distance(left.position, right.position);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(left.position, direction, distance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("Fruit"))
            {
                anim.enabled = true;
                EventDispatcher.Instance.PostEvent(EventID.On_Check_Dead);
                return;
            }
        }
        
        anim.enabled = false;
        sr.color = Color.white;
    }
}
