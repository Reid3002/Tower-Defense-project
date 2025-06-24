using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public UpgradesPanelUI upgradesPanel;

    [SerializeField] private string levelScene = "GameScene";

    public static System.Action OnPlayPressed = delegate { };
    public static System.Action OnExitPressed = delegate { };

    private void Start()
    {
        // Al entrar al menu, refresca la UI de upgrades y XP
        if (upgradesPanel != null)
        {
            upgradesPanel.RefreshUI();
        }

    }
    public void OnPlayButtonPressed()
    {
        OnPlayPressed?.Invoke();
        SceneManager.LoadScene(levelScene);
    }
    public void OnExitButtonPressed()
    {
        OnExitPressed?.Invoke();
        Application.Quit();
    }
}
