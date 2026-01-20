using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    private GameObject player;
    private Transform deathCanvas;
    private Transform tiles;
    private Sittables sittablesScript;
    private VitalController vitalScript;
    private Transform mc;
    private Inventory inventoryScript;
    private Transform ic;
    private Sprite clearSprite;
    private SetInitialOutfits setInitialOutfitsScript;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player");
        deathCanvas = RootObjectCache.GetRoot("DeathBlockerCanvas").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        sittablesScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Sittables>();
        vitalScript = player.GetComponent<VitalController>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        setInitialOutfitsScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<SetInitialOutfits>();

        deathCanvas.gameObject.SetActive(false);
    }
    public void KillPlayer()
    {
        player.GetComponent<HPAChecker>().isDead = true;
        player.GetComponent<PlayerCollectionData>().playerData.isDead = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerAnimation>().enabled = false;
        BodyController bc = player.GetComponent<BodyController>();
        OutfitController oc = player.GetComponent<OutfitController>();
        player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
        if (player.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
        {
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
            int outfitItemID = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id;
            if (outfitItemID == 29 || outfitItemID == 30 || outfitItemID == 31 || outfitItemID == 32) //check if its an inmate outfit (this is because the inmate sleeping outfit sprite is not 16x16 like every other sprite for some reason)
            {
                if (NPCSave.instance.playerCharacter != 1)
                {
                    player.transform.Find("Outfit").localPosition = new Vector3(0, -.025f, 0);
                }
                else
                {
                    player.transform.Find("Outfit").localPosition = new Vector3(0, -.02f, 0);
                }
            }
        }
        StartCoroutine(PlayerFadeOut());
    }
    private IEnumerator PlayerFadeOut()
    {
        deathCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        //send player to medic
        GameObject medicBed = null;
        List<GameObject> medicBeds = new List<GameObject>();
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "MedicBed")
            {
                medicBeds.Add(obj.gameObject);
            }
        }
        int rand = UnityEngine.Random.Range(0, medicBeds.Count);
        medicBed = medicBeds[rand];
        sittablesScript.sittable = medicBed;

        Vector3 bedOffset;
        if(NPCSave.instance.playerCharacter != 1)
        {
            bedOffset = new Vector3(0, .4f);
        }
        else
        {
            bedOffset = new Vector3(0, .35f);
        }

        medicBed.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForFixedUpdate();
        player.transform.position = medicBed.transform.position + bedOffset;

        vitalScript.energyRate = 1;
        vitalScript.energyRateAmount = 2;
        vitalScript.healthRate = 1;
        vitalScript.healthRateAmount = 1;

        player.GetComponent<PlayerCollectionData>().playerData.energy -= 50;
        if(player.GetComponent<PlayerCollectionData>().playerData.energy < 0)
        {
            player.GetComponent<PlayerCollectionData>().playerData.energy = 0;
        }

        //lose bad items and reset outfit and weapon
        int i = 0;
        List<int> badItems = new List<int>();
        foreach(InventoryItem item in inventoryScript.inventory)
        {
            if(item.itemData != null)
            {
                if (item.itemData.isContraband)
                {
                    item.itemData = null;
                    badItems.Add(i);
                }
            }
            i++;
        }
        List<Transform> slots = new List<Transform>();
        foreach(Transform slot in ic.Find("GUIPanel"))
        {
            slots.Add(slot);
        }
        foreach(int badNum in badItems)
        {
            slots[badNum].GetComponent<Image>().sprite = clearSprite;
        }

        PlayerIDInv playerIDInvScript = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
        
        playerIDInvScript.idInv[0].itemData = setInitialOutfitsScript.outfitData;
        mc.Find("PlayerMenuPanel").Find("Outfit").GetComponent<Image>().sprite = setInitialOutfitsScript.outfitData.sprite;

        if (playerIDInvScript.idInv[1].itemData != null)
        {
            playerIDInvScript.idInv[1].itemData = null;
            mc.Find("PlayerMenuPanel").Find("Weapon").GetComponent<Image>().sprite = clearSprite;
        }

        yield return new WaitForSeconds(2);//waits for the animation to stop and for the game to allow the player to get off the bed
        sittablesScript.onSittable = true;
        deathCanvas.gameObject.SetActive(false);
    }
    public void KillNPC(GameObject npc)
    {
        Debug.Log("Killing " + npc.name);
        npc.GetComponent<NPCCombat>().isAggro = false;
        npc.GetComponent<NPCCombat>().enabled = false;
        npc.GetComponent<NavMeshAgent>().enabled = false;
        npc.GetComponent<AILerp>().enabled = false;
        npc.GetComponent<NPCAI>().enabled = false;
        npc.GetComponent<NPCCollectionData>().npcData.isDead = true;
        npc.GetComponent<NPCAnimation>().enabled = false;
        BodyController bc = npc.GetComponent<BodyController>();
        OutfitController oc = npc.GetComponent<OutfitController>();
        npc.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
        if (npc.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
        {
            npc.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
            int outfitItemID = npc.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData.id;
            if (outfitItemID == 29 || outfitItemID == 30 || outfitItemID == 31 || outfitItemID == 32) //check if its an inmate outfit (this is because the inmate sleeping outfit sprite is not 16x16 like every other sprite for some reason)
            {
                if (npc.GetComponent<NPCCollectionData>().npcData.charNum != 1)
                {
                    npc.transform.Find("Outfit").localPosition = new Vector3(0, -.025f, 0);
                }
                else
                {
                    npc.transform.Find("Outfit").localPosition = new Vector3(0, -.02f, 0);
                }
            }
        }
        StartCoroutine(NPCWake(npc));
    }
    private IEnumerator NPCWake(GameObject npc)
    {
        yield return new WaitForSeconds(11);
        npc.GetComponent<NPCCombat>().enabled = true;
        npc.GetComponent<AILerp>().enabled = true;
        npc.GetComponent<NPCAI>().enabled = true;
        npc.GetComponent<NPCCollectionData>().npcData.isDead = false;
        npc.GetComponent<NPCAnimation>().enabled = true;
    }
}
