using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SeeBadActions : MonoBehaviour
{
    private List<Vector2> upVectors = new List<Vector2>();
    private List<Vector2> rightVectors = new List<Vector2>();
    private List<Vector2> downVectors = new List<Vector2>();
    private List<Vector2> leftVectors = new List<Vector2>();
    private List<Vector2> currentVectors;
    public float rangeOfSight = 8f;
    private Transform player;
    private Map map;
    private bool canSee = true;
    public List<string> availableBadObjects = new List<string>();
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        map = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        MakeVetorLists();
        MakeBadObjectList();

        StartCoroutine(LookWait());
    }
    private void MakeVetorLists()
    {
        //upVectors
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 162f), Mathf.Sin(Mathf.Deg2Rad * 162f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 144f), Mathf.Sin(Mathf.Deg2Rad * 144f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 126f), Mathf.Sin(Mathf.Deg2Rad * 126f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 108f), Mathf.Sin(Mathf.Deg2Rad * 108f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90f), Mathf.Sin(Mathf.Deg2Rad * 90f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 72f), Mathf.Sin(Mathf.Deg2Rad * 72f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 54f), Mathf.Sin(Mathf.Deg2Rad * 54f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 36f), Mathf.Sin(Mathf.Deg2Rad * 36f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 18f), Mathf.Sin(Mathf.Deg2Rad * 18f)));
        //rightVectors
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 72f), Mathf.Sin(Mathf.Deg2Rad * 72f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 54f), Mathf.Sin(Mathf.Deg2Rad * 54f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 36f), Mathf.Sin(Mathf.Deg2Rad * 36f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 18f), Mathf.Sin(Mathf.Deg2Rad * 18f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 0f), Mathf.Sin(Mathf.Deg2Rad * 0f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 342f), Mathf.Sin(Mathf.Deg2Rad * 342f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 324f), Mathf.Sin(Mathf.Deg2Rad * 324f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 306f), Mathf.Sin(Mathf.Deg2Rad * 306f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 288f), Mathf.Sin(Mathf.Deg2Rad * 288f)));
        //leftVectors
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 108f), Mathf.Sin(Mathf.Deg2Rad * 108f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 126f), Mathf.Sin(Mathf.Deg2Rad * 126f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 144f), Mathf.Sin(Mathf.Deg2Rad * 144f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 162f), Mathf.Sin(Mathf.Deg2Rad * 162f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 180f), Mathf.Sin(Mathf.Deg2Rad * 180f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 198f), Mathf.Sin(Mathf.Deg2Rad * 198f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 216f), Mathf.Sin(Mathf.Deg2Rad * 216f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 234f), Mathf.Sin(Mathf.Deg2Rad * 234f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 252f), Mathf.Sin(Mathf.Deg2Rad * 252f)));
        //downVectors
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 198f), Mathf.Sin(Mathf.Deg2Rad * 198f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 216f), Mathf.Sin(Mathf.Deg2Rad * 216f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 234f), Mathf.Sin(Mathf.Deg2Rad * 234f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 252f), Mathf.Sin(Mathf.Deg2Rad * 252f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270f), Mathf.Sin(Mathf.Deg2Rad * 270f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 288f), Mathf.Sin(Mathf.Deg2Rad * 288f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 306f), Mathf.Sin(Mathf.Deg2Rad * 306f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 324f), Mathf.Sin(Mathf.Deg2Rad * 324f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 342f), Mathf.Sin(Mathf.Deg2Rad * 342f)));
    }
    private void MakeBadObjectList()
    {
        availableBadObjects.Add("onDesk");
        availableBadObjects.Add("guardSearchingDesk");
        availableBadObjects.Add("inmateSearchingDesk");
        availableBadObjects.Add("pickedUp");
        availableBadObjects.Add("inWrongCell");
        availableBadObjects.Add("playerPunching");
        availableBadObjects.Add("wrongRoutine");
        availableBadObjects.Add("notAtWork");
        availableBadObjects.Add("noOutfit");
        availableBadObjects.Add("guardNonInmateOutfit");
        availableBadObjects.Add("inmateNonInmateOutfit");
        availableBadObjects.Add("highHeat");
        availableBadObjects.Add("inmateBreakingTile");
        availableBadObjects.Add("guardBreakingTile");
    }
    private IEnumerator LookWait()
    {
        float rand = UnityEngine.Random.Range(0f, .1f);
        yield return new WaitForSeconds(rand);
        StartCoroutine(Look());
    }
    private IEnumerator Look()
    {
        while (true)
        {
            switch (GetComponent<NPCAnimation>().lookDir)
            {
                case "up":
                    currentVectors = upVectors;
                    break;
                case "right":
                    currentVectors = rightVectors;
                    break;
                case "left":
                    currentVectors = leftVectors;
                    break;
                case "down":
                    currentVectors = downVectors;
                    break;
            }

            foreach (Vector2 vector in currentVectors)
            {
                RaycastHit2D[] wallHits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight);

                RaycastHit2D? wallHit = null;
                foreach (RaycastHit2D aHit in wallHits)
                {
                    if (aHit.collider.CompareTag("Wall"))
                    {
                        wallHit = aHit;
                        break;
                    }
                }

                if (wallHit.HasValue)
                {
                    Debug.DrawLine(transform.position, wallHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }

                RaycastHit2D[] badHits;
                try
                {
                    badHits = Physics2D.RaycastAll(transform.position, wallHit.Value.point, wallHit.Value.distance);

                }
                catch
                {
                    badHits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight);
                }

                RaycastHit2D? badHit = null;
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        Debug.Log(aHit.collider.name);
                    }
                }
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        if (availableBadObjects.Contains(aHit.collider.name) && !GetComponent<NPCCombat>().isAggro && aHit.collider.name != "playerPunching")
                        {
                            badHit = aHit;
                            SeeBadAction(aHit.transform.gameObject);
                        }
                        else if (availableBadObjects.Contains(aHit.collider.name) && aHit.collider.name == "playerPunching")
                        {
                            badHit = aHit;
                            SeeBadAction(aHit.transform.gameObject);
                        }
                    }
                }

                if (badHit.HasValue)
                {
                    Debug.DrawLine(transform.position, badHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    public void SeeBadAction(GameObject badObject)
    {
        if(badObject.name != "highHeat") //add things here that shouldnt have a cooldown
        {
            availableBadObjects.Remove(badObject.name);
        }

        BadObjectData data = badObject.GetComponent<BadObjectData>();
        bool isGuard = GetComponent<NPCCollectionData>().npcData.isGuard;

        //add heat
        int heatToAdd = 0;
        if (data.isMultiplied && isGuard)
        {
            heatToAdd = data.heatGain * map.npcLevel;
        }
        else if(!data.isMultiplied && isGuard)
        {
            heatToAdd = data.heatGain;
        }
        player.GetComponent<PlayerCollectionData>().playerData.heat += heatToAdd;

        //set heat
        if(data.heatSet != -1 && isGuard)
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat = data.heatSet;
        }

        //should aggro
        if (data.shouldAggro && !data.forInmate) //make this also work for searching inmate desks when the same inmate sees it
        {
            GetComponent<NPCCombat>().isAggro = true;
            GetComponent<NPCCombat>().target = data.attachedObject;
        }

        //solitary
        if (data.solitary && isGuard)
        {
            Debug.Log("solitary");
        }

        //item
        if (data.item && isGuard)
        {
            Debug.Log("item");
        }

        //toilet
        if (data.toilet && isGuard)
        {
            Debug.Log("toilet");
        }

        //untie
        if (data.untie && isGuard)
        {
            Debug.Log("untie");
        }

        //sheets
        if (data.sheets && isGuard)
        {
            Debug.Log("sheets");
        }

        //message type
        if(data.messageType != null)
        {
            NPCSpeech speechScript = GetComponent<NPCSpeech>();
            speechScript.MakeTextBox(speechScript.GetMessage(data.messageType), transform, true);
        }

        //should call
        if (data.shouldCall && !isGuard && data.forInmate)
        {
            Debug.Log("call");
            //remember to add 30 heat
        }

        //do timers
        if (badObject.name != "guardNonInmateOutfit" && badObject.name != "inmateNonInmateOutfit")
        {
            StartCoroutine(SeeBadCooldown(5, badObject.name));
        }
        else if (badObject.name == "guardNonInmateOutfit" || badObject.name == "inmateNonInmateOutfit")
        {
            StartCoroutine(SeeBadCooldown(2, badObject.name));
        }
    }

    private IEnumerator SeeBadCooldown(float time, string name)
    {
        yield return new WaitForSeconds(time);
        availableBadObjects.Add(name);
    }
}
