using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public	float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    public float minX = float.NegativeInfinity;
    public float maxX = float.PositiveInfinity;
    public float minY = float.NegativeInfinity;
    public float maxY = float.PositiveInfinity;

    private Transform target;

    private void Start()
    {
        target = RootObjectCache.GetRoot("Player").transform;
    }
    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
