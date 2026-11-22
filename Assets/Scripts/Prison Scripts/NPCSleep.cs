using Pathfinding;
using System.Collections;
using UnityEngine;

public class NPCSleep : MonoBehaviour
{
    public void Sleep(GameObject npc, GameObject bed)
    {
        npc.GetComponent<AILerp>().enabled = false;
        StartCoroutine(WaitForIttttt(npc, bed));
    }
    private IEnumerator WaitForIttttt(GameObject npc, GameObject bed)
    {
        yield return new WaitForEndOfFrame();
        Vector3 bedOffset;
        if (npc.GetComponent<NPCCollectionData>().npcData.charNum != 1)
        {
            bedOffset = new Vector3(0, .4f);
        }
        else
        {
            bedOffset = new Vector3(0, .35f);
        }
        if (bed.name.Contains("Horizontal"))
        {
            bedOffset = new Vector3(-.3f, .1f);
        }

        if (bed.name.Contains("Horizontal"))
        {
            npc.transform.position = bed.transform.position + bedOffset;
            npc.GetComponent<NPCCollectionData>().npcData.isSleeping = true;
            npc.GetComponent<NPCAnimation>().enabled = false;
            BodyController bc = npc.GetComponent<BodyController>();
            npc.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][2];
            OutfitController oc = npc.GetComponent<OutfitController>();
            if (npc.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                npc.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][2];
            }
        }
        else
        {
            npc.transform.position = bed.transform.position + bedOffset;
            npc.GetComponent<NPCCollectionData>().npcData.isSleeping = true;
            npc.GetComponent<NPCAnimation>().enabled = false;
            BodyController bc = npc.GetComponent<BodyController>();
            npc.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
            OutfitController oc = npc.GetComponent<OutfitController>();
            if (npc.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                npc.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
                if (oc.outfit == "Inmate")
                {
                    if (npc.GetComponent<NPCCollectionData>().npcData.charNum != 1)
                    {
                        npc.transform.Find("Outfit").localPosition = new Vector3(0, -.25f, 0);
                    }
                    else
                    {
                        npc.transform.Find("Outfit").localPosition = new Vector3(0, -.2f, 0);
                    }
                }
            }
        }
        npc.GetComponent<AILerp>().enabled = true;
    }
    public void Wake(GameObject npc)
    {
        npc.GetComponent<NPCCollectionData>().npcData.isSleeping = false;
        npc.GetComponent<NPCAnimation>().enabled = true;
        npc.GetComponent<NPCAI>().DoSearch();
    }
}
