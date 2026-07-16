using UnityEngine;

public class RoofFallCheckerFollow : MonoBehaviour
{
    private Vector3 pos = new Vector3(0, -.16f, 0);
    private void Update()
    {
        transform.localPosition = pos;
    }
}
