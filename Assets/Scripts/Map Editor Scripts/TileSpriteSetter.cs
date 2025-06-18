using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TileSpriteSetter : MonoBehaviour
{
    public SpriteSplitter spriteSplitterScript;
    public Transform tilesPanel;
    public GameObject tileObject;
    private void Start()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        SetSprites();
    }
    private void SetSprites()
    {
        int i = 0;
        foreach (Sprite sprite in spriteSplitterScript.sprites)
        {
            GameObject tile = Instantiate(tileObject, tilesPanel.Find("TileGrid"));
            tile.GetComponent<Image>().sprite = spriteSplitterScript.sprites[i];
            tile.name = "tile" + i;
            tile.tag = "Button";
            i++;
        }
    }
}
