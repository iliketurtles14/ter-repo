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
            //punch animation and health remove from npc
            AggroNPC(currentNPC);

            inPunchCycle = true;
            StartCoroutine(PunchCycle());
            inPunchCycle = false;
        }
    }
    public void AggroNPC(GameObject npc)
    {
        //change pathfinding method
    }
    public IEnumerator PunchCycle()
    {
        float punchTime = (player.GetComponent<PlayerCollectionData>().playerData.speed * -.005f) + 1.15f;

        yield return new WaitForSeconds(punchTime);
    }
    public IEnumerator TargetAnim(GameObject npc)
    {

    }
    public void LockOn(GameObject npc)
    {
        //do stuff for whenever you lock on to an npc
    }
}
