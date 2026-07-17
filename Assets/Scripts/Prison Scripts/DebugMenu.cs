using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    private TMP_InputField input;
    private TextMeshProUGUI output;
    private ItemDataCreator creator;
    private Inventory invScript;
    private Routine routineScript;
    private Transform ic;
    private bool isFocused;
    private IniFile options;
    private bool isOpen;
    private PauseController pc;
    private GeneratorController genCtrl;
    private Transform player;
    private PlayerCollectionData playerColData;
    private Transform tiles;
    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private int uiLayer;
    private int ventCoverLayer;
    private HoleClimb holeClimbScript;
    private Transform globalLight;
    private Transform undergroundLight;
    private void Start()
    {
        input = transform.Find("Input").GetComponent<TMP_InputField>();
        output = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Transform so = RootObjectCache.GetRoot("ScriptObject").transform;
        creator = so.GetComponent<ItemDataCreator>();
        invScript = so.GetComponent<Inventory>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        routineScript = ic.Find("Time").GetComponent<Routine>();
        input.onSelect.AddListener(OnInputFocused);
        input.onDeselect.AddListener(OnInputUnfocused);
        options = new IniFile(Path.Combine(Application.streamingAssetsPath, "UserData.ini"));
        pc = so.GetComponent<PauseController>();
        genCtrl = so.GetComponent<GeneratorController>();
        player = RootObjectCache.GetRoot("Player").transform;
        playerColData = player.GetComponent<PlayerCollectionData>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
        uiLayer = LayerMask.NameToLayer("UI");
        ventCoverLayer = LayerMask.NameToLayer("VentCovers");
        holeClimbScript = so.GetComponent<HoleClimb>();
        globalLight = RootObjectCache.GetRoot("GlobalLight").transform;
        undergroundLight = RootObjectCache.GetRoot("UndergroundLight").transform;
        if (options.Read("DebugMode", "Settings") == "False")
        {
            gameObject.SetActive(false);
        }
        else
        {
            CloseMenu();
        }
    }
    private void Update()
    {
        if(isFocused && !string.IsNullOrEmpty(input.text) && Input.GetKeyDown(KeyCode.Return))
        {
            EnterCommand(input.text);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        //debug command stuff
        if (playerColData.playerData.inGodMode)
        {
            playerColData.playerData.energy = 0;
        }
    }
    private void OpenMenu()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        ic.Find("DebugText").gameObject.SetActive(false);
        pc.Pause(true);
        isOpen = true;
    }
    private void CloseMenu()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        ic.Find("DebugText").gameObject.SetActive(true);
        pc.Unpause();
        isOpen = false;
    }
    private void OnInputFocused(string val)
    {
        isFocused = true;
    }
    private void OnInputUnfocused(string val)
    {
        isFocused = false;
    }
    private void EnterCommand(string command)
    {
        input.text = "";

        if (!command.StartsWith("/"))
        {
            output.text += "\nInvalid command. Type \"/help\" for help with using the Debug Menu.";
            return;
        }

        string[] args = command.Split(' ');

        if (command == "/help")
        {
            output.text += "\nHere is a list of valid commands:\n\t" +
                "/give [int itemID]\n\t" +
                "/timeFast [true/false]\n\t" +
                "/generatorFlip\n\t" +
                "/oob [true/false]\n\t" +
                "/setSpeed [int speed (default:10)]\n\t" +
                "/godMode [true/false]\n\t" +
                "/cameraView [float viewSize]\n\t" +
                "/timeFreeze [true/false]\n\t" +
                "/powerOff\n\t" +
                "/layer [int layer (0,1,2,3)]";
            return;
        }

        if (args[0] == "/give")
        {
            try
            {
                int id = Convert.ToInt32(args[1]);
                creator.CreateItemData(id);
            }
            catch
            {
                output.text += "\nInvalid item ID.";
                return;
            }
            bool isFull = true;
            for (int i = 0; i < 6; i++)
            {
                if (invScript.inventory[i].itemData == null)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                output.text += "\nThe inventory is full.";
                return;
            }
            ItemData item = creator.CreateItemData(Convert.ToInt32(args[1]));
            invScript.Add(item);
            output.text += "\nAdding " + item.displayName + " to the inventory.";
            return;
        }
        if (args[0] == "/timeFast")
        {
            if (args[1] == "true")
            {
                routineScript.isSpeedingUp = true;
                output.text += "\nSetting time to fast speed.";
                return;
            }
            if (args[1] == "false")
            {
                routineScript.isSpeedingUp = false;
                output.text += "\nSetting time to normal speed.";
                return;
            }
        }
        if (command == "/generatorFlip")
        {
            genCtrl.FlipGenerator();
            output.text += "\nFlipping the generator.";
            return;
        }
        if (args[0] == "/oob")
        {
            if (args[1] == "true")
            {
                player.GetComponent<CapsuleCollider2D>().isTrigger = true;
                output.text += "\nSetting player collider isTrigger to true.";
                return;
            }
            else if (args[1] == "false")
            {
                player.GetComponent<CapsuleCollider2D>().isTrigger = false;
                output.text += "\nSetting player collider isTrigger to false.";
                return;
            }
        }
        if (args[0] == "/setSpeed")
        {
            try
            {
                player.GetComponent<PlayerCtrl>().movSpeed = Convert.ToInt32(args[1]);
                output.text += "\nSetting player speed to " + Convert.ToInt32(args[1]) + ".";
                return;
            }
            catch
            {
                output.text += "\nInvalid speed.";
                return;
            }
        }
        if (args[0] == "/godMode")
        {
            if (args[1] == "true")
            {
                player.GetComponent<PlayerCollectionData>().playerData.inGodMode = true;
                output.text += "\nSetting playerData.inGodMode to true.";
                return;
            }
            else if (args[1] == "false")
            {
                player.GetComponent<PlayerCollectionData>().playerData.inGodMode = false;
                output.text += "\nSetting playerData.inGodMode to false.";
                return;
            }
        }
        //if (args[0] == "/giveCustom")
        //{
        //    string rawArgs = command.Replace("/giveCustom ", "");
        //    string[] itemArgs = rawArgs.Split(" ");
        //    foreach(string arg in itemArgs)
        //    {

        //    }
        //}
        if (args[0] == "/cameraView")
        {
            try
            {
                float view = Convert.ToSingle(args[1]);
                Camera.main.orthographicSize = view;
                output.text += "\nSetting Camera.main.orthographicSize to " + view.ToString() + ".";
                return;
            }
            catch
            {
                output.text += "\nInvalid view size.";
                return;
            }
        }
        if (args[0] == "/timeFreeze")
        {
            if (args[1] == "true")
            {
                routineScript.isFrozen = true;
                output.text += "\nSetting Routine.isFrozen to true.";
                return;
            }
            else if (args[1] == "false")
            {
                routineScript.isFrozen = false;
                output.text += "\nSetting Routine.isFrozen to false.";
                return;
            }
        }
        if (args[0] == "/powerOff")
        {
            genCtrl.genIsOff = true;
            output.text += "\nSetting GeneratorController.genIsOff to true.";
            return;
        }
        if (args[0] == "/layer")
        {
            if (args[1] != "0" && args[1] != "1" && args[1] != "2" && args[1] != "3")
            {
                output.text += "\nInvalid layer.";
                return;
            }
            ChangeLayer(args[1]);
            string layerName = "Ground";
            switch (args[1])
            {
                case "0":
                    layerName = "Underground";
                    break;
                case "2":
                    layerName = "Vents";
                    break;
                case "3":
                    layerName = "Roof";
                    break;
            }
            output.text += "\nMoving to layer " + layerName + ".";
            return;
        }
        //keep at end
        output.text += "\nInvalid command. Type \"/help\" for help with using the Debug Menu.";
    }
    private void ChangeLayer(string layer) //taken straight from LadderClimb.cs
    {
        switch (layer)
        {
            case "0":
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, false);
                player.GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                tiles.Find("UndergroundObjects").gameObject.SetActive(true);
                undergroundLight.gameObject.SetActive(true);
                globalLight.gameObject.SetActive(false);
                holeClimbScript.isUnderground = true;
                break;
            case "1":
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                tiles.Find("RoofTiles").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("VentTiles").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(false);
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundObjects").gameObject.SetActive(false);
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
                undergroundLight.gameObject.SetActive(false);
                globalLight.gameObject.SetActive(true);
                holeClimbScript.isUnderground = false;
                break;
            case "2":
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                tiles.Find("RoofTiles").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("VentTiles").gameObject.SetActive(true);
                tiles.Find("VentObjects").gameObject.SetActive(true);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(false);
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundObjects").gameObject.SetActive(false);
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, false);
                Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, false);
                VentEnable();
                undergroundLight.gameObject.SetActive(false);
                globalLight.gameObject.SetActive(true);
                holeClimbScript.isUnderground = false;
                break;
            case "3":
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                tiles.Find("RoofTiles").gameObject.SetActive(true);
                tiles.Find("RoofObjects").gameObject.SetActive(true);
                tiles.Find("VentTiles").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(true);
                tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                tiles.Find("UndergroundObjects").gameObject.SetActive(false);
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, false);
                undergroundLight.gameObject.SetActive(false);
                globalLight.gameObject.SetActive(true);
                holeClimbScript.isUnderground = false;
                break;
        }
    }
    private void DisableAllLayerCollisions()
    {
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, true);
    }
    public void VentEnable()
    {
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true;
        Color color = tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color;
        color.a = 235f / 256f;
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

        SpriteRenderer ventTilesSpriteRenderer = tiles.Find("VentTiles").GetComponent<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = tiles.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
        ventTilesSpriteRenderer.color = new Color(ventTilesSpriteRenderer.color.r, ventTilesSpriteRenderer.color.g, ventTilesSpriteRenderer.color.b, 1);
        foreach (SpriteRenderer sr in ventObjectSpriteRenderers)
        {
            Color aColor = sr.color;
            aColor.a = 1;
            sr.color = aColor;
        }
    }
}
