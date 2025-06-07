using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCAggro : MonoBehaviour
{
    public void ActivateAggro(GameObject npc, GameObject target)
    {
        target = npc.GetComponent<NPCCollectionData>().npcData.aggroTarget;

        npc.GetComponent<AILerp>().enabled = false;
        npc.GetComponent<AStar>().enabled = false;
        npc.GetComponent<Seeker>().enabled = false;
        NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        StartCoroutine(AggroLoop(npc, target.transform));
    }
    public IEnumerator AggroLoop(GameObject npc, Transform target)
    {
        NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();

        while (npc.GetComponent<NPCCollectionData>().npcData.isAggro)
        {
            agent.SetDestination(target.transform.position);
            
            yield return new WaitForEndOfFrame();
        }
    }
    public void DeactivateAggro(GameObject npc)
    {
        npc.GetComponent<NavMeshAgent>().enabled = false;
        npc.GetComponent<AILerp>().enabled = true;
        npc.GetComponent<AStar>().enabled = true;
        npc.GetComponent<Seeker>().enabled = true;
    }
}
