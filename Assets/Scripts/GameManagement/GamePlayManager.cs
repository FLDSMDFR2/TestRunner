using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    protected virtual void Awake()
    {
        GameEventSystem.Player_Paused += GameEventSystem_Player_Paused;
        GameEventSystem.Game_Pause += GameEventSystem_Game_Pause;
        GameEventSystem.Game_Resume += GameEventSystem_Game_Resume;
        GameEventSystem.Game_Over += GameEventSystem_Game_Over;
        GameEventSystem.Game_Start += GameEventSystem_Game_Start;
    }

    private void GameEventSystem_Game_Start()
    {
        GameStateData.IsPaused = false;
        GameStateData.IsGameOver = false;
    }

    protected virtual void GameEventSystem_Game_Over()
    {
        GameStateData.IsPaused = true;
        GameStateData.IsGameOver = true;
    }

    protected virtual void GameEventSystem_Game_Resume()
    {
        GameStateData.IsPaused = false;
    }

    protected virtual void GameEventSystem_Game_Pause()
    {
        GameStateData.IsPaused = true;
    }

    protected virtual void GameEventSystem_Player_Paused(Player data)
    {
        HandleGamePause();
    }

    protected virtual void HandleGamePause()
    {
        GameStateData.IsPaused = !GameStateData.IsPaused;
    }
}
