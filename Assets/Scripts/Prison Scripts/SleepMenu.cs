using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SleepMenu : MonoBehaviour
{
    private Schedule scheduleScript;
    private Routine routineScript;
    private bool canSleep;
    private MouseCollisionOnItems mcs;
    private Transform player;
    private bool menuIsOpen;
    private Saving saveScript;
    private Transform aStar;
    private NPCSleep sleepScript;
    private Transform tiles;
    private PauseController pc;
    private Transform mc;
    private InventorySelection selectionScript;
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        saveScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Saving>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        sleepScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<NPCSleep>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        selectionScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<InventorySelection>();

        DataSender ds = DataSender.instance;
        if(ds.currentSave == -1)
        {
            transform.Find("SaveText").GetComponent<TextMeshProUGUI>().text = "Would you like to sleep until morning?";

        }

        Close();
    }
    private void Update()
    {
        if(scheduleScript.periodCode == "LO" && routineScript.min != routineScript.startingMin)
        {
            canSleep = true;
        }
        else
        {
            canSleep = false;
        }

        if (!canSleep)
        {
            return;
        }

        if(!selectionScript.aSlotSelected && Input.GetMouseButtonDown(0) && mcs.isTouchingSittable && mcs.touchedSittable.name.Contains("PlayerBed"))
        {
            float distance = Vector2.Distance(player.position, mcs.touchedSittable.transform.position);
            if(distance > 2.4f)
            {
                return;
            }
            Open();
            PSoundController.PlaySound("open");
        }
    }
    public void Open()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(false);
        menuIsOpen = true;
    }
    public void Close()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        menuIsOpen = false;
    }
    public IEnumerator Sleep()
    {
        Close();

        //fade screen
        //save
        //set time
        //send inmates to beds
        //keep it niche
        //put sheets and pillows back on beds tsssss
        //advance day (i think do that at a time script or something)
        //unfade

        Transform blocker = RootObjectCache.GetRoot("DeathBlockerCanvas").transform;
        blocker.gameObject.SetActive(true);
        blocker.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1);

        routineScript.AdvanceDay();

        foreach (Transform npc in aStar)
        {
            if (npc.name.StartsWith("Inmate") && !npc.GetComponent<NPCAI>().hasSetPath)
            {
                if (npc.GetComponent<NPCCollectionData>().npcData.bed != null)
                {
                    sleepScript.Sleep(npc.gameObject, npc.GetComponent<NPCCollectionData>().npcData.bed);
                }
            }
        }

        //niche

        DataSender ds = DataSender.instance;
        foreach (Transform obj in tiles.Find("GroundObjects"))
        {
            switch (obj.name)
            {
                case "PlayerBedHorizontal":
                case "BedHorizontal":
                    obj.GetComponent<SpriteRenderer>().sprite = ds.PrisonObjectImages[265];
                    break;
                case "PlayerBedVertical":
                case "BedVertical":
                    obj.GetComponent<SpriteRenderer>().sprite = ds.PrisonObjectImages[264];
                    break;
            }
        }

        routineScript.min = routineScript.startingMin;
        routineScript.sec = 50;

        yield return new WaitForSeconds(1);
        if (ds.currentSave != -1)
        {
            saveScript.Save();
        }
        yield return new WaitForSeconds(1);
        blocker.gameObject.SetActive(false);
    }
}
