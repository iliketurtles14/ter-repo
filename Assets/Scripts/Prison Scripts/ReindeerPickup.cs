using UnityEngine;

public class ReindeerPickup : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    private HPAChecker hpa;
    private GameObject pickedUpDeer;
    private bool hasPickedUp;
    private DeskStand deskStandScript;
    private DeskPickUp deskPickUpScript;
    private Vector3 offsetVector;
    private BodyController bc;
    private OutfitController oc;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        hpa = player.GetComponent<HPAChecker>();
        deskStandScript = GetComponent<DeskStand>();
        deskPickUpScript = GetComponent<DeskPickUp>();
        offsetVector = new Vector2(0, .8f);
        bc = player.GetComponent<BodyController>();
        oc = player.GetComponent<OutfitController>();
    }
    private void Update()
    {
        hpa.hasPickedUp = hasPickedUp;

        if(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground")))
        {
            return;
        }

        if(mcs.isTouchingReindeer && Input.GetMouseButtonDown(1) && !hpa.isBusy && !hasPickedUp && !deskPickUpScript.isPickedUp && !deskStandScript.hasClimbed && !deskStandScript.isClimbing)
        {
            float dist = Vector2.Distance(player.position, mcs.touchedReindeer.transform.position);
            if(dist <= 2.4f)
            {
                PSoundController.PlaySound("pickup");
                hasPickedUp = true;
                deskStandScript.isPickedUp = true;
                pickedUpDeer = mcs.touchedReindeer;
                pickedUpDeer.GetComponent<ReindeerAI>().shouldMove = false;
            }
        }
        if (hasPickedUp)
        {
            pickedUpDeer.transform.position = player.position + offsetVector;
            pickedUpDeer.GetComponent<SpriteRenderer>().sortingOrder = 7;
            bc.deskIsPickedUp = true;
            oc.deskIsPickedUp = true;
            pickedUpDeer.GetComponent<ReindeerAnimation>().lookDir = player.GetComponent<PlayerAnimation>().lookDir;
            if(mcs.isTouchingFloor && Input.GetMouseButtonDown(1))
            {
                float dist = Vector2.Distance(player.position, mcs.touchedFloor.transform.position);
                if(dist <= 2.4f)
                {
                    PSoundController.PlaySound("throw");
                    pickedUpDeer.transform.position = mcs.touchedFloor.transform.position;
                    pickedUpDeer.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    bc.deskIsPickedUp = false;
                    oc.deskIsPickedUp = false;
                    hasPickedUp = false;
                    deskStandScript.isPickedUp = false;
                    pickedUpDeer.GetComponent<ReindeerAI>().shouldMove = true;
                    pickedUpDeer = null;
                }
            }
        }
    }
}
