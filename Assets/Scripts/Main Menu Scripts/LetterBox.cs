using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class Letterbox : MonoBehaviour
{
    public float targetAspect = 16f / 9f;

    private Camera cam;

    private void Update()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        float aspect = (float)Screen.width / Screen.height;
        if (aspect < targetAspect)
        {
            float height = aspect / targetAspect;

            cam.rect = new Rect(
                0,
                (1 - height) / 2,
                1,
                height
            );
        }
        else
        {
            float width = targetAspect / aspect;

            cam.rect = new Rect(
                (1 - width) / 2,
                0,
                width,
                1
            );
        }
    }
}