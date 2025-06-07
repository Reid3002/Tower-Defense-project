using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultSceneController : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text wavesText;
    public TMP_Text sessionXpText;
    public TMP_Text totalXpText;

    void Start()
    {
        var data = ResultData.GetData();
        timeText.text = $"Tiempo de juego: {FormatTime(data.timePlayed)}";
        wavesText.text = $"Oleadas completadas: {data.wavesCompleted}";
        sessionXpText.text = $"XP Ganada: {data.sessionExperience}";
        totalXpText.text = $"XP Total Acumulada: {PlayerExperienceManager.Instance.GetTotalExperience()}";
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPlayAgainButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    private string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{mins:D2}:{secs:D2}";
    }
}
