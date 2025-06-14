using UnityEngine;
using UnityEngine.EventSystems;

public class PointerSuperDebug : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerDown(PointerEventData eventData) => Debug.Log("SUPER DEBUG >>> OnPointerDown");
    public void OnPointerUp(PointerEventData eventData) => Debug.Log("SUPER DEBUG >>> OnPointerUp");
    public void OnPointerEnter(PointerEventData eventData) => Debug.Log("SUPER DEBUG >>> OnPointerEnter");
    public void OnPointerExit(PointerEventData eventData) => Debug.Log("SUPER DEBUG >>> OnPointerExit");
}
