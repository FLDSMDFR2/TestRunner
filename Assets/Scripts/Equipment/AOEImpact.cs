using UnityEngine;
using UnityEngine.EventSystems;

public class AOEImpact : MonoBehaviour, IDamager
{
    public string Name;

    public int Damage;
    public int Radius;
    public float Duration;
    public ExperienceBase ExperienceBase;
    public GameObject EffectArea;
    public GameObject GroundEffect;

    protected bool active;

    protected float scaleTime;

    #region IDamager
    public void SetDamage(int damage) 
    {
        Damage = damage;
    }

    public int GetDamage()
    {
        return Damage;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetDtls()
    {
        return "AOEImpact Dtls";
    }
    #endregion

    protected virtual void FixedUpdate()
    {
        if (!active) return;

        scaleTime += Time.deltaTime + Duration;
        EffectArea.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(Radius * 2, Radius * 2, Radius * 2), scaleTime / Radius);
        GroundEffect.transform.localScale = new Vector3(EffectArea.transform.localScale.x, GroundEffect.transform.localScale.y, EffectArea.transform.localScale.z);
        if (EffectArea.transform.localScale.x >= Radius * 2)
        {
            active = false;
            var eventTrigger = EffectArea.GetComponent<ColliderTriggers>();
            if (eventTrigger != null) eventTrigger.OnColliderTriggerEnter -= EventTrigger_OnColliderTriggerEnter;
            this.gameObject.SetActive(false);
        }
    }

    public virtual void TriggerImpact(Vector3 location, ExperienceBase ExperienceBase, int Damage, int Radius, float Duration)
    {
        this.Damage = Damage;
        this.Radius = Radius;
        this.Duration = Duration;
        this.ExperienceBase = ExperienceBase;

        var eventTrigger = EffectArea.GetComponent<ColliderTriggers>();
        if (eventTrigger != null) eventTrigger.OnColliderTriggerEnter += EventTrigger_OnColliderTriggerEnter;

        EffectArea.transform.position = location;
        EffectArea.transform.localScale = Vector3.zero;
        scaleTime = 0;

        GroundEffect.transform.position = new Vector3(location.x, .1f, location.z);
        GroundEffect.transform.localScale = new Vector3(0,.001f,0);

        active = true;
    }

    protected virtual void EventTrigger_OnColliderTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Player>() != null) return;

        var damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) return;
        damageable.TakeDamage(this, ExperienceBase, null);
    }
}
