using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileSetter : MonoBehaviour
{
    public GetGivenData givenDataScript;
    public DataSender senderScript;

    private List<Texture2D> groundTextureList = new List<Texture2D>();
    private List<Texture2D> tileTextureList = new List<Texture2D>();

    private Sprite convertedTileSprite;
    private Sprite convertedGroundSprite;
    private Sprite convertedUndergroundSprite;

    public List<Sprite> groundList = new List<Sprite>();
    public List<Sprite> tileList = new List<Sprite>();


    public void SetTiles(int whatPrison)
    {
        tileTextureList = givenDataScript.tileTextureList;
        groundTextureList = givenDataScript.groundTextureList;

        convertedTileSprite = Texture2DToSprite(tileTextureList[whatPrison]);
        convertedGroundSprite = Texture2DToSprite(groundTextureList[whatPrison]);

        convertedUndergroundSprite = Texture2DToSprite(groundTextureList[18]);
        SliceAndDice();

        senderScript.SetKnownLists(tileList, convertedGroundSprite, convertedUndergroundSprite);
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        // Set the filter mode to Point (no filter) for pixel-perfect rendering
        texture.filterMode = FilterMode.Point;

        // Create the sprite with the appropriate pixels per unit
        float pixelsPerUnit = 100f;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
    }

    private void SliceAndDice()
    {
        Texture2D tileTexture = convertedTileSprite.texture;

        for (int y = 0; y < tileTexture.height; y += 16)
        {
            for (int x = 0; x < tileTexture.width; x += 16)
            {
                Rect rect = new Rect(x, y, 16, 16);
                Sprite subSprite = Sprite.Create(tileTexture, rect, new Vector2(0.5f, 0.5f), 100f);
                tileList.Add(subSprite);
            }
        }
    }
}
