using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ApplyMapEditorData : MonoBehaviour
{
    private DataSender senderScript;
    public List<Sprite> ItemSprites = new List<Sprite>();
    public List<Sprite> NPCSprites = new List<Sprite>();
    public List<Sprite> PrisonObjectSprites = new List<Sprite>();
    public List<Sprite> UISprites = new List<Sprite>();

    private void Start()
    {
        senderScript = DataSender.instance;

        ItemSprites = senderScript.ItemImages;
        NPCSprites = senderScript.NPCImages;
        PrisonObjectSprites = senderScript.PrisonObjectImages;
        UISprites = senderScript.UIImages;

        LoadImages();
    }
    private Sprite Cutter(Sprite sprite, int x, int y, int width, int height)
    {
        Rect rect = new Rect(x, y, width, height);
        Texture2D texture = sprite.texture;
        Sprite newSprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f), sprite.pixelsPerUnit);
        return newSprite;
    }
    private void LoadImages()
    {

    }
}
