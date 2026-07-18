using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPCharlie : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private InventorySelection selectionScript;
    private Transform player;
    private Inventory invScript;
    private Transform tiles;
    private Particles particlesScript;
    private Sprite clear;
    private bool hasTalked;
    private List<GameObject> slots = new List<GameObject>();
    private List<Transform> charlies = new List<Transform>();
    private bool ready;
    private List<string> objLayers = new List<string>
    {
        "GroundObjects", "UndergroundObjects", "VentObjects", "RoofObjects"
    };
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        selectionScript = GetComponent<InventorySelection>();
        player = RootObjectCache.GetRoot("Player").transform;
        invScript = GetComponent<Inventory>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        particlesScript = GetComponent<Particles>();
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            slots.Add(slot.gameObject);
        }
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < 4; i++)
        {
            foreach (Transform obj in tiles.Find(objLayers[i]))
            {
                if (obj.name == "CheckpointCharlie")
                {
                    charlies.Add(obj);
                }
            }
        }
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if (mcs.isTouchingCharlie && Input.GetMouseButtonDown(0) && selectionScript.aSlotSelected)
        {
            if (Vector2.Distance(player.position, mcs.touchedCharlie.transform.position + new Vector3(0, 1.6f, 0)) <= 2.4f)
            {
                GiveToCharlie(selectionScript.selectedSlotNum, mcs.touchedCharlie);
            }
        }
        if (!hasTalked)
        {
            foreach (Transform charlie in charlies)
            {
                if (Vector2.Distance(player.position, charlie.position) <= 8)
                {
                    hasTalked = true;
                    CharlieTalk(charlie.gameObject);
                }
            }
        }
    }
    private void GiveToCharlie(int slot, GameObject charlie)
    {
        int id = invScript.inventory[slot].itemData.id;//252 - signed; 256 - unsigned
        if(id == 252)
        {
            hasTalked = true;
            string msg = charlie.GetComponent<NPCSpeech>().GetMessage("Charlie_Signed");
            StartCoroutine(charlie.GetComponent<NPCSpeech>().MakeTextBox(msg, charlie.transform, false));
            for(int i = 0; i < 4; i++)
            {
                foreach(Transform obj in tiles.Find(objLayers[i]))
                {
                    if(obj.name == "CharlieGate")
                    {
                        particlesScript.CreateDust(obj.position, 1, obj.GetComponent<SpriteRenderer>().sortingLayerName);
                        Destroy(obj.gameObject);
                    }
                }
            }
            invScript.inventory[slot].itemData = null;
            slots[slot].GetComponent<Image>().sprite = clear;
            PSoundController.PlaySound("pickup");
        }
        else if(id == 256)
        {
            hasTalked = true;
            string msg = charlie.GetComponent<NPCSpeech>().GetMessage("Charlie_Unsigned");
            StartCoroutine(charlie.GetComponent<NPCSpeech>().MakeTextBox(msg, charlie.transform, false));
        }
    }
    private void CharlieTalk(GameObject charlie)
    {
        OutfitController oc = player.GetComponent<OutfitController>();
        List<string> unsafeOutfits = new List<string>
        {
            "Inmate", "POW", "Elf", "Tux", "Prisoner"
        };
        if(unsafeOutfits.Contains(oc.outfit) || !player.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat = 99;
            string msg = charlie.GetComponent<NPCSpeech>().GetMessage("Guards_Halt");
            StartCoroutine(charlie.GetComponent<NPCSpeech>().MakeTextBox(msg, charlie.transform, true));
            PSoundController.PlaySound("lose");
        }
        else
        {
            StartCoroutine(charlie.GetComponent<NPCSpeech>().MakeTextBox("Papers, please!", charlie.transform, false));
        }
        StartCoroutine(TalkWait());
    }
    private IEnumerator TalkWait()
    {
        yield return new WaitForSeconds(6.5f);
        hasTalked = false;
    }
}
 
