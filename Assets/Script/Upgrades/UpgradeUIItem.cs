using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeUIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image iconImage; // la imagen principal
    public GameObject tooltipPanel;
    public TMP_Text tooltipNameText;
    public TMP_Text tooltipDescriptionText;
    public TMP_Text tooltipCostText;
    public Image circleSlider; // tipo radial (Image con fill radial)

    [Header("Config")]
    public float unlockHoldTime = 1.5f;

    [Header("Tooltip Offset")]
    //public Vector2 tooltipOffset = new Vector2(240f, 0f); // 120 px a la derecha, ajustá a gusto

    private float holdTimer = 0f;
    private bool isHolding = false;
    private bool canUnlock = true;

    private UpgradeData currentUpgrade;
    private System.Action<string> unlockCallback;

    public void Setup(UpgradeData upgrade, bool unlocked, System.Action<string> onUnlock)
    {
        currentUpgrade = upgrade;
        unlockCallback = onUnlock;

        // Actualizá el icono (si tenés un campo Sprite)
        // iconImage.sprite = upgrade.icon;

        canUnlock = !unlocked;
        // Visual feedback:
        iconImage.color = unlocked ? new Color(1, 1, 1, 0.4f) : Color.white;

        // Ocultá tooltip y reseteá círculo de progreso
        tooltipPanel.SetActive(false);
        circleSlider.fillAmount = 0f;
    }

    void Update()
    {
        if (isHolding && canUnlock)
        {
            holdTimer += Time.deltaTime;
            circleSlider.fillAmount = Mathf.Clamp01(holdTimer / unlockHoldTime);

            if (holdTimer >= unlockHoldTime)
            {
                UnlockUpgrade();
                isHolding = false;
                circleSlider.fillAmount = 0;
            }
        }
    }
    public void ShowTooltipPanel()
    {
        tooltipPanel.SetActive(true);

        RectTransform myRect = GetComponent<RectTransform>();
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();

        // Usar Pivot (0,0.5) en TooltipPanel
        float offsetX = myRect.rect.width / 2f + 20; // 20 px a la derecha del item
        tooltipRect.anchoredPosition = new Vector2(offsetX, 0);

        ClampTooltipToCanvas(tooltipRect);
    }


    void ClampTooltipToCanvas(RectTransform tooltipRect)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 anchoredPos = tooltipRect.anchoredPosition;
        Vector2 size = tooltipRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;

        // Chequeá los bordes
        anchoredPos.x = Mathf.Clamp(anchoredPos.x, 0, canvasSize.x - size.x);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, -canvasSize.y / 2f + size.y / 2f, canvasSize.y / 2f - size.y / 2f);

        tooltipRect.anchoredPosition = anchoredPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltipPanel();

        // Posicionar el tooltip a la derecha del icono
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        tooltipRect.anchoredPosition = new Vector2(200f, 0f); // 100px a la derecha, ajustá a gusto

        // Rellená textos
        tooltipNameText.text = currentUpgrade.upgradeName;
        tooltipDescriptionText.text = currentUpgrade.description;
        tooltipCostText.text = $"Cost: {currentUpgrade.xpCost}";
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
        isHolding = false;
        holdTimer = 0;
        circleSlider.fillAmount = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canUnlock)
        {
            isHolding = true;
            holdTimer = 0;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTimer = 0;
        circleSlider.fillAmount = 0;
    }

    private void UnlockUpgrade()
    {
        // Llamá a tu manager para desbloquear
        // Por ejemplo: OnUnlockUpgrade?.Invoke(currentUpgrade.upgradeId);
        // O podés comunicarte directo con el manager:

        // Desactivá el unlock si ya está hecho
        canUnlock = false;
        iconImage.color = new Color(1, 1, 1, 0.4f);
        tooltipPanel.SetActive(false);
        // ...otros efectos visuales
    }
}
