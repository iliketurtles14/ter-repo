using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public Routine routineScript;

    public void Update()
    {
        var light2D = GetComponent<Light2D>();

        // Define colors (normalized)
        Color nightColor = new Color(95f / 255f, 110f / 255f, 255f / 255f);
        Color dayColor = new Color(1f, 1f, 1f);

        // Morning transition: 7:00 to 9:00 (increase intensity and whiten color)
        if (routineScript.min >= 7 && routineScript.min < 9)
        {
            float totalMinutes = (routineScript.min - 7) + (routineScript.sec / 60.0f);
            float t = Mathf.Clamp01(totalMinutes / 2.0f); // 0 at 7:00, 1 at 9:00
            light2D.intensity = Mathf.Lerp(0.1f, 1f, t);
            light2D.color = Color.Lerp(nightColor, dayColor, t);
        }
        // Night transition: 21:00 to 23:00 (decrease intensity and blue the color)
        else if (routineScript.min >= 21 && routineScript.min < 23)
        {
            float totalMinutes = (routineScript.min - 21) + (routineScript.sec / 60.0f);
            float t = Mathf.Clamp01(totalMinutes / 2.0f); // 0 at 21:00, 1 at 23:00
            light2D.intensity = Mathf.Lerp(1f, 0.1f, t);
            light2D.color = Color.Lerp(dayColor, nightColor, t);
        }
        // Day: after 9:00 and before 21:00
        else if (routineScript.min >= 9 && routineScript.min < 21)
        {
            light2D.intensity = 1f;
            light2D.color = dayColor;
        }
        // Night: after 23:00 and before 7:00
        else
        {
            light2D.intensity = 0.1f;
            light2D.color = nightColor;
        }
    }
}
