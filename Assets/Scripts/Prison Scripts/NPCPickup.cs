using UnityEngine;

public class NPCPickup : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private HPAChecker hpa;
    private Transform player;
    public bool hasPickedUp;
    private GameObject pickedUpNPC = null;
    private Vector3 offsetVector;
    private BodyController bc;
    private OutfitController oc;
    private PlayerCollectionData playerColData;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        hpa = RootObjectCache.GetRoot("Player").GetComponent<HPAChecker>();
        player = RootObjectCache.GetRoot("Player").transform;
        offsetVector = new Vector2(0, .8f);
        bc = player.GetComponent<BodyController>();
        oc = player.GetComponent<OutfitController>();
        playerColData = player.GetComponent<PlayerCollectionData>();
    }
    private void Update()
    {
        hpa.hasPickedUp = hasPickedUp;
        
        if (mcs.isTouchingNPC && Input.GetMouseButtonDown(1) && !hpa.isBusy && !hasPickedUp)
        {
            if (mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead)
            {
                float distance = Vector2.Distance(player.position, mcs.touchedNPC.transform.position);
                if(distance <= 2.4f)
                {
                    hasPickedUp = true;
                    pickedUpNPC = mcs.touchedNPC;
                }
            }
        }
        if (hasPickedUp)
        {
            pickedUpNPC.transform.position = player.position + offsetVector;
            pickedUpNPC.GetComponent<SpriteRenderer>().sortingOrder = 8;
            pickedUpNPC.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 9;
            bc.deskIsPickedUp = true;
            oc.deskIsPickedUp = true;

            if (mcs.isTouchingFloor && Input.GetMouseButtonDown(1))
            {
                float distance = Vector2.Distance(player.position, mcs.touchedFloor.transform.position);
                if(distance <= 2.4f)
                {
                    pickedUpNPC.transform.position = mcs.touchedFloor.transform.position;
                    pickedUpNPC.GetComponent<SpriteRenderer>().sortingOrder = 4;
                    pickedUpNPC.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 5;
                    bc.deskIsPickedUp = false;
                    oc.deskIsPickedUp = false;
                    hasPickedUp = false;
                    pickedUpNPC = null;
                }
            }
        }
        if((hasPickedUp && !pickedUpNPC.GetComponent<NPCCollectionData>().npcData.isDead) ||
            (playerColData.playerData.isDead && hasPickedUp))
        {
            pickedUpNPC.transform.position = player.position;
            pickedUpNPC.GetComponent<SpriteRenderer>().sortingOrder = 4;
            pickedUpNPC.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 5;
            bc.deskIsPickedUp = false;
            oc.deskIsPickedUp = false;
            hasPickedUp = false;
            pickedUpNPC = null;
        }
    }
}
