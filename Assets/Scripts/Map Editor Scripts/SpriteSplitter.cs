using System.Collections.Generic;
using UnityEngine;

public class SpriteSplitter : MonoBehaviour
{
    public Texture2D sourceTexture;
    public List<Sprite> sprites = new List<Sprite>();

    void Start()
    {
        if (sourceTexture != null)
            SplitTexture();
    }

    public void SplitTexture()
    {
        sprites.Clear();

        int spriteWidth = 16;
        int spriteHeight = 16;
        int columns = sourceTexture.width / spriteWidth;   // 64/16 = 4
        int rows = sourceTexture.height / spriteHeight;    // 320/16 = 20

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Unity's y=0 is bottom, so invert row for top-to-bottom order
                int y = sourceTexture.height - ((row + 1) * spriteHeight);
                Rect rect = new Rect(col * spriteWidth, y, spriteWidth, spriteHeight);
                Vector2 pivot = new Vector2(0.5f, 0.5f);

                Sprite sprite = Sprite.Create(sourceTexture, rect, pivot, 16f);
                sprites.Add(sprite);
            }
        }
    }
}