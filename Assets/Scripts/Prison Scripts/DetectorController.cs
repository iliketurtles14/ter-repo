using NUnit.Framework.Constraints;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class DetectorController : MonoBehaviour
{//.55, 3
    private Transform player;
    private bool canTrigger = true;
    private bool hasContraband;
    private Inventory inventoryScript;

    private Sprite horizontalDetectedSprite;
    private Sprite verticalDetectedSprite;
    private Sprite horizontalNormalSprite;
    private Sprite verticalNormalSprite;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        DataSender ds = DataSender.instance;
        verticalNormalSprite = ds.PrisonObjectImages[68];
        verticalDetectedSprite = ds.PrisonObjectImages[69];
        horizontalNormalSprite = ds.PrisonObjectImages[155];
        horizontalDetectedSprite = ds.PrisonObjectImages[156];

        switch (name)
        {
            case "DetectorHorizontal":
                GetComponent<SpriteRenderer>().sprite = horizontalNormalSprite;
                break;
            case "DetectorVertical":
                GetComponent<SpriteRenderer>().sprite = verticalNormalSprite;
                break;
        }
    }
    private void FixedUpdate()
    {
        if (player.GetComponent<CapsuleCollider2D>().IsTouching(GetComponent<BoxCollider2D>()) && canTrigger && hasContraband)
        {
            canTrigger = false;
            StartCoroutine(TriggerDetector());
        }
    }
    private void Update()
    {
        foreach(InventoryItem item in inventoryScript.inventory)
        {
            if (item.itemData.isContraband)
            {
                hasContraband = true;
                break;
            }
            else
            {
                hasContraband = false;
            }
        }
    }
    private IEnumerator TriggerDetector()
    {
        player.GetComponent<PlayerCollectionData>().playerData.heat = 99;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        switch (name)
        {
            case "DetectorVertical":
                for(int i = 0; i < 3; i++)
                {
                    sr.sprite = verticalDetectedSprite;
                    yield return new WaitForSeconds(.55f);
                    sr.sprite = verticalNormalSprite;
                    yield return new WaitForSeconds(.55f);
                }
                break;
            case "DetectorHorizontal":
                for (int i = 0; i < 3; i++)
                {
                    sr.sprite = horizontalDetectedSprite;
                    yield return new WaitForSeconds(.55f);
                    sr.sprite = horizontalNormalSprite;
                    yield return new WaitForSeconds(.55f);
                }
                break;
        }
        canTrigger = true;
    }
}
