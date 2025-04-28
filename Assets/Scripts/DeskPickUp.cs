using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeskPickUp : MonoBehaviour
{
    public DeskInv deskScript;
    public Transform player;
    public MouseCollisionOnItems mcs;
    public PlayerAnimation playerAnimationScript;
    public ApplyPrisonData applyScript;
    public OutfitController outfitControllerScript;
    private float distance;
    private float distance2;
    public bool isPickedUp;
    private GameObject desk;
    private Vector3 deskVector;
    private List<GameObject> touchedFloors = new List<GameObject>();

    private void Start()
    {
        deskVector = new Vector3(0, .8f);
    }
    private void Update()
    {
        if (mcs.isTouchingDesk && !isPickedUp)
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
            
            desk.transform.position = player.position + deskVector;

            if (mcs.isTouchingFloor)
            {
                distance2 = Vector2.Distance(player.position, mcs.touchedFloor.transform.position);
                if(distance2 <= 2.4f && Input.GetMouseButtonDown(1) && !deskScript.deskIsOpen && !player.GetComponent<PolygonCollider2D>().IsTouching(mcs.touchedFloor.GetComponent<BoxCollider2D>()))
                {
                    isPickedUp = false;
                    outfitControllerScript.deskIsPickedUp = false;
                    DropDesk(mcs.touchedFloor);
                }
            }
        }
    }
    private void PickUpDesk(GameObject aDesk)
    {
        int pickedUpLayer = LayerMask.NameToLayer("PickedUpDesk");
        aDesk.layer = pickedUpLayer;
        aDesk.GetComponent<SpriteRenderer>().sortingOrder = 8;

        switch (NPCSave.instance.playerCharacter)
        {
            case 1: playerAnimationScript.bodyDirSprites = applyScript.RabbitHoldingSprites; break;
            case 2: playerAnimationScript.bodyDirSprites = applyScript.BaldEagleHoldingSprites; break;
            case 3: playerAnimationScript.bodyDirSprites = applyScript.LiferHoldingSprites; break;
            case 4: playerAnimationScript.bodyDirSprites = applyScript.YoungBuckHoldingSprites; break;
            case 5: playerAnimationScript.bodyDirSprites = applyScript.OldTimerHoldingSprites; break;
            case 6: playerAnimationScript.bodyDirSprites = applyScript.BillyGoatHoldingSprites; break;
            case 7: playerAnimationScript.bodyDirSprites = applyScript.FrosephHoldingSprites; break;
            case 8: playerAnimationScript.bodyDirSprites = applyScript.TangoHoldingSprites; break;
            case 9: playerAnimationScript.bodyDirSprites = applyScript.MaruHoldingSprites; break;
        }
        playerAnimationScript.outfitDirSprites = applyScript.InmateOutfitHoldingSprites;

    }
    private void DropDesk(GameObject floor)
    {
        desk.transform.position = floor.transform.position;
        desk.layer = 10;
        desk.GetComponent<SpriteRenderer>().sortingOrder = 3;

        switch (NPCSave.instance.playerCharacter)
        {
            case 1: playerAnimationScript.bodyDirSprites = DataSender.instance.RabbitSprites; break;
            case 2: playerAnimationScript.bodyDirSprites = DataSender.instance.BaldEagleSprites; break;
            case 3: playerAnimationScript.bodyDirSprites = DataSender.instance.LiferSprites; break;
            case 4: playerAnimationScript.bodyDirSprites = DataSender.instance.YoungBuckSprites; break;
            case 5: playerAnimationScript.bodyDirSprites = DataSender.instance.OldTimerSprites; break;
            case 6: playerAnimationScript.bodyDirSprites = DataSender.instance.BillyGoatSprites; break;
            case 7: playerAnimationScript.bodyDirSprites = DataSender.instance.FrosephSprites; break;
            case 8: playerAnimationScript.bodyDirSprites = DataSender.instance.TangoSprites; break;
            case 9: playerAnimationScript.bodyDirSprites = DataSender.instance.MaruSprites; break;
        }
        playerAnimationScript.outfitDirSprites = DataSender.instance.InmateOutfitSprites;

    }
}
