using System.Collections;
using System.Windows.Forms;
using UnityEngine;

public class CameraBarrier : MonoBehaviour
{
    private Map currentMap;
    private Transform cam;
    private CameraFollow followScript;

    private float cameraXOffset;
    private float cameraYOffset;

    private bool ready;
    private void Start()
    {
        ready = false;
        cameraXOffset = 19.2f;
        cameraYOffset = 10.8f;
        cam = RootObjectCache.GetRoot("Main Camera").transform;
        followScript = cam.GetComponent<CameraFollow>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = GetComponent<LoadPrison>().currentMap;
        SetCameraBounds();
        ready = true;
    }
    private void SetCameraBounds()
    {
        float southBound = -.8f;
        float westBound = -.8f;
        float northBound = (currentMap.sizeY * 1.6f) - .8f;
        float eastBound = (currentMap.sizeX * 1.6f) - .8f;

        float mapWidth = eastBound - westBound;
        float mapHeight = northBound - southBound;
        float cameraViewportWidth = cameraXOffset * 2f;
        float cameraViewportHeight = cameraYOffset * 2f;

        // Check if map is smaller than camera viewport
        if (mapWidth < cameraViewportWidth)
        {
            // Center camera horizontally
            float centerX = (westBound + eastBound) / 2f;
            followScript.minX = centerX;
            followScript.maxX = centerX;
        }
        else
        {
            // Normal clamping for X
            followScript.minX = westBound + cameraXOffset;
            followScript.maxX = eastBound - cameraXOffset;
        }

        if (mapHeight < cameraViewportHeight)
        {
            // Center camera vertically
            float centerY = (southBound + northBound) / 2f;
            followScript.minY = centerY;
            followScript.maxY = centerY;
        }
        else
        {
            // Normal clamping for Y
            followScript.minY = southBound + cameraYOffset;
            followScript.maxY = northBound - cameraYOffset;
        }
    }
}
