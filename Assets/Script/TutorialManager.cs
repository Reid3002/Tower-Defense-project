using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject tutorialPanel;
    public Image tutorialImage;
    public Sprite[] tutorialSprites;
    public Button continueButton;
    public TMP_Text continueButtonText;

    private int currentIndex = 0;

    void Start()
    {
        if (PlayerPrefs.GetInt("HasPlayedBefore", 0) == 1)
        {
            tutorialPanel.SetActive(false);
            return;
        }
        tutorialPanel.SetActive(true);
        currentIndex = 0;
        UpdateTutorial();
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    void OnContinueClicked()
    {
        currentIndex++;
        if (currentIndex < tutorialSprites.Length)
        {
            UpdateTutorial();
        }
        else
        {
            PlayerPrefs.SetInt("HasPlayedBefore", 1);
            tutorialPanel.SetActive(false);
            // Aquí podrías iniciar el juego, desactivar bloqueo, etc.
        }
    }

    void UpdateTutorial()
    {
        tutorialImage.sprite = tutorialSprites[currentIndex];
        if (currentIndex == tutorialSprites.Length - 1)
            continueButtonText.text = "Play";
        else
            continueButtonText.text = "Continue";
    }
#if UNITY_EDITOR
    [ContextMenu("Reset Tutorial")]
    public void ResetTutorialFlag()
    {
        PlayerPrefs.SetInt("HasPlayedBefore", 0);
        Debug.Log("Se reseteó el flag del tutorial. Se mostrará nuevamente al iniciar la escena.");
    }
#endif

}
