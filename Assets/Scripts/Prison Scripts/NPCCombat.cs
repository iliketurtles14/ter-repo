using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPCCombat : MonoBehaviour
{
    public bool isAggro;
    public GameObject target;
    public bool isPunching;
    private Transform mc;
    private Death deathScript;
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        deathScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Death>();
    }

    private void Update()
    {
        if (isAggro)
        {
            SetTarget(target);
        }
        else
        {
            DeAggro();
        }

        if (!isPunching && isAggro && GetComponent<CapsuleCollider2D>().IsTouching(target.GetComponent<CapsuleCollider2D>()))
        {
            isPunching = true;
            StartCoroutine(Punch(target));
        }

        if (isAggro)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if(distance > 11.2f) //7 tiles
            {
                DeAggro();
            }
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        DeAggro();
    }
    private void OnEnable()
    {
        DeAggro();//holy shit please deaggro lmao i have to keep putting this in everywhere
    }
    public void SetTarget(GameObject aTarget)
    {
        GetComponent<NPCAI>().enabled = false;
        GetComponent<AILerp>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<NavMeshAgent>().destination = aTarget.transform.position;
    }
    public IEnumerator Punch(GameObject aTarget)
    {
        int netDamage;
        int targetDef;
        int npcStr;
        int realNPCStr;
        int npcWeaponStr;
        ItemData npcWeaponData;
        ItemData targetOutfitData;

        try
        {
            if(aTarget.name != "Player")
            {
                targetOutfitData = aTarget.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData;
            }
            else
            {
                targetOutfitData = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData;
            }
        }
        catch
        {
            targetOutfitData = null;
        }

        try
        {
            npcWeaponData = GetComponent<NPCCollectionData>().npcData.inventory[6].itemData;
        }
        catch
        {
            npcWeaponData = null;
        }

        //take away target health
        if(targetOutfitData != null)
        {
            targetDef = targetOutfitData.defense;
        }
        else
        {
            targetDef = 0;
        }

        if(npcWeaponData != null)
        {
            npcWeaponStr = npcWeaponData.strength;
        }
        else
        {
            npcWeaponStr = 0;
        }

        npcStr = GetComponent<NPCCollectionData>().npcData.strength;

        realNPCStr = Mathf.FloorToInt(npcStr / 20) - 1;

        netDamage = realNPCStr + npcWeaponStr - targetDef;
        if(netDamage < 0)
        {
            netDamage = 0;
        }

        if(aTarget.name != "Player")
        {
            aTarget.GetComponent<NPCCollectionData>().npcData.health -= netDamage;
            if(aTarget.GetComponent<NPCCollectionData>().npcData.health < 0)
            {
                aTarget.GetComponent<NPCCollectionData>().npcData.health = 0;
            }
            if(aTarget.GetComponent<NPCCollectionData>().npcData.health == 0)
            {
                deathScript.KillNPC(aTarget);
            }
        }
        else
        {
            aTarget.GetComponent<PlayerCollectionData>().playerData.health -= netDamage;
            if(aTarget.GetComponent<PlayerCollectionData>().playerData.health < 0)
            {
                aTarget.GetComponent<PlayerCollectionData>().playerData.health = 0;
            }
            if(aTarget.GetComponent<PlayerCollectionData>().playerData.health == 0)
            {
                deathScript.KillPlayer();
                DeAggro();
            }
        }

        //aggro target if not player
        if(aTarget.name != "Player")
        {
            aTarget.GetComponent<NPCCombat>().isAggro = true;
            aTarget.GetComponent<NPCCombat>().target = gameObject;
        }

        //punch anim plays

        int lookNum = 0;
        string lookDir = GetComponent<NPCAnimation>().lookDir;
        switch (lookDir)
        {
            case "right": lookNum = 0; break;
            case "up": lookNum = 1; break;
            case "left": lookNum = 2; break;
            case "down": lookNum = 3; break;
        }

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<NavMeshAgent>().speed = 0;
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();
        GetComponent<NPCAnimation>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][lookNum];
        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
        {
            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][3][lookNum];
        }
        yield return new WaitForSeconds(.45f);
        GetComponent<NPCAnimation>().enabled = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<NavMeshAgent>().speed = 8;

        float timeBetweenPunches;
        int speed = GetComponent<NPCCollectionData>().npcData.speed;

        timeBetweenPunches = ((-11f / 1500f) * speed) + .68f + (11f / 150f);

        yield return new WaitForSeconds(timeBetweenPunches);

        isPunching = false;
    }
    public void DeAggro()
    {
        isAggro = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<AILerp>().enabled = true;
        GetComponent<NPCAI>().enabled = true;
    }
}
