using Unity.VisualScripting;
using UnityEngine;

public class BedDummyBadAction : MonoBehaviour
{
    public bool hasDummy;
    private Transform tiles;
    private bool madeBadAction;
    private MakeBadObject mbo;
    private Zones zonesScript;
    private UnlockDoors unlockDoorsScript;
    private Transform badObjects;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        mbo = RootObjectCache.GetRoot("ScriptObject").GetComponent<MakeBadObject>();
        unlockDoorsScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<UnlockDoors>();
        zonesScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Zones>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
    }
    private void Update()
    {
        hasDummy = false;
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name.Contains("Dummy"))
            {
                if(Vector2.Distance(transform.position, obj.position) <= .1f)
                {
                    hasDummy = true;
                    break;
                }
            }
        }

        if(!hasDummy && !madeBadAction && unlockDoorsScript.hasLockedCells && !zonesScript.isTouchingYourCell)
        {
            madeBadAction = true;
            MakeBadAction();
        }
        else if(madeBadAction && (hasDummy || !unlockDoorsScript.hasLockedCells || zonesScript.isTouchingYourCell))
        {
            madeBadAction = false;
            DestroyBadAction();
        }
    }
    private void MakeBadAction()
    {
        BadObjectData data = new BadObjectData
        {
            solitary = true,
            attachedObject = gameObject
        };
        mbo.CreateBadObject(data, "noDummy");
    }
    private void DestroyBadAction()
    {
        foreach(Transform bo in badObjects)
        {
            if(bo.name == "noDummy" && Vector2.Distance(bo.position, transform.position) <= .01f)
            {
                Destroy(bo.gameObject);
                break;
            }
        }
    }
}
