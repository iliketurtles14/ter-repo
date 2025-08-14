using System.Collections;
using UnityEngine;
using System;

public class Combat : MonoBehaviour
{
    public bool inAttackMode;
    private MouseCollisionOnItems mcs;
    public bool hasLockedOn;
    public GameObject currentNPC;
    private GameObject player;
    public bool inPunchCycle;
    private GameObject combatBox;
    private Transform idPanel;
    private NPCAggro npcAggroScript;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        combatBox = RootObjectCache.GetRoot("CombatBox");
        idPanel = RootObjectCache.GetRoot("MenuCanvas").transform.Find("PlayerMenuPanel");
        npcAggroScript = GetComponent<NPCAggro>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAttackMode)
        {
            inAttackMode = true;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && inAttackMode)
        {
            inAttackMode = false;
        }

        if (mcs.isTouchingNPC && inAttackMode && Input.GetMouseButtonDown(0))
        {
            hasLockedOn = true;
            currentNPC = mcs.touchedNPC;
            LockOn(mcs.touchedNPC);
        }

        if (hasLockedOn)
        {
            combatBox.transform.position = currentNPC.transform.position;
        }

        if(inAttackMode && hasLockedOn && player.GetComponent<CapsuleCollider2D>().IsTouching(combatBox.GetComponent<BoxCollider2D>()) && !inPunchCycle)
        {
            if (!currentNPC.GetComponent<NPCCollectionData>().npcData.isAggro)//if not aggroed already
            {
                AggroNPC(currentNPC);
            }

            StartCoroutine(PunchCycle());
        }
    }
    public void AggroNPC(GameObject npc)
    {
        npc.GetComponent<NPCCollectionData>().npcData.isAggro = true;
        npc.GetComponent<NPCCollectionData>().npcData.aggroTarget = player;
        npcAggroScript.ActivateAggro(npc, player);
    }
    public void DeaggroNPC(GameObject npc)
    {
        npc.GetComponent<NPCCollectionData>().npcData.isAggro = false;
        npc.GetComponent<NPCCollectionData>().npcData.aggroTarget = null;
        npcAggroScript.DeactivateAggro(npc);
    }
    public IEnumerator PunchCycle()
    {
        float punchTime = (player.GetComponent<PlayerCollectionData>().playerData.speed * -.005f) + 1.15f;

        //ADD THE NPC DEEFENSE WHEN THATS DONE
        int str;
        if (idPanel.GetComponent<PlayerIDInv>().idInv[1].itemData != null)
        {
            str = (int)Math.Ceiling(player.GetComponent<PlayerCollectionData>().playerData.strength / 20.0) + idPanel.GetComponent<PlayerIDInv>().idInv[1].itemData.strength;
        }
        else
        {
            str = (int)Math.Ceiling(player.GetComponent<PlayerCollectionData>().playerData.strength / 20.0);
        }

        currentNPC.GetComponent<NPCCollectionData>().npcData.health -= str;
        //if(currentNPC.GetComponent<NPCCollectionData>().npcData.health <= 0)
        //{
        //    KillNPC(currentNPC);
        //}

        int lookNum;
        switch (player.GetComponent<PlayerAnimation>().lookDir)
        {
            case "right":
                lookNum = 0;
                break;
            case "up":
                lookNum = 1;
                break;
            case "left":
                lookNum = 2;
                break;
            case "down":
                lookNum = 3;
                break;
            default:
                lookNum = 0;
                break;
        }
        inPunchCycle = true;
        StartCoroutine(PunchAnim(lookNum));
        yield return new WaitForSeconds(punchTime);
        inPunchCycle = false;
    }
    public IEnumerator PunchAnim(int lookNum)
    {
        BodyController bc = player.GetComponent<BodyController>();
        player.GetComponent<PlayerCtrl>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerAnimation>().enabled = false;
        player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][lookNum];
        yield return new WaitForSeconds(.45f);
        player.GetComponent<PlayerCtrl>().enabled = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PlayerAnimation>().enabled = true;
    }
    public void KillNPC(GameObject npc)
    {
        Debug.Log("hes dead lol");
    }
    public void LockOn(GameObject npc)
    {
        combatBox.SetActive(true);
        combatBox.transform.position = npc.transform.position;
    }
}
