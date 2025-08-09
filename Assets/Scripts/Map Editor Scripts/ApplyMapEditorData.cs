using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ApplyMapEditorData : MonoBehaviour
{
    private DataSender senderScript;
    public List<Sprite> ItemSprites = new List<Sprite>();
    public List<Sprite> NPCSprites = new List<Sprite>();
    public List<Sprite> PrisonObjectSprites = new List<Sprite>();
    public List<Sprite> UISprites = new List<Sprite>();

    public Transform uic;

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
    public static Sprite AddPaddingToSprite(Sprite originalSprite, int padding)
    {
        // Get original texture and rect
        Texture2D originalTexture = originalSprite.texture;
        Rect spriteRect = originalSprite.rect;

        int originalWidth = (int)spriteRect.width;
        int originalHeight = (int)spriteRect.height;

        // Copy only sprite area (in case of atlas)
        Texture2D spriteTexture = new Texture2D(originalWidth, originalHeight, TextureFormat.RGBA32, false);
        spriteTexture.filterMode = originalTexture.filterMode; // match original
        spriteTexture.wrapMode = TextureWrapMode.Clamp;

        Color[] pixels = originalTexture.GetPixels(
            (int)spriteRect.x,
            (int)spriteRect.y,
            originalWidth,
            originalHeight
        );
        spriteTexture.SetPixels(0, 0, originalWidth, originalHeight, pixels);
        spriteTexture.Apply();

        // Create padded texture
        int newWidth = originalWidth + padding * 2;
        int newHeight = originalHeight + padding * 2;

        Texture2D paddedTexture = new Texture2D(newWidth, newHeight, originalTexture.format, false);

        // FIX: No blur
        paddedTexture.filterMode = originalTexture.filterMode; // or FilterMode.Point for pixel art
        paddedTexture.wrapMode = TextureWrapMode.Clamp;

        // Fill with transparent
        Color32[] clearPixels = new Color32[newWidth * newHeight];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = new Color32(0, 0, 0, 0);
        }
        paddedTexture.SetPixels32(clearPixels);

        // Copy original into center
        Color32[] originalPixels = spriteTexture.GetPixels32();
        for (int y = 0; y < originalHeight; y++)
        {
            for (int x = 0; x < originalWidth; x++)
            {
                Color32 pixel = originalPixels[y * originalWidth + x];
                paddedTexture.SetPixel(x + padding, y + padding, pixel);
            }
        }

        paddedTexture.Apply();

        // Create new sprite from padded texture
        Sprite paddedSprite = Sprite.Create(
            paddedTexture,
            new Rect(0, 0, newWidth, newHeight),
            new Vector2(0.5f, 0.5f), // pivot
            originalSprite.pixelsPerUnit
        );

        return paddedSprite;
    }
    private void LoadImages()
    {
        Transform objContent = uic.Find("ObjectsPanel").Find("ObjectsScrollRect").Find("Viewport").Find("Content");
        foreach(Transform obj in objContent)
        {
            switch (obj.name)
            {
                case "Sink":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[32];
                    break;
                case "ToiletLeft":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[34];
                    break;
                case "ToiletRight":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[35];
                    break;
                case "SlatsVertical":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[40];
                    break;
                case "Washer":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[46];
                    break;
                case "Oven":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[48];
                    break;
                case "Bookshelf":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[50];
                    break;
                case "NPCDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[51];
                    break;
                case "Benchpress":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[52];
                    break;
                case "Treadmill":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[54];
                    break;
                case "Freezer":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[55];
                    break;
                case "MedicBed":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[56];
                    break;
                case "Table":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[60];
                    break;
                case "FoodTable":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[61];
                    break;
                case "CutleryTable":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[67];
                    break;
                case "DetectorVertical":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[68];
                    break;
                case "Camera":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[72];
                    break;
                case "DirtyLaundry":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[75];
                    break;
                case "CleanLaundry":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[76];
                    break;
                case "Seat":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[78];
                    break;
                case "TimberBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[87];
                    break;
                case "MetalBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[89];
                    break;
                case "PlatesBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[90];
                    break;
                case "LicensePress":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[91];
                    break;
                case "JanitorDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[93];
                    break;
                case "MedicDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[94];
                    break;
                case "BookBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[95];
                    break;
                case "Vent":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[137];
                    break;
                case "SlatsHorizontal":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[139];
                    break;
                case "LadderDown":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[140];
                    break;
                case "PlayerDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[143];
                    break;
                case "GuardBed":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[144];
                    break;
                case "LadderUp":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[147];
                    break;
                case "SolitaryBed":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[148];
                    break;
                case "DetectorHorizontal":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[155];
                    break;
                case "Generator":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[165];
                    break;
                case "WorkDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[174];
                    break;
                case "UtilityDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[175];
                    break;
                case "StaffDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[176];
                    break;
                case "EnteranceDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[177];
                    break;
                case "CellDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[178];
                    break;
                case "JobBoard":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[184];
                    break;
                case "FurnitureBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[186];
                    break;
                case "Locker":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[187];
                    break;
                case "ComputerTable":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[188];
                    break;
                case "Payphone":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[189];
                    break;
                case "WhiteDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[190];
                    break;
                case "DeliveryTruckRight":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[191];
                    break;
                case "DeliveryTruckLeft":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[192];
                    break;
                case "DeliveryTruckUp":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[193];
                    break;
                case "DeliveryTruckDown":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[194];
                    break;
                case "TailorBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[200];
                    break;
                case "ClothesBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[201];
                    break;
                case "RedBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[202];
                    break;
                case "BlueBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[203];
                    break;
                case "MailBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[204];
                    break;
                case "TV":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[205];
                    break;
                case "Lounger":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[206];
                    break;
                case "YardWorkBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[211];
                    break;
                case "RunningMat":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[212];
                    break;
                case "PushupMat":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[213];
                    break;
                case "Stash":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[224];
                    break;
                case "CharlieGate":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[229];
                    break;
                case "PunchingMat":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[236];
                    break;
                case "JumpropeMat":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[238];
                    break;
                case "ZipUp":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[247];
                    break;
                case "ZipDown":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[247];
                    break;
                case "ZipLeft":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[247];
                    break;
                case "ZipRight":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[247];
                    break;
                case "ZipEnd":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[248];
                    break;
                case "PullupBar":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[250];
                    break;
                case "AlcaBoat":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[251];
                    break;
                case "ToiletDown":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[253];
                    break;
                case "SpeedBag":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[259];
                    break;
                case "BlankDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[260];
                    break;
                case "BedVertical":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[264];
                    break;
                case "BedHorizontal":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[265];
                    break;
                case "ETTank":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[268];
                    break;
                case "ETStatueWalkway":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[269];
                    break;
                case "ETSpecialDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[270];
                    break;
                case "ETPlayerDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[271];
                    break;
                case "ETNPCDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[272];
                    break;
                case "ETTable":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[273];
                    break;
                case "ETStatuePlaque":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[278];
                    break;
                case "ETChair":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[283];
                    break;
                case "ETLocker":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[285];
                    break;
                case "ETPhone":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[286];
                    break;
                case "ETStatue":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[287];
                    break;
                case "DTAFCylindar":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[288];
                    break;
                case "DTAFSpecialChairRight":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[290];
                    break;
                case "DTAFSpecialChairLeft":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[291];
                    break;
                case "DTAFComputerStair":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[292];
                    break;
                case "DTAFComputerBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[293];
                    break;
                case "DTAFComputerCylindar":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[294];
                    break;
                case "DTAFSpecialChairUp":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[295];
                    break;
                case "DTAFTableLong":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[296];
                    break;
                case "DTAFPlantTop":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[297];
                    break;
                case "DTAFPlantBottom":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[298];
                    break;
                case "DTAFTruckVertical":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[299];
                    break;
                case "DTAFTruckHorizontal":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[300];
                    break;
                case "DTAFSpecialChairDoubleDown":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[301];
                    break;
                case "DTAFSpecialChairDoubleRight":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[302];
                    break;
                case "DTAFSpecialChairDoubleUp":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[303];
                    break;
                case "DTAFCarpet":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[304];
                    break;
                case "DTAFWorldMap":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[305];
                    break;
                case "DTAFSharkWalkway":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[306];
                    break;
                case "DTAFSpecialDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[307];
                    break;
                case "DTAFComputerLeft":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[308];
                    break;
                case "DTAFComputerRight":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[309];
                    break;
                case "DTAFComputerDown":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[310];
                    break;
                case "DTAFShark":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[317];
                    break;
                case "DTAFPlaque":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[320];
                    break;
                case "DTAFSign":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[323];
                    break;
                case "DTAFRocket":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[324];
                    break;
                case "DTAFWardenChair":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[325];
                    break;
                case "DTAFComputer":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[326];
                    break;
                case "GuardDoor":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[327];
                    break;
                case "SSCandyCaneRightTop":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[328];
                    break;
                case "SSCandyCaneLeftTop":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[329];
                    break;
                case "SSCandyCaneRightBottom":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[330];
                    break;
                case "SSCandyCaneLeftBottom":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[331];
                    break;
                case "SSBoxVertical":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[332];
                    break;
                case "SSBoxHorizontal":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[333];
                    break;
                case "SSLetterBags":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[334];
                    break;
                case "SSPresent1":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[335];
                    break;
                case "SSPresent2":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[336];
                    break;
                case "SSPresent3":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[337];
                    break;
                case "SSLetterBasket":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[339];
                    break;
                case "SSLetterShredder":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[340];
                    break;
                case "SSGuardBedDouble":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[343];
                    break;
                case "SSLamp":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[344];
                    break;
                case "SSSpecialChair":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[346];
                    break;
                case "SSCouch":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[347];
                    break;
                case "SSCarpet":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[348];
                    break;
                case "SSToyWoodBox":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[351];
                    break;
                case "SSSleigh":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[352];
                    break;
                case "SSSign":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[353];
                    break;
                case "SSTree":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[356];
                    break;
                case "ChristmasDesk":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[359];
                    break;
                case "GreenCar":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[367];
                    break;
                case "BlueCar":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[368];
                    break;
                case "BrownCar":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[369];
                    break;
                case "JingleTruck":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[370];
                    break;
                case "SSSantaSleigh":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[372];
                    break;
                case "JingleSantaSleigh":
                    obj.GetComponent<Image>().sprite = PrisonObjectSprites[378];
                    break;
            }
            if(obj.name != "GuardCanteen" && obj.name != "GuardRollcall" && obj.name != "GuardGym" &&
                obj.name != "GuardShower" && obj.name != "GuardWaypoint" && obj.name != "InmateRollcall" &&
                obj.name != "InmateCanteen" && obj.name != "InmateWaypoint" && obj.name != "InmateShower" && 
                !obj.name.StartsWith("Jeep") && obj.name != "JobWaypoint" && obj.name != "MedicWaypoint" &&
                obj.name != "Mines" && obj.name != "NPCSpawnpoint" && obj.name != "Spotlight")
            {
                obj.GetComponent<Image>().sprite = AddPaddingToSprite(obj.GetComponent<Image>().sprite, 1);
                obj.GetComponent<RectTransform>().sizeDelta += new Vector2(10f, 10f);
            }
        }

        //job panel checkboxes
        foreach(Transform child in uic.Find("JobPanel").Find("CheckBoxGrid1"))
        {
            child.GetComponent<Image>().sprite = UISprites[447];
        }
        foreach(Transform child in uic.Find("JobPanel").Find("CheckBoxGrid2"))
        {
            child.GetComponent<Image>().sprite = UISprites[447];
        }
        //extras panel checkboxes
        uic.Find("ExtrasPanel").Find("SnowingCheckbox").GetComponent<Image>().sprite = UISprites[447];
        uic.Find("ExtrasPanel").Find("POWCheckbox").GetComponent<Image>().sprite = UISprites[447];
        uic.Find("ExtrasPanel").Find("StunRodCheckbox").GetComponent<Image>().sprite = UISprites[447];
        //submenucontroller checkbox sprites
        GetComponent<SubMenuController>().uncheckedBoxSprite = UISprites[447];
        GetComponent<SubMenuController>().checkedBoxSprite = UISprites[448];
        //loadmap checkbox sprites
        GetComponent<LoadMap>().uncheckedBox = UISprites[447];
        GetComponent<LoadMap>().checkedBox = UISprites[448];
    }
}
