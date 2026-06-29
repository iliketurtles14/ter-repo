using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private Camera mainCamera;
    public float speed;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    public IEnumerator StartCredits()
    {
        while (true)
        {
            mainCamera.transform.position += new Vector3(0, speed * -1 * Time.deltaTime);
            yield return null;
        }
    }
}
