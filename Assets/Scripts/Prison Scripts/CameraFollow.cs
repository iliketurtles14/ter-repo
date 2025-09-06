using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public	float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    private Transform target;

    private void Start()
    {
        target = RootObjectCache.GetRoot("Player").transform;
    }
    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
