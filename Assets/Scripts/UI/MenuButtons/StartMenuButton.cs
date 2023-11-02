using UnityEngine;

public class StartMenuButton : MonoBehaviour
{
    public void OnStartClick()
    {
        GameEventSystem.UI_OnStartClick();
    }
}
