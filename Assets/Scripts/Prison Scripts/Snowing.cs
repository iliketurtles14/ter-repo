using System.Collections;
using UnityEngine;

public class Snowing : MonoBehaviour
{
    private Map currentMap;
    private GameObject snowFlake;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
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
        if (!currentMap.snowing)
        {
            enabled = false;
            yield break;
        }

        snowFlake = new GameObject("SnowFlake");
        snowFlake.AddComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        snowFlake.GetComponent<SpriteRenderer>().sprite = DataSender.instance.UIImages[311];
        snowFlake.GetComponent<SpriteRenderer>().size = new Vector2(.5f, .5f);
        snowFlake.GetComponent<SpriteRenderer>().sortingOrder = 100;
        snowFlake.GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
        snowFlake.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 180f / 255f);
        snowFlake.AddComponent<SnowFall>();
        snowFlake.transform.position = new Vector2(9999, 9999);

        StartCoroutine(SnowLoop());
    }
    private IEnumerator SnowLoop()
    {
        while (true)
        {
            float randTime = UnityEngine.Random.Range(.25f, .75f);
            float rotation = UnityEngine.Random.Range(0f, 360f);
            float randX = UnityEngine.Random.Range(-19.2f, 19.2f);//edges of screen if screen position is 0,0
            yield return new WaitForSeconds(randTime);
            GameObject flake = Instantiate(snowFlake);
            flake.name = "SnowFlake";
            flake.transform.position = new Vector3(randX + cam.transform.position.x, 11 + cam.transform.position.y, 0);
            flake.transform.rotation = Quaternion.Euler(0, 0, rotation);
            flake.GetComponent<SnowFall>().speed = UnityEngine.Random.Range(1f, 4f);
            flake.GetComponent<SnowFall>().horizontalMovement = UnityEngine.Random.Range(-.5f, .5f);
            float randScale = UnityEngine.Random.Range(.2f, 1f);
            flake.transform.localScale = new Vector3(randScale, randScale, 1);
        }
    }
}
