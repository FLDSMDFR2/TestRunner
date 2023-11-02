using UnityEngine;

public class ColliderTriggers : MonoBehaviour
{
    public delegate void ColliderTrigger(Collider collision);
    public event ColliderTrigger OnColliderTriggerEnter;

    protected virtual void OnTriggerEnter(Collider collision)
    {
        OnColliderTriggerEnter?.Invoke(collision);
    }
}
