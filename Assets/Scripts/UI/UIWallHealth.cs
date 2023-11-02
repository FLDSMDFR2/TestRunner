using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWallHealth : MonoBehaviour
{
    protected TextMeshProUGUI Label;
    protected Wall wallScript;

    public virtual void Awake()
    {
        wallScript = gameObject.GetComponent<Wall>();
        Label = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        GameEventSystem.Wall_HealthUpdate += GameEventSystem_Wall_HealthUpdate;
    }

    protected virtual void GameEventSystem_Wall_HealthUpdate(Wall data)
    {
        if (Label != null && wallScript != null && wallScript == data) Label.text = data.GetHealth().ToString();
    }
}
