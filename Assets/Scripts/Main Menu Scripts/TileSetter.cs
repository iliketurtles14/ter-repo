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

    public List<Sprite> groundList = new List<Sprite>();
    public List<Sprite> tileList = new List<Sprite>();
    public List<Sprite> perksList = new List<Sprite>();
    public List<Sprite> stalagList = new List<Sprite>();
    public List<Sprite> shanktonList = new List<Sprite>();
    public List<Sprite> jungleList = new List<Sprite>();
    public List<Sprite> sanpanchoList = new List<Sprite>();
    public List<Sprite> hmpList = new List<Sprite>();

    public void SetTiles(int whatPrison)//starting from tutorial, being 0
    {
        tileTextureList = givenDataScript.tileTextureList;
        groundTextureList = givenDataScript.groundTextureList;

        switch (whatPrison)
        {
            case 0: convertedTileSprite = Texture2DToSprite(tileTextureList[0]);
                convertedGroundSprite = Texture2DToSprite(groundTextureList[17]);
                break;
            case 1: convertedTileSprite = Texture2DToSprite(tileTextureList[0]);
                convertedGroundSprite = Texture2DToSprite(groundTextureList[11]);
                break;
            case 2: convertedTileSprite = Texture2DToSprite(tileTextureList[1]);
                convertedGroundSprite = Texture2DToSprite(groundTextureList[15]);
                break;
            case 3: convertedTileSprite = Texture2DToSprite(tileTextureList[2]);
                convertedGroundSprite = Texture2DToSprite(groundTextureList[13]);
                break;
            case 4: convertedTileSprite = Texture2DToSprite(tileTextureList[3]);
                convertedGroundSprite = Texture2DToSprite(groundTextureList[9]);
                break;
            case 5: convertedTileSprite = Texture2DToSprite(tileTextureList[4]);
                convertedGroundSprite = Texture2DToSprite(groundTextureList[12]);
                break;
            case 6: convertedTileSprite = Texture2DToSprite(tileTextureList[5]);
                convertedGroundSprite = Texture2DToSprite(groundTextureList[8]);
                break;
        }
        SliceAndDice();

        senderScript.SetKnownLists(tileList, convertedGroundSprite);
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        // Set the filter mode to Point (no filter) for pixel-perfect rendering
        texture.filterMode = FilterMode.Point;

        // Create the sprite with the appropriate pixels per unit
        float pixelsPerUnit = 100f;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
    }

    //private void SeparateTilesIntoLists()
    //{
    //    List<List<Sprite>> tileLists = new List<List<Sprite>> { perksList, stalagList, shanktonList, jungleList, sanpanchoList, hmpList };

    //    for (int i = 0; i < tileList.Count; i++)
    //    {
    //        Sprite tileSprite = tileList[i];
    //        Texture2D tileTexture = tileSprite.texture;

    //        for (int y = 0; y < tileTexture.height; y += 16)
    //        {
    //            for (int x = 0; x < tileTexture.width; x += 16)
    //            {
    //                Rect rect = new Rect(x, y, 16, 16);
    //                Sprite subSprite = Sprite.Create(tileTexture, rect, new Vector2(0.5f, 0.5f), 100f);
    //                tileLists[i].Add(subSprite);
    //            }
    //        }
    //    }
    //}
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
