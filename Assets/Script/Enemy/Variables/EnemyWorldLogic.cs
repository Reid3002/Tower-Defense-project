using UnityEngine;

public class EnemyWorldLogic : MonoBehaviour, IWorldAware
{
    private WorldState originWorld;
    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true); // Cacheamos los renderers
    }

    public void SetOriginWorld(WorldState world)
    {
        originWorld = world;
        UpdateVisibility(); // Se aplica de inmediato
    }

    public bool IsTargetable()
    {
        return originWorld == WorldManager.Instance.CurrentWorld;
    }

    public WorldState GetOriginWorld()
    {
        return originWorld;
    }

    public void UpdateVisibility()
    {
        if (WorldManager.Instance == null) return;

        bool isVisible = WorldManager.Instance.CurrentWorld == originWorld;
        SetTransparency(isVisible ? 1f : 0.3f);
    }

    private void SetTransparency(float alpha)
    {
        if (renderers == null) return;

        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.materials)
            {
                if (mat == null) continue;

                SetMaterialToFade(mat);

                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }
    }

    private void SetMaterialToFade(Material material)
    {
        if (material == null) return;

        material.SetFloat("_Mode", 2); // 2 = Fade
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
}
