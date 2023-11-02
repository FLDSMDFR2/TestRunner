using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI HightScoreLabel;
    public TextMeshProUGUI KilledByLabel;
    public TextMeshProUGUI KilledByDtlsLabel;
    public GameObject NewHighScorelabel;

    public virtual void InitGameOver()
    {
        KilledByLabel.text = "Killed By : " + PlayerData.LastKilledBy;
        KilledByDtlsLabel.text = PlayerData.LastKilledByDtls;

        ScoreLabel.text = PlayerData.PlayerScore.ToString();

        CheckForNewHighScore();

        PlayerData.PlayerScore = 0;
    }

    protected virtual void CheckForNewHighScore()
    {
        if (PlayerData.PlayerScore > PlayerData.PlayerHighScore)
        {
            PlayerData.PlayerHighScore = PlayerData.PlayerScore;
            NewHighScorelabel.SetActive(true);
        }
        else
        {
            NewHighScorelabel.SetActive(false);
        }

        HightScoreLabel.text = PlayerData.PlayerHighScore.ToString();
    }
}
