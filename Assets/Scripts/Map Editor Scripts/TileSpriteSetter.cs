using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSpriteSetter : MonoBehaviour
{
    public Transform tilesPanel;
    public GameObject tileObject;
    public Texture2D devTiles;
    public List<Sprite> sprites = new List<Sprite>();
    private void Start()
    {
        SetSprites(devTiles);
    }
    public void SetSprites(Texture2D tileset)
    {
        SplitTexture(tileset);
        
        int i = 0;
        foreach (Sprite sprite in sprites)
        {
            GameObject tile = Instantiate(tileObject, tilesPanel.Find("TileGrid"));
            tile.GetComponent<Image>().sprite = sprites[i];
            tile.name = "tile" + i;
            tile.tag = "Button";
            i++;
        }
    }
    private void SplitTexture(Texture2D tileset)
    {
        sprites.Clear();

        int spriteWidth = 16;
        int spriteHeight = 16;
        int columns = tileset.width / spriteWidth;   // 64/16 = 4
        int rows = tileset.height / spriteHeight;    // 320/16 = 20

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Unity's y=0 is bottom, so invert row for top-to-bottom order
                int y = tileset.height - ((row + 1) * spriteHeight);
                Rect rect = new Rect(col * spriteWidth, y, spriteWidth, spriteHeight);
                Vector2 pivot = new Vector2(0.5f, 0.5f);

                Sprite sprite = Sprite.Create(tileset, rect, pivot, 16f);
                sprites.Add(sprite);
            }
        }
    }
}
