using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IDamager, ISpawnable
{
    public string Name;

    public int Points;

    public int MaxHealth;

    [ReadOnlyInspector]
    [SerializeField]
    protected int currentHealth;

    public float Speed;

    public float TargetingDistance;

    public float TargetingSpeedChange;

    [ReadOnlyInspector]
    public GameObject Target;

    public GameObject TargetSearch;

    protected virtual void Awake()
    {
        if (TargetSearch != null)
        {
            var collider = TargetSearch.GetComponent<SphereCollider>();
            if (collider) collider.radius = TargetingDistance;

            var colliderTrigger = TargetSearch.GetComponent<ColliderTriggers>();
            if (colliderTrigger) colliderTrigger.OnColliderTriggerEnter += ColliderTrigger_OnColliderTriggerEnter;
        }
    }

    protected virtual void ColliderTrigger_OnColliderTriggerEnter(Collider collision)
    {
        // target player
        if (collision.gameObject.GetComponentInChildren<Player>())
        {
            Target = collision.gameObject;
        }
    }

    protected virtual void Update()
    {
        if (GameStateData.IsPaused) return;

        if (Target == null)
        {
            // move down screen
            transform.Translate(Vector3.forward * MoveSpeed() * Time.deltaTime);
        }
        else
        {
            // move gate to player
            var targetMovement = -Vector3.Normalize(Target.transform.position - transform.position);
            targetMovement.y = 0;
            transform.Translate(targetMovement * (MoveSpeed() * Time.deltaTime));
        }
    }

    protected virtual float MoveSpeed()
    {
        return GameTimeManager.GetSpeedByTime(Speed);
    }

    #region IDamageable
    public virtual void SetHealth(int health)
    {
        this.currentHealth = health;
    }
    public virtual int GetHealth()
    {
        return currentHealth;
    }

    public virtual void TakeDamage(IDamager damage)
    {
        SetHealth(currentHealth - damage.GetDamage());

        IsEnemyKilled();
    }
    #endregion

    #region IDamager
    // currently will just do health value worth of damage
    public void SetDamage(int damage) { }

    public int GetDamage()
    {
        return GetHealth();
    }

    public string GetName()
    {
        return Name;
    }

    public string GetDtls()
    {
        return "Devoured by the undead!";
    }
    #endregion

    #region ISpawnable
    public virtual void RestData()
    {
        SetHealth(MaxHealth);
        Points = MaxHealth;
    }
    #endregion

    protected virtual void IsEnemyKilled()
    {
        if (currentHealth <= 0 && this != null && !gameObject.IsDestroyed())
        {
            GameEventSystem.Enemy_OnEnemyKilled(this);
            this.gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        // perform attacks
        var player = collision.gameObject.GetComponentInChildren<Player>();
        if (player != null)
        {
            // appley current health as damage
            player.TakeDamage(this);

            // if after we hit the player the player is still alive
            // then we take damage = to current health so we should be dead
            if (player.GetHealth() > 0) TakeDamage(this); 
        }

        if (collision.gameObject.tag == "Finish")
        {
            // once object is off screen delete it
            this.gameObject.SetActive(false);
        }
    }
}
