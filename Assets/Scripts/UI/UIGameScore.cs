using TMPro;
using UnityEngine;

public class UIGameScore : MonoBehaviour
{
    protected TextMeshProUGUI Label;
    public virtual void Awake()
    {
        Label = gameObject.GetComponent<TextMeshProUGUI>();
        GameEventSystem.Enemy_Killed += GameEventSystem_Enemy_Killed;
        GameEventSystem.Wall_Destroyed += GameEventSystem_Wall_Destroyed;
        GameEventSystem.Game_Resume += UpdateScore;
        UpdateScore();
    }

    private void GameEventSystem_Wall_Destroyed(Wall data)
    {
        PlayerData.PlayerScore += data.Points;
        UpdateScore();
    }

    private void GameEventSystem_Enemy_Killed(Enemy data)
    {
        PlayerData.PlayerScore += data.Points;
        UpdateScore();
    }

    protected virtual void UpdateScore()
    {
        if (Label != null) Label.text = PlayerData.PlayerScore.ToString();
    }
}
