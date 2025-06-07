using UnityEngine;

public class PreviewClickable : MonoBehaviour
{
    private TileExpansion tileData;
    private Vector3 basePosition;

    public void Initialize(TileExpansion tile, Vector3 position)
    {
        tileData = tile;
        basePosition = position;
    }

    private void OnMouseDown()
    {
        // Generar camino real al hacer clic sobre la previsualización
        GridManager.Instance.ApplyTileExpansionAtWorldPosition(tileData, basePosition);
        Destroy(gameObject); // eliminar el preview
    }
}
