using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class ZonePlacer : MonoBehaviour
{
    private MouseCollisionOnMap mcs;
    private bool isGrabbingHandle;
    private GameObject grabbedHandle;
    private bool isGrabbingMover;
    private GameObject grabbedMover;
    private bool inDeleteMode;
    public Transform zonesLayer;
    public Sprite deleteZoneSprite;
    public Sprite moveZoneSprite;
    public Transform gridLines;

    private List<string> zoneButtonNames = new List<string>
    {
        "YourCell", "Cell", "Safe", "Solitary", "Rollcall", "Canteen",
        "Gym", "Showers", "Janitor", "Gardening", "Woodshop", "Metalshop",
        "Deliveries", "Kitchen", "Laundry", "Tailorshop"
    };

    [DllImport("user32.dll")]
    private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

    [DllImport("user32.dll")]
    private static extern IntPtr SetCursor(IntPtr hCursor);

    // Predefined cursor IDs
    public const int IDC_ARROW = 32512;
    public const int IDC_IBEAM = 32513;
    public const int IDC_WAIT = 32514;
    public const int IDC_CROSS = 32515;
    public const int IDC_UPARROW = 32516;
    public const int IDC_SIZE = 32640;
    public const int IDC_ICON = 32641;
    public const int IDC_SIZENWSE = 32642;
    public const int IDC_SIZENESW = 32643;
    public const int IDC_SIZEWE = 32644;
    public const int IDC_SIZENS = 32645;
    public const int IDC_SIZEALL = 32646;
    public const int IDC_NO = 32648;
    public const int IDC_HAND = 32649;
    public const int IDC_APPSTARTING = 32650;
    public const int IDC_HELP = 32651;

    private void Start()
    {
        mcs = GetComponent<MouseCollisionOnMap>();
    }
    private void Update()
    {
        //if (mcs.isTouchingHandle)
        //{
        //    SetSystemCursor(IDC_HAND);
        //}
        //else
        //{
        //    SetSystemCursor(IDC_ARROW);
        //}

        if(mcs.isTouchingButton && mcs.touchedButton.name == "DeleteZoneButton" && Input.GetMouseButtonDown(0))
        {
            inDeleteMode = true;
            
            foreach(Transform zone in zonesLayer)
            {
                if(zone.name != "empty")
                {
                    zone.Find("Mover").GetComponent<SpriteRenderer>().sprite = deleteZoneSprite;
                }
            }
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "MoveZoneButton" && Input.GetMouseButtonDown(0))
        {
            inDeleteMode = false;

            foreach(Transform zone in zonesLayer)
            {
                if(zone.name != "empty")
                {
                    zone.Find("Mover").GetComponent<SpriteRenderer>().sprite = moveZoneSprite;
                }
            }
        }

        if(mcs.isTouchingButton && zoneButtonNames.Contains(mcs.touchedButton.name) && Input.GetMouseButtonDown(0))
        {
            PlaceZone(mcs.touchedButton.name);
        }

        if(inDeleteMode && mcs.isTouchingMover && Input.GetMouseButtonDown(0)) //delete zone
        {
            Destroy(mcs.touchedMover.transform.parent.gameObject);
        }

        if(mcs.isTouchingHandle && Input.GetMouseButtonDown(0))
        {
            isGrabbingHandle = true;
            grabbedHandle = mcs.touchedHandle;
        }

        if (!Input.GetMouseButton(0))
        {
            isGrabbingHandle = false;
            grabbedHandle = null;
        }

        if(!inDeleteMode && mcs.isTouchingMover && Input.GetMouseButtonDown(0))
        {
            isGrabbingMover = true;
            grabbedMover = mcs.touchedMover;
        }

        if (!Input.GetMouseButton(0))
        {
            isGrabbingMover = false;
            grabbedMover = null;
        }

        if (isGrabbingHandle)
        {
            Transform handle = grabbedHandle.transform;
            Transform zoneObj = handle.parent;

            Vector3 newPos;
            Vector2 newScale;
            Vector3 newHandlePos;

            Vector3 posY = new Vector3(0, .8f);
            Vector3 negY = new Vector3(0, -.8f);
            Vector3 posX = new Vector3(.8f, 0);
            Vector3 negX = new Vector3(-.8f, 0);

            Vector2 zoneSize = zoneObj.GetComponent<SpriteRenderer>().size;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            switch (handle.gameObject.name) //magic numbers yayy: 
                                            //bleuh
            {
                case "NW":
                    //making bigger
                    if(mousePos.y >= .8f + handle.position.y) //check for positive y
                    {
                        //change pos
                        newPos = zoneObj.position + posY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + posY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + negY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x <= handle.position.x - .8f) //check for negative x
                    {
                        //change pos
                        newPos = zoneObj.position + negX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + negX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + negX;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + posX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posX;
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    //making smaller
                    if (mousePos.y <= handle.position.y - .8f && zoneSize.y >= .31f) //check for negative y
                    {
                        //change pos
                        newPos = zoneObj.position + negY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, -.16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + negY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + posY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + posY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x >= handle.position.x + .8f && zoneSize.x >= .31f) //check for positive x
                    {
                        //change pos
                        newPos = zoneObj.position + posX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(-.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + posX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + posX;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negX;
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    break;
                case "NE":
                    if (mousePos.y >= .8f + handle.position.y) //check for positive y
                    {
                        //change pos
                        newPos = zoneObj.position + posY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + posY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + negY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x >= handle.position.x + .8f) //check for positive x
                    {
                        //change pos
                        newPos = zoneObj.position + posX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SE").position + posX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posX;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + negX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + negX;
                        zoneObj.Find("NW").position = newHandlePos;
                    }
                    //making smaller
                    if (mousePos.y <= handle.position.y - .8f && zoneSize.y     ) //check for negative y
                    {
                        //change pos
                        newPos = zoneObj.position + negY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, -.16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + negY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + posY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + posY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x <= handle.position.x - .8f && zoneSize.x >= .31f) //check for negative x
                    {
                        //change pos
                        newPos = zoneObj.position + negX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(-.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + posX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + posX;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negX;
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    break;
                case "SW":
                    if (mousePos.y <= handle.position.y - .8f) //check for negative y
                    {
                        //change pos
                        newPos = zoneObj.position + negY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + posY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + negY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x <= handle.position.x - .8f) //check for negative x
                    {
                        //change pos
                        newPos = zoneObj.position + negX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + negX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + negX;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + posX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posX;
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    //making smaller
                    if (mousePos.y >= handle.position.y + .8f && zoneSize.y >= .31f) //check for positive y
                    {
                        //change pos
                        newPos = zoneObj.position + posY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, -.16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + negY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + posY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + posY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x >= handle.position.x + .8f && zoneSize.x >= .31f) //check for positive x
                    {
                        //change pos
                        newPos = zoneObj.position + posX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(-.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + posX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + posX;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negX;
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    break;
                case "SE":
                    if (mousePos.y <= handle.position.y - .8f) //check for negative y
                    {
                        //change pos
                        newPos = zoneObj.position + negY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + posY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + negY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x >= handle.position.x + .8f) //check for positive x
                    {
                        //change pos
                        newPos = zoneObj.position + posX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SE").position + posX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + posX;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + negX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + negX;
                        zoneObj.Find("NW").position = newHandlePos;
                    }
                    //making smaller
                    if (mousePos.y >= handle.position.y + .8f && zoneSize.y >= .31f) //check for positive y
                    {
                        //change pos
                        newPos = zoneObj.position + posY;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, -.16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + negY;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negY;
                        zoneObj.Find("NE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SW").position + posY;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + posY;
                        zoneObj.Find("SE").position = newHandlePos;
                    }
                    if (mousePos.x <= handle.position.x - .8f && zoneSize.x >= .31f) //check for negative x
                    {
                        //change pos
                        newPos = zoneObj.position + negX;
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(-.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + posX;
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + posX;
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("SE").position + negX;
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + negX;
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    break;
            }
        }

        if (isGrabbingMover)
        {
            Transform zoneObj = grabbedMover.transform.parent;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 newPos;

            if (mousePos.x >= zoneObj.position.x + .8f) //pos x
            {
                newPos = new Vector3(zoneObj.position.x + 1.6f, zoneObj.position.y);
                zoneObj.position = newPos;
            }
            if (mousePos.x <= zoneObj.position.x - .8f) //neg x
            {
                newPos = new Vector3(zoneObj.position.x - 1.6f, zoneObj.position.y);
                zoneObj.position = newPos;
            }
            if (mousePos.y >= zoneObj.position.y + .8f) //pos y
            {
                newPos = new Vector3(zoneObj.position.x, zoneObj.position.y + 1.6f);
                zoneObj.position = newPos;
            }
            if (mousePos.y <= zoneObj.position.y - .8f) //neg y
            {
                newPos = new Vector3(zoneObj.position.x, zoneObj.position.y - 1.6f);
                zoneObj.position = newPos;
            }
        }
    }
    public void PlaceZone(string name)
    {
        Vector3 placePos = new Vector3(-.8f, -.8f, 0);

        GameObject newZone = Instantiate(Resources.Load<GameObject>("MapEditorPrefabs/ZoneObject"), placePos, Quaternion.identity, zonesLayer);
        newZone.name = name;
        newZone.transform.Find("NameText").GetComponent<TextMeshPro>().text = name;
        newZone.transform.Find("NameText").GetComponent<MeshRenderer>().sortingOrder = 6;

        if (inDeleteMode)
        {
            newZone.transform.Find("Mover").GetComponent<SpriteRenderer>().sprite = deleteZoneSprite;
        }
    }
    public void SetSystemCursor(int cursorId)
    {
        IntPtr cursor = LoadCursor(IntPtr.Zero, cursorId);
        SetCursor(cursor);
    }
}
