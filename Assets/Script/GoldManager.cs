using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int currentGold = 30;
    public TMP_Text goldText;

    public System.Action<int> OnGoldEarned;

    void Awake()
    {
        Instance = this;
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        CoreUI.Instance?.UpdateUI(); // actualizar estado del botón
        UpdateGoldUI();

        OnGoldEarned?.Invoke(amount);
    }

    public void SpendGold(int amount)
    {
        currentGold -= amount;
        CoreUI.Instance?.UpdateUI(); // actualizar estado del botón
        UpdateGoldUI();
    }

    public bool HasEnoughGold(int amount)
    {
        return currentGold >= amount;
    }

    public void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"Oro: {currentGold}";

    }

}
