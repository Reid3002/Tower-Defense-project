using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public UpgradesPanelUI upgradesPanel; // Asignalo desde el Inspector

    private void Start()
    {
        // Al entrar al menú, refrescá la UI de upgrades y XP
        if (upgradesPanel != null)
            upgradesPanel.RefreshUI();
    }
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnExitButtonPressed()
    {
        Application.Quit();
    }
}
