using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zones : MonoBehaviour
{
    public bool isTouchingCells;
    public bool isTouchingUnsafe;
    public bool isTouchingSafe;
    public bool isTouchingEscape;
    public bool isTouchingSolitary;
    public bool isTouchingCurrentZone;

    public bool isOverriding;

    private float baseDistanceFromPlayer = 250f;
    private float speed = 5f;
    private float amplitude = 25f;
    private float angle;

    private float distanceFromPlayer;

    private Schedule scheduleScript;
    private string periodCode;
    private Transform player;
    private Transform tiles;
    private List<GameObject> specialZones = new List<GameObject>();
    private GameObject arrow;
    private Transform ic;

    private Dictionary<string, string> zoneDict = new Dictionary<string, string>() //go from period code to zone name
    {
        { "LO", "YourCell" }, { "R", "Rollcall" }, { "B", "Canteen" }, { "L", "Canteen" },
        { "D", "Canteen" }, { "E", "Gym" }, { "S", "Showers" }
    };
    private List<string> jobZones = new List<string>()
    {
        "Woodshop", "Metalshop", "Deliveries", "Kitchen", "Laundry", "Tailorshop"
    };

    private void Start()
    {
        isTouchingCurrentZone = true;
        
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        arrow = RootObjectCache.GetRoot("MenuCanvas").transform.Find("ZoneArrow").gameObject;
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GetSpecialZones();
        StartCoroutine(CheckLoop());
        StartCoroutine(ArrowLoop());
        StartCoroutine(ArrowAnim());
    }
    private void GetSpecialZones()
    {
        foreach (Transform zone in tiles.Find("Zones"))
        {
            if (zone.name == "Unsafe" || zone.name == "Safe" || zone.name == "Escape" ||
                zone.name == "Cells" || zone.name == "Solitary")
            {
                specialZones.Add(zone.gameObject);
            }
        }
    }
    private IEnumerator ArrowAnim() //this just changes the base distance from the player
    {
        while (true)
        {
            distanceFromPlayer = baseDistanceFromPlayer + Mathf.Sin(Time.time * speed) * amplitude;
            
            yield return null;
        }
    }
    private IEnumerator ArrowLoop()
    {
        while (true)
        {
            if (isTouchingCurrentZone)
            {
                arrow.SetActive(false);
                yield return null;
                continue;
            }

            //get current zone
            GameObject currentZone = null;
            foreach(Transform zone in tiles.Find("Zones"))
            {
                if(zone.name == zoneDict[scheduleScript.periodCode])
                {
                    currentZone = zone.gameObject;
                    break;
                }
            }
            if(currentZone == null)
            {
                arrow.SetActive(false);
                yield return null;
                continue;
            }

            Vector2 direction = (Vector2)currentZone.transform.position - (Vector2)ic.position;
            
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            float xDist = Mathf.Cos(angle * Mathf.Deg2Rad) * distanceFromPlayer;
            float yDist = Mathf.Sin(angle * Mathf.Deg2Rad) * distanceFromPlayer;

            arrow.transform.localPosition = new Vector3(xDist, yDist, 0);

            if (!isTouchingCurrentZone)
            {
                arrow.SetActive(true);
            }

            yield return null;
        }
    }
    private IEnumerator CheckLoop()
    {
        while (true)
        {
            if (isOverriding)
            {
                yield return null;
                continue;
            }
            
            if(player.gameObject.layer != 3) //if not on ground layer
            {
                isTouchingCurrentZone = true;
                yield return new WaitForSeconds(.1f);
                continue;
            }
            
            periodCode = scheduleScript.periodCode;
            if(periodCode != "FT" && periodCode != "W")
            {
                bool zoneExists = false;
                List<GameObject> currentZones = new List<GameObject>();
                foreach (Transform zone in tiles.Find("Zones"))
                {
                    if(zone.name == zoneDict[periodCode])
                    {
                        currentZones.Add(zone.gameObject);
                        zoneExists = true;
                    }
                }

                if (!zoneExists)
                {
                    isTouchingCurrentZone = true;
                    yield return new WaitForSeconds(.1f);
                    continue;
                }

                foreach(GameObject zone in currentZones)
                {
                    if (player.GetComponent<CapsuleCollider2D>().IsTouching(zone.GetComponent<BoxCollider2D>()))
                    {
                        isTouchingCurrentZone = true;
                        break;
                    }
                    else
                    {
                        isTouchingCurrentZone = false;
                    }
                }
            }
            else if(periodCode == "FT")
            {
                isTouchingCurrentZone = true;
            }
            else if(periodCode == "W")
            {
                if (jobZones.Contains(player.GetComponent<PlayerCollectionData>().playerData.job))
                {
                    List<GameObject> currentZones = new List<GameObject>();
                    bool zoneExists = false;
                    string job = player.GetComponent<PlayerCollectionData>().playerData.job;
                    foreach(Transform zone in tiles.Find("Zones"))
                    {
                        if(zone.name == job)
                        {
                            currentZones.Add(zone.gameObject);
                            zoneExists = true;
                        }
                    }

                    if (!zoneExists)
                    {
                        isTouchingCurrentZone = true;
                        yield return new WaitForSeconds(.1f);
                        continue;
                    }

                    foreach (GameObject zone in currentZones)
                    {
                        if (player.GetComponent<CapsuleCollider2D>().IsTouching(zone.GetComponent<BoxCollider2D>()))
                        {
                            isTouchingCurrentZone = true;
                            break;
                        }
                        else
                        {
                            isTouchingCurrentZone = false;
                        }
                    }
                }
            }

            isTouchingCells = false;
            isTouchingEscape = false;
            isTouchingSafe = false;
            isTouchingUnsafe = false;
            isTouchingSolitary = false;
            foreach(GameObject zone in specialZones)
            {
                if (player.GetComponent<CapsuleCollider2D>().IsTouching(zone.GetComponent<BoxCollider2D>()))
                {
                    switch (zone.name)
                    {
                        case "Cells":
                            isTouchingCells = true;
                            break;
                        case "Escape":
                            isTouchingEscape = true;
                            break;
                        case "Safe":
                            isTouchingSafe = true;
                            break;
                        case "Unsafe":
                            isTouchingUnsafe = true;
                            break;
                        case "Solitary":
                            isTouchingSolitary = true;
                            break;
                    }
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
