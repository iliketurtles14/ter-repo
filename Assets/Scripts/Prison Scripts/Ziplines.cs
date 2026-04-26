using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ziplines : MonoBehaviour
{
    private Transform tiles;
    private List<GameObject> zipsWithEndPoints = new List<GameObject>();
    public bool isZipping;
    private Transform player;
    private InventorySelection selectionScript;
    private Inventory inventoryScript;
    private MouseCollisionOnItems mcs;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        player = RootObjectCache.GetRoot("Player").transform;
        selectionScript = GetComponent<InventorySelection>();
        inventoryScript = GetComponent<Inventory>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SetZipPoints();
        MakeLineObjects();
    }
    private void SetZipPoints()
    {
        foreach(Transform zip in tiles.Find("RoofObjects"))
        {
            if (zip.name.StartsWith("Zip") && zip.name != "ZipEnd")
            {
                List<Vector2> possibleEndPoints = new List<Vector2>();

                foreach (Transform zipEnd in tiles.Find("RoofObjects")) //get possible end points
                {
                    if (zipEnd.name == "ZipEnd")
                    {
                        if((zip.name == "ZipUp" && zipEnd.position.x == zip.position.x && zipEnd.position.y > zip.position.y) ||
                            (zip.name == "ZipDown" && zipEnd.position.x == zip.position.x && zipEnd.position.y < zip.position.y) ||
                            (zip.name == "ZipLeft" && zipEnd.position.y == zip.position.y && zipEnd.position.x < zip.position.x) ||
                            (zip.name == "ZipRight" && zipEnd.position.y == zip.position.y && zipEnd.position.x > zip.position.x))
                        {
                            possibleEndPoints.Add(zipEnd.position);
                        }
                    }
                }

                //get smallest endpoint and set that one
                List<float> endPointDistances = new List<float>();
                foreach(Vector2 possibleEndPoint in possibleEndPoints)
                {
                    endPointDistances.Add(Vector2.Distance(zip.position, possibleEndPoint));
                }

                float smallestDistance = -1;
                int index = 0;
                int smallestIndex = 0;
                foreach(float distance in endPointDistances)
                {
                    if(index == 0 || distance < smallestDistance)
                    {
                        smallestDistance = distance;
                        smallestIndex = index;
                    }
                    index++;
                }

                if(smallestDistance != -1)
                {
                    zip.GetComponent<ZiplineData>().endPoint = possibleEndPoints[smallestIndex];
                    zip.GetComponent<ZiplineData>().hasEndPoint = true;
                    zipsWithEndPoints.Add(zip.gameObject);
                }
            }
        }
    }
    private void MakeLineObjects()
    {
        foreach(GameObject zip in zipsWithEndPoints)
        {
            Vector2 startPoint = zip.transform.position;
            Vector2 endPoint = zip.GetComponent<ZiplineData>().endPoint;

            //get midpoint
            float distX = endPoint.x - startPoint.x;
            float distY = endPoint.y - startPoint.y;
            float midX = (distX / 2f) + startPoint.x;
            float midY = (distY / 2f) + startPoint.y;
            Vector2 midPoint = new Vector2(midX, midY);

            //get width/height
            float width = Mathf.Abs(endPoint.x - startPoint.x);
            float height = Mathf.Abs(endPoint.y - startPoint.y);

            if(width == 0)
            {
                width = height;
            }

            width *= 10f;

            //get zip specific widths and spacing or whatever
            switch (zip.name)
            {
                case "ZipRight":
                case "ZipLeft":
                    midPoint += new Vector2(.05f, .15f);
                    width += -9f;
                    break;
                case "ZipUp":
                    width += -10f;
                    midPoint += new Vector2(.05f, .3f);
                    break;
                case "ZipDown":
                    width += -2f;
                    midPoint += new Vector2(.05f, .3f);
                    break;
                
            }

            //get rotation
            int angle;
            if(zip.name == "ZipLeft" || zip.name == "ZipRight")
            {
                angle = 90;
            }
            else
            {
                angle = 0;
            }

            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameObject line = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/ZiplineString"));
            line.name = "ZiplineString";
            line.transform.parent = tiles.Find("RoofObjects");
            line.transform.position = midPoint;
            line.transform.rotation = rot;
            line.GetComponent<SpriteRenderer>().sprite = DataSender.instance.UIImages[339];
            line.GetComponent<SpriteRenderer>().size = new Vector2(.3f, width * .1f);
        }
    }
    private void Update()
    {
        if(selectionScript.aSlotSelected && inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id == 159 && Input.GetMouseButtonDown(0) && !isZipping && mcs.isTouchingZipline)
        {
            StartCoroutine(Zip(mcs.touchedZipline));
        }
    }
    private IEnumerator Zip(GameObject zip)
    {
        isZipping = true;

        //climb zip
        player.GetComponent<PlayerCtrl>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForFixedUpdate();
        float speed = 5f;
        Vector3 direction = (zip.transform.position - player.position).normalized;
        while(Vector3.Distance(player.position, zip.transform.position) > 0.1f)
        {
            player.position += speed * Time.deltaTime * direction;
            yield return null;
        }
        player.position = zip.transform.position;
        player.GetComponent<PlayerAnimation>().enabled = false;
        BodyController bc = player.GetComponent<BodyController>();
        OutfitController oc = player.GetComponent<OutfitController>();

        int dir = 0;
        switch (zip.name)
        {
            case "ZipRight":
                dir = 0;
                break;
            case "ZipUp":
                dir = 1;
                break;
            case "ZipLeft":
                dir = 2;
                break;
            case "ZipDown":
                dir = 3;
                break;
        }
        player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][14][dir];
        player.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][14][dir];

        //move on zip
        speed = 20f;
        Vector2 startPos = zip.transform.position;
        Vector2 endPos = zip.GetComponent<ZiplineData>().endPoint;
        direction = (endPos - startPos).normalized;
        while(Vector3.Distance(player.position, endPos) > .8f)
        {
            player.position += speed * Time.deltaTime * direction;
            yield return null;
        }

        //get off zip
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PlayerCtrl>().enabled = true;
        player.GetComponent<PlayerAnimation>().enabled = true;
        isZipping = false;
    }
}
