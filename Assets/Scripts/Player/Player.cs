using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int MaxHealth;
    [ReadOnlyInspector]
    [SerializeField]
    protected int health;

    protected float lastTriggerTime;

    [ReadOnlyInspector]
    [SerializeField]
    public int ActiveAbilityIndex;
    public List<Ability> Abilities;

    public Transform FireLocation;

    protected virtual void Awake()
    {
        GameEventSystem.Game_Start += ResetPlayer;
        GameEventSystem.Player_TriggerActiveAbility += GameEventSystem_Player_TriggerActiveAbility;
        GameEventSystem.Player_ToggleAbilityLeft += GameEventSystem_Player_ToggleAbilityLeft;
        GameEventSystem.Player_ToggleAbilityRight += GameEventSystem_Player_ToggleAbilityRight;
        lastTriggerTime = Time.time;
    }

    #region Player Abilities
    protected virtual void GameEventSystem_Player_TriggerActiveAbility(Player data)
    {
        if (Abilities == null ||
            Abilities.Count <= 0 ||
            ActiveAbilityIndex < 0 ||
            ActiveAbilityIndex >= Abilities.Count) return;

        Abilities[ActiveAbilityIndex].TriggerAbility(this);
    }

    protected virtual void GameEventSystem_Player_ToggleAbilityRight(Player data)
    {
        UpdateActiveAbilityIndex(ActiveAbilityIndex + 1);
    }

    protected virtual void GameEventSystem_Player_ToggleAbilityLeft(Player data)
    {
        UpdateActiveAbilityIndex(ActiveAbilityIndex - 1);
    }
    protected virtual void UpdateActiveAbilityIndex(int newIndex)
    {
        if (Abilities == null || Abilities.Count <= 0) return;

        if (newIndex < 0) ActiveAbilityIndex = Abilities.Count - 1;
        else if (newIndex > Abilities.Count - 1) ActiveAbilityIndex = 0;
        else ActiveAbilityIndex = newIndex;

        GameEventSystem.Player_OnActiveAbilityUpdate(this);
    }
    #endregion

    protected virtual void ResetPlayer()
    {
        SetHealth(MaxHealth);
        Abilities = GetComponentsInChildren<Ability>().ToList();
        GameEventSystem.Player_OnResetData(this);
        UpdateActiveAbilityIndex(ActiveAbilityIndex);
    }

    #region IDamageable
    public void SetHealth(int Health)
    {
        this.health = Math.Clamp(Health, 0, MaxHealth);
        GameEventSystem.Player_OnHealthUpdate(this);
    }
    public virtual int GetHealth()
    {
        return health;
    }

    public void UpdateHealth(int Health)
    {
        this.health = Math.Clamp(this.health + health, 0, MaxHealth);
        GameEventSystem.Player_OnHealthUpdate(this);
    }

    public void TakeDamage(IDamager damage)
    {
        SetHealth(health - damage.GetDamage());

        IsPlayerKilled(damage);
    }

    #endregion

    public virtual void TriggeredGate()
    {
        lastTriggerTime = Time.time;
    }

    public virtual bool CanTriggerGate()
    {
        return (Time.time - lastTriggerTime) > 3;
    }

    protected virtual void IsPlayerKilled(IDamager damage)
    {
        if (health <= 0 && this != null && !gameObject.IsDestroyed())
        {
            PlayerData.LastKilledBy = damage.GetName();
            PlayerData.LastKilledByDtls = damage.GetDtls();

            GameEventSystem.Player_OnKilled(this);
        }
    }
}
