using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameHighScore : MonoBehaviour
{
    protected TextMeshProUGUI Label;
    public virtual void Awake()
    {
        Label = gameObject.GetComponent<TextMeshProUGUI>();
        GameEventSystem.Game_Resume += GameEventSystem_Game_Resume;  
    }

    private void GameEventSystem_Game_Resume()
    {
        if (Label != null) Label.text = PlayerData.PlayerHighScore.ToString();
    }
}
