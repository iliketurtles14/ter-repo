using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectScrollForwarder : MonoBehaviour, IScrollHandler
{
    public ScrollRect scrollRect;

    public void OnScroll(PointerEventData eventData)
    {
        scrollRect.OnScroll(eventData);
    }
}