using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ApplyMainMenuData : MonoBehaviour
{
    public MemoryMappedFileReader mmfrScript;
    private List<Sprite> NPCSprites;
    private List<Sprite> UISprites;
    public Transform mmc;
    private bool hasApplied;

    public void Update()
    {
        if(mmfrScript.canApply && !hasApplied)
        {
            NPCSprites = ConvertTexture2DListToSpriteList(mmfrScript.NPCImages);
            UISprites = ConvertTexture2DListToSpriteList(mmfrScript.UIImages);

            hasApplied = true;

            LoadImages();

        }
    }
    private List<Sprite> ConvertTexture2DListToSpriteList(List<Texture2D> textures)
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (Texture2D texture in textures)
        {
            Sprite sprite = Texture2DToSprite(texture);
            sprites.Add(sprite);
        }
        return sprites;
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        // Set the filter mode to Point (no filter) for pixel-perfect rendering
        texture.filterMode = FilterMode.Point;

        // Create the sprite with the appropriate pixels per unit
        float pixelsPerUnit = 100f; // Adjust this value based on your texture resolution and desired size
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
    }
    public void LoadImages()
    {
        //mouse
        mmc.Find("MouseOverlay").GetComponent<Image>().sprite = UISprites[41];
        //main menu buttons
        mmc.Find("TitlePanel").Find("PlayButton").GetComponent<Image>().sprite = UISprites[198];
        mmc.Find("TitlePanel").Find("OptionsButton").GetComponent<Image>().sprite = UISprites[198];
        mmc.Find("TitlePanel").Find("MapEditorButton").GetComponent<Image>().sprite = UISprites[198];
        //escapists
        mmc.Find("TitlePanel").Find("The Escapists").GetComponent<Image>().sprite = UISprites[222];
        //prison select backdrop
        mmc.Find("PrisonSelectPanel").GetComponent<Image>().sprite = UISprites[208];
        //arrows
        mmc.Find("PrisonSelectPanel").Find("LeftArrow").GetComponent<Image>().sprite = UISprites[203];
        mmc.Find("PrisonSelectPanel").Find("RightArrow").GetComponent<Image>().sprite = UISprites[200];
        //prison select buttons
        mmc.Find("PrisonSelectPanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[248];
        mmc.Find("PrisonSelectPanel").Find("ContinueButton").GetComponent<Image>().sprite = UISprites[248];
        //prison image
        mmc.Find("PrisonSelectPanel").Find("PrisonImage").GetComponent<Image>().sprite = UISprites[519];
        //player menu buttons
        mmc.Find("PlayerPanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[248];
        mmc.Find("PlayerPanel").Find("ContinueButton").GetComponent<Image>().sprite = UISprites[248];
        //arrows
        mmc.Find("PlayerPanel").Find("LeftArrow").GetComponent<Image>().sprite = UISprites[203];
        mmc.Find("PlayerPanel").Find("RightArrow").GetComponent<Image>().sprite = UISprites[200];
        //player menu backdrop
        mmc.Find("PlayerPanel").GetComponent<Image>().sprite = UISprites[184];
        //player backdrop
        mmc.Find("PlayerPanel").Find("NPCBackdrop").GetComponent<Image>().sprite = UISprites[255];
        //name box
        mmc.Find("PlayerPanel").Find("NameBox").GetComponent<Image>().sprite = UISprites[186];
        //small menu panel set button
        mmc.Find("SmallMenuPanel").Find("SetButton").GetComponent<Image>().sprite = UISprites[326];
        //arros
        mmc.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Image>().sprite = UISprites[203];
        mmc.Find("SmallMenuPanel").Find("RightArrow").GetComponent<Image>().sprite = UISprites[200];
        //npc backdrop
        mmc.Find("SmallMenuPanel").Find("NPCBackdrop").GetComponent<Image>().sprite = UISprites[255];
        //name box
        mmc.Find("SmallMenuPanel").Find("NameBox").GetComponent<Image>().sprite = UISprites[186];
        //npc customize panel backdrop
        mmc.Find("NPCCustomizePanel").GetComponent<Image>().sprite = UISprites[320];
        //npc backdrop
        mmc.Find("NPCCustomizePanel").Find("NPCBackdrop").GetComponent<Image>().sprite = UISprites[321];
        //name box
        mmc.Find("NPCCustomizePanel").Find("NameBox").GetComponent<Image>().sprite = UISprites[322];
        //npc customize panel buttons
        mmc.Find("NPCCustomizePanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[326];
        mmc.Find("NPCCustomizePanel").Find("RandomButton").GetComponent<Image>().sprite = UISprites[330];
        mmc.Find("NPCCustomizePanel").Find("StartGameButton").GetComponent<Image>().sprite = UISprites[328];

    }

}