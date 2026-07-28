using UnityEngine;
using UnityEngine.UI;

public class CanvasRebuilder : MonoBehaviour
{
    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
