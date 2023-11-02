using UnityEngine;

public class RestartMenuButton : MonoBehaviour
{
    public void OnRestartClick()
    {
        GameEventSystem.UI_OnRestartClick();
    }
}
