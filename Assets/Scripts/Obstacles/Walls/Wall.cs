using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : Obstacle, IDamageable, IDamager, ISpawnable
{
    public int MaxHealthRange;
    public int MinHealthRange;

    [ReadOnlyInspector]
    [SerializeField]
    public int MaxHealth;
    [ReadOnlyInspector]
    [SerializeField]
    protected int currentHealth;

    public int Points;

    public DamagePopup DamagePopup;

    #region IDamageable
    public virtual void SetHealth(int health)
    {
        currentHealth = health;
        GameEventSystem.Wall_OnHealthUpdate(this);
    }

    public virtual int GetHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(IDamager damage, ExperienceBase experienceBase, List<StatusModifier> statusModifiers)
    {
        //if we are over killing then just return
        if (GetHealth() <= 0) return;

        var newHealth = GetHealth() - damage.GetDamage();

        var totalDamage = damage.GetDamage();
        if (newHealth <= 0) totalDamage = GetHealth();

        if (experienceBase != null)
        {
            experienceBase.GainExperience(totalDamage);
        }

        if (DamagePopup != null) this.DamagePopup.Create(transform.position, totalDamage, 5, ColorsHelper.GetBaseDamageColor());

        SetHealth(newHealth);

        IsWallDestroyed();
    }
    #endregion

    #region IDamager
    // currently will just do health value worth of damage
    public void SetDamage(int damage){ }

    public int GetDamage()
    {
        return GetHealth();
    }

    public string GetDtls()
    {
        return "Smashed by a wall!";
    }
    #endregion

    #region ISpawnable
    public override void ResetData()
    {
        SetHealth(MaxHealth);
        Points = MaxHealth;
    }
    #endregion

    protected virtual void IsWallDestroyed()
    {
        if (currentHealth <= 0 && this != null && !gameObject.IsDestroyed())
        {
            GameEventSystem.Wall_OnDestroyed(this);
            this.gameObject.SetActive(false);
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);

        // hit player
        var player = collision.gameObject.GetComponentInChildren<Player>();
        if (player != null)
        {
            // appley current health as damage
            player.TakeDamage(this, null, null);

            // if after we hit the player the player is still alive
            // then we take damage = to current health so we should be dead
            if (player.GetHealth() > 0) TakeDamage(this, null, null);
        }
    }
}
