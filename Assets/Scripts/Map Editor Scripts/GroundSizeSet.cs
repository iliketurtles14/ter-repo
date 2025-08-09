using System.Collections;
using UnityEngine;

public class GroundSizeSet : MonoBehaviour
{
    public Transform gridLines;
    private void Start()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        SetSize();
    }
    public void SetSize()
    {
        int x = gridLines.GetComponent<RuntimeGrid>().sizeX;
        int y = gridLines.GetComponent<RuntimeGrid>().sizeY;

        if(transform.Find("Ground").GetComponent<SpriteRenderer>().drawMode == SpriteDrawMode.Tiled)
        {
            transform.Find("Ground").GetComponent<SpriteRenderer>().size = new Vector2(.16f * x, .16f * y);
        }
        transform.Find("Ground").position = new Vector2((x - 1) * .8f, (y - 1) * .8f);
        transform.Find("Underground").GetComponent<SpriteRenderer>().size = new Vector2(.16f * x, .16f * y);
        transform.Find("Underground").position = new Vector2((x - 1) * .8f, (y - 1) * .8f);
        transform.Find("Vent").GetComponent<SpriteRenderer>().size = new Vector2(.16f * x, .16f * y);
        transform.Find("Vent").position = new Vector2((x - 1) * .8f, (y - 1) * .8f);
        transform.Find("Roof").GetComponent<SpriteRenderer>().size = new Vector2(.16f * x, .16f * y);
        transform.Find("Roof").position = new Vector2((x - 1) * .8f, (y - 1) * .8f);
        transform.Find("Zones").GetComponent<SpriteRenderer>().size = new Vector2(.16f * x, .16f * y);
        transform.Find("Zones").position = new Vector2((x - 1) * .8f, (y - 1) * .8f);
    }
}
