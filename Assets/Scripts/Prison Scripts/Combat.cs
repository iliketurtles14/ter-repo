using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public bool inAttackMode;
    private MouseCollisionOnItems mcs;
    private GameObject combatBox;
    public bool isLockedOn;
    public GameObject targetNPC;
    public bool isPunching;
    private Transform mc;
    private GameObject combatHealth;
    private Death deathScript;
    private MissionAsk missionAskScript;
    private SpecialMessages specialMessagesScript;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        combatBox = RootObjectCache.GetRoot("CombatBox");
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        combatHealth = RootObjectCache.GetRoot("CombatHealth");
        deathScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Death>();
        missionAskScript = mc.Find("MissionPanel").GetComponent<MissionAsk>();
        specialMessagesScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("SpecialMessagePanel").GetComponent<SpecialMessages>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//set inAttackMode
        {
            if (inAttackMode)
            {
                inAttackMode = false;
            }
            else
            {
                inAttackMode = true;
            }
        }

        if (!inAttackMode)
        {
            LockOff();
        }

        if(inAttackMode && mcs.isTouchingNPC && Input.GetMouseButtonDown(0))
        {
            LockOn(mcs.touchedNPC);
            return;
        }

        if(isLockedOn && !isPunching && GetComponent<CapsuleCollider2D>().IsTouching(combatBox.GetComponent<BoxCollider2D>()))
        {
            isPunching = true;
            StartCoroutine(Punch(targetNPC));
            return;
        }

        if (isLockedOn)
        {
            combatBox.transform.position = targetNPC.transform.position;
            combatHealth.transform.position = combatBox.transform.position + new Vector3(0, -1.2f);
            SetHealth(targetNPC);
        }
    }
    public IEnumerator Punch(GameObject npc)
    {
        int netDamage;
        int npcDef;
        int playerStr;
        int realPlayerStr;
        int playerWeaponStr;
        ItemData npcOutfitData;
        ItemData playerWeaponData;
        try
        {
            npcOutfitData = npc.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData;
        }
        catch
        {
            npcOutfitData = null;
        }
        try
        {
            playerWeaponData = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[1].itemData;
        }
        catch
        {
            playerWeaponData = null;
        }
        //take away npc health

        if (npcOutfitData != null)
        {
            npcDef = npcOutfitData.defense;
        }
        else
        {
            npcDef = 0;
        }
        if(playerWeaponData != null)
        {
            playerWeaponStr = playerWeaponData.strength;
        }
        else
        {
            playerWeaponStr = 0;
        }

        playerStr = GetComponent<PlayerCollectionData>().playerData.strength;

        realPlayerStr = Mathf.FloorToInt(playerStr / 10) - 1;

        netDamage = realPlayerStr + playerWeaponStr - npcDef;
        if(netDamage < 0)
        {
            netDamage = 0;
        }

        npc.GetComponent<NPCCollectionData>().npcData.health -= netDamage;
        if(npc.GetComponent<NPCCollectionData>().npcData.health < 0)
        {
            npc.GetComponent<NPCCollectionData>().npcData.health = 0;
        }
        if(npc.GetComponent<NPCCollectionData>().npcData.health == 0)
        {
            KillNPC(npc);
        }

        //aggro npc
        npc.GetComponent<NPCCombat>().isAggro = true;
        npc.GetComponent<NPCCombat>().target = gameObject;

        //punch animation plays

        int lookNum = 0;
        string lookDir = GetComponent<PlayerAnimation>().lookDir;
        switch (lookDir)
        {
            case "right": lookNum = 0; break;
            case "up": lookNum = 1; break;
            case "left": lookNum = 2; break;
            case "down": lookNum = 3; break;
        }

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<PlayerCtrl>().canMove = false;
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();
        GetComponent<PlayerAnimation>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][lookNum];
        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
        {
            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][3][lookNum];
        }
        yield return new WaitForSeconds(.45f);
        GetComponent<PlayerAnimation>().enabled = true;
        GetComponent<PlayerCtrl>().canMove = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        float timeBetweenPunches;
        int speed = GetComponent<PlayerCollectionData>().playerData.speed;

        timeBetweenPunches = ((-11f / 1500f) * speed) + .68f + (11f / 150f);

        yield return new WaitForSeconds(timeBetweenPunches);

        isPunching = false;
    }
    public void KillNPC(GameObject npc)
    {
        deathScript.KillNPC(npc);
        KillFavor(npc);
        LockOff();
    }
    public void LockOn(GameObject npc)
    {
        isLockedOn = true;
        targetNPC = npc;

        combatBox.transform.position = npc.transform.position;
        combatHealth.transform.position = combatBox.transform.position + new Vector3(0, -1.2f);
        combatBox.SetActive(true);
        combatHealth.SetActive(true);
    }
    public void LockOff()
    {
        isLockedOn = false;
        combatBox.SetActive(false);
        combatHealth.SetActive(false);
    }
    public void SetHealth(GameObject npc)
    {
        float maxHealth;
        float currentHealth = npc.GetComponent<NPCCollectionData>().npcData.health;
        int npcStrength = npc.GetComponent<NPCCollectionData>().npcData.strength;
        float healthPercent;
        float barWidth;

        maxHealth = Mathf.FloorToInt(npcStrength / 2);
        healthPercent = currentHealth / maxHealth;

        barWidth = healthPercent * .22f;

        Debug.Log(" barWidth = " + barWidth + " healthPercent = " + healthPercent + " currentHealth = " + currentHealth + " maxHealth = " + maxHealth);

        combatHealth.transform.Find("Bar").GetComponent<SpriteRenderer>().size = new Vector2(barWidth, .02f);
        combatHealth.transform.Find("Bar").transform.localPosition = new Vector3(-.11f + (barWidth / 2), 0);
    }
    private void KillFavor(GameObject npc)
    {
        List<Mission> killMissions = new List<Mission>();
        foreach(Mission mission in missionAskScript.savedMissions)
        {
            if(mission.type == "guardBeat" || mission.type == "inmateBeat")
            {
                killMissions.Add(mission);
            }
        }

        if(killMissions.Count == 0)
        {
            return;
        }

        foreach(Mission mission in killMissions)
        {
            if(mission.target == npc.GetComponent<NPCCollectionData>().npcData.displayName)
            {
                StartCoroutine(specialMessagesScript.MakeMessage("You completed a Favor!\n+$" + mission.pay, "favor"));
                GetComponent<PlayerCollectionData>().playerData.money += mission.pay;
                missionAskScript.savedMissions.Remove(mission);
            }
        }
    }
}
