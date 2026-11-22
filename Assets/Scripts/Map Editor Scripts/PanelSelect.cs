using UnityEngine;

public class PanelSelect : MonoBehaviour
{
    public string currentPanel = "FilePanel";
    private void Update()
    {
        if (string.IsNullOrEmpty(currentPanel))
        {
            currentPanel = "FilePanel";
        }
    }
}
