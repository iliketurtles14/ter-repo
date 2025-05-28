using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public bool inAttackMode;
    public MouseCollisionOnItems mcs;
    public bool hasLockedOn;
    public GameObject currentNPC;
    public GameObject player;
    public bool inPunchCycle;
    public Transform aStar;
    public void Start()
    {
        foreach(Transform npc in aStar)
        {
            npc.Find("CombatBox").gameObject.SetActive(false);
        }
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

        if(inAttackMode && hasLockedOn && player.transform.Find("CombatBox").GetComponent<BoxCollider2D>().IsTouching(currentNPC.GetComponent<BoxCollider2D>()) && !inPunchCycle)
        {
            if (!currentNPC.GetComponent<NPCCollectionData>().npcData.isAggro)//if not aggroed already
            {
                AggroNPC(currentNPC);
            }

            inPunchCycle = true;
            StartCoroutine(PunchCycle());
            inPunchCycle = false;
        }
    }
    public void AggroNPC(GameObject npc)
    {
        npc.GetComponent<NPCCollectionData>().npcData.isAggro = true;
    }
    public IEnumerator PunchCycle()
    {
        float punchTime = (player.GetComponent<PlayerCollectionData>().playerData.speed * -.005f) + 1.15f;

        int str = 0; //PLACEHOLDER INT. THIS IS THE PLAYER'S TOTAL STRENGTH (INCLUDING STRENGTH STAT AND WEAPON)
        currentNPC.GetComponent<NPCCollectionData>().npcData.health -= str;
        if(currentNPC.GetComponent<NPCCollectionData>().npcData.health <= 0)
        {
            KillNPC(currentNPC);
        }

        yield return new WaitForSeconds(punchTime);
    }
    public void KillNPC(GameObject npc)
    {
        //kill npc
    }
    public void LockOn(GameObject npc)
    {
        npc.transform.Find("CombatBox").gameObject.SetActive(true);
    }
}
