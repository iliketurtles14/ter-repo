using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeskPickUp : MonoBehaviour
{
    private DeskInv deskScript;
    private Transform player;
    private MouseCollisionOnItems mcs;
    private OutfitController outfitControllerScript;
    private BodyController bodyControllerScript;
    private DeskStand deskStandScript;
    private HPAChecker HPAScript;
    private float distance;
    private float distance2;
    public bool isPickedUp;
    private GameObject desk;
    private Vector3 deskVector;
    private List<GameObject> touchedFloors = new List<GameObject>();
    private bool isBusy;
    private PlayerCollectionData playerColData;
    private Transform badObjects;
    private void Start()
    {
        deskScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("DeskMenuPanel").GetComponent<DeskInv>();
        player = RootObjectCache.GetRoot("Player").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        outfitControllerScript = player.GetComponent<OutfitController>();
        bodyControllerScript = player.GetComponent<BodyController>();
        deskStandScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<DeskStand>();
        HPAScript = player.GetComponent<HPAChecker>();
        playerColData = player.GetComponent<PlayerCollectionData>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;

        deskVector = new Vector3(0, .8f);
    }
    private void Update()
    {
        HPAScript.hasPickedUp = isPickedUp;
        isBusy = HPAScript.isBusy;
        
        if (!isBusy && mcs.isTouchingDesk && !isPickedUp && mcs.touchedDesk == gameObject)
        {
            distance = Vector2.Distance(player.position, mcs.touchedDesk.transform.position);
            if (distance <= 2.4f && Input.GetMouseButtonDown(1) && !deskScript.deskIsOpen)
            {
                isPickedUp = true;
                desk = mcs.touchedDesk;
                PickUpDesk(mcs.touchedDesk);
                desk.transform.position = player.position + deskVector;
            }
        }
        else if (isPickedUp)
        {
            outfitControllerScript.deskIsPickedUp = true;
            bodyControllerScript.deskIsPickedUp = true;
            
            desk.transform.position = player.position + deskVector;

            if (mcs.isTouchingFloor)
            {
                distance2 = Vector2.Distance(player.position, mcs.touchedFloor.transform.position);
                if(distance2 <= 2.4f && Input.GetMouseButtonDown(1) && !deskScript.deskIsOpen && !player.GetComponent<CapsuleCollider2D>().IsTouching(mcs.touchedFloor.GetComponent<Collider2D>()))
                {
                    isPickedUp = false;
                    outfitControllerScript.deskIsPickedUp = false;
                    bodyControllerScript.deskIsPickedUp = false;
                    DropDesk(mcs.touchedFloor);
                }
            }
        }
        else if(isPickedUp && playerColData.playerData.isDead)
        {
            isPickedUp = false;
            outfitControllerScript.deskIsPickedUp = false;
            bodyControllerScript.deskIsPickedUp = false;
            DropDesk(desk);
        }
    }
    private void PickUpDesk(GameObject aDesk)
    {
        PSoundController.PlaySound("pickup");
        aDesk.GetComponent<BoxCollider2D>().isTrigger = true;
        aDesk.GetComponent<SpriteRenderer>().sortingOrder = 7;
        deskStandScript.isPickedUp = true;

        foreach (Transform bo in badObjects)
        {
            if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, aDesk.transform.position) <= .1f && bo.name == "openHole")
            {
                bo.gameObject.SetActive(true);
                break;
            }
        }
    }
    private void DropDesk(GameObject floor)
    {
        PSoundController.PlaySound("throw");
        GetComponent<BoxCollider2D>().isTrigger = false;
        transform.position = floor.transform.position;
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        deskStandScript.isPickedUp = false;

        foreach(Transform bo in badObjects)
        {
            if(Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, floor.transform.position) <= .1f && bo.name == "openHole")
            {
                bo.gameObject.SetActive(false);
                break;
            }
        }
    }
}
