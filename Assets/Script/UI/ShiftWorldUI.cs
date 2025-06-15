using UnityEngine;
using UnityEngine.UI;

public class ShiftWorldUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image fillImage; // Image tipo Filled (asignar en Inspector)

    private WorldManager worldManager;

    void Start()
    {
        worldManager = WorldManager.Instance;
        if (worldManager == null)
        {
            Debug.LogError("[ShiftWorldUI] No se encontr� WorldManager en la escena.");
            enabled = false;
            return;
        }

        // Opcional: suscribite al evento si quer�s reaccionar visualmente al cambio de mundo
        WorldManager.OnWorldChanged += OnWorldChanged;

        UpdateBar();
    }

    void OnDestroy()
    {
        WorldManager.OnWorldChanged -= OnWorldChanged;
    }

    void Update()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (worldManager == null) return;

        float cooldown = GetShiftCooldown();
        float elapsed = Time.time - GetLastShiftTime();
        float t = Mathf.Clamp01(elapsed / cooldown);
        fillImage.fillAmount = t;
    }

    private float GetShiftCooldown()
    {
        // Usamos serialized/private, as� acced�s a shiftCooldown
        var type = typeof(WorldManager);
        var field = type.GetField("shiftCooldown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (float)field.GetValue(worldManager);
    }

    private float GetLastShiftTime()
    {
        var type = typeof(WorldManager);
        var field = type.GetField("lastShiftTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (float)field.GetValue(worldManager);
    }

    private void OnWorldChanged(WorldState _)
    {
        // Si quer�s un feedback especial, ponelo ac�
        // Ejemplo: animar la barra o flashear color
    }

    // Llam� esto si tu bot�n de UI quiere saber si est� listo para shift
    public bool CanShiftWorld()
    {
        float cooldown = GetShiftCooldown();
        float elapsed = Time.time - GetLastShiftTime();
        return elapsed >= cooldown;
    }
}
