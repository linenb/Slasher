using UnityEngine;
using UnityEngine.EventSystems;

public class XPBarTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (XPSystem.instance != null)
            XPSystem.instance.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (XPSystem.instance != null)
            XPSystem.instance.HideTooltip();
    }
}
