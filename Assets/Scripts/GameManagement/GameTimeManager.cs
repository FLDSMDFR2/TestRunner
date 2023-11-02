
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public float GameSpeed;

    protected static float gameSpeed;

    protected virtual void Awake()
    {
        SetGameSpeed(GameSpeed);
    }
    private void OnValidate()
    {
        SetGameSpeed(GameSpeed);
    }

    public static float GetGameSpeed()
    {
        return gameSpeed;
    }

    public static void SetGameSpeed(float speed)
    {
        gameSpeed = speed;
    }

    public static float GetSpeedByTime(float speed)
    {
        return (gameSpeed * speed);
    }
}
