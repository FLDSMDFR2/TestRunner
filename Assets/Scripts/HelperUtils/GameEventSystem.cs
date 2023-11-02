public class GameEventSystem
{
    #region UI Events
    public delegate void UIEvent();
    public static event UIEvent UI_StartClick;
    public static event UIEvent UI_RestartClick;

    public static void UI_OnStartClick()
    {
        UI_StartClick?.Invoke();
    }

    public static void UI_OnRestartClick()
    {
        UI_RestartClick?.Invoke();
    }
    #endregion

    #region Game Events
    public delegate void GameEvent();
    public static event GameEvent Game_Pause;
    public static event GameEvent Game_Resume;
    public static event GameEvent Game_Start;
    public static event GameEvent Game_Over;

    public static void Game_OnPause()
    {
        Game_Pause?.Invoke();
    }
    public static void Game_OnResume()
    {
        Game_Resume?.Invoke();
    }
    public static void Game_OnStart()
    {
        Game_Start?.Invoke();
    }
    public static void Game_OnGameOver()
    {
        Game_Over?.Invoke();
    }
    #endregion

    #region Player Events
    public delegate void PlayerEvent(Player data);
    public static event PlayerEvent Player_Killed;
    public static event PlayerEvent Player_HealthUpdate;
    public static event PlayerEvent Player_Paused;
    public static event PlayerEvent Player_TriggerActiveAbility;
    public static event PlayerEvent Player_ToggleAbilityLeft;
    public static event PlayerEvent Player_ToggleAbilityRight;
    public static event PlayerEvent Player_ActiveAbilityUpdate;
    public static event PlayerEvent Player_ResetData;
    public static void Player_OnKilled(Player data)
    {
        Player_Killed?.Invoke(data);
    }
    public static void Player_OnHealthUpdate(Player data)
    {
        Player_HealthUpdate?.Invoke(data);
    }
    public static void Player_OnPaused(Player data)
    {
        Player_Paused?.Invoke(data);
    }
    public static void Player_OnTriggerActiveAbility(Player data)
    {
        Player_TriggerActiveAbility?.Invoke(data);
    }
    public static void Player_OnToggleAbilityLeft(Player data)
    {
        Player_ToggleAbilityLeft?.Invoke(data);
    }
    public static void Player_OnToggleAbilityRight(Player data)
    {
        Player_ToggleAbilityRight?.Invoke(data);
    }
    public static void Player_OnActiveAbilityUpdate(Player data)
    {
        Player_ActiveAbilityUpdate?.Invoke(data);
    }
    public static void Player_OnResetData(Player data)
    {
        Player_ResetData?.Invoke(data);
    }
    #endregion

    #region Enemy Events
    public delegate void EnemyEvent(Enemy data);
    public static event EnemyEvent Enemy_Killed;

    public static void Enemy_OnEnemyKilled(Enemy data)
    {
        Enemy_Killed?.Invoke(data);
    }
    #endregion

    #region Wall Events
    public delegate void WallEvent(Wall data);
    public static event WallEvent Wall_HealthUpdate;
    public static event WallEvent Wall_Destroyed;
    public static void Wall_OnHealthUpdate(Wall data)
    {
        Wall_HealthUpdate?.Invoke(data);
    }
    public static void Wall_OnDestroyed(Wall data)
    {
        Wall_Destroyed?.Invoke(data);
    }
    #endregion
}
