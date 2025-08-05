using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.XR;

public class ZonePlacer : MonoBehaviour
{
    private MouseCollisionOnMap mcs;
    private bool isGrabbingHandle;
    private GameObject grabbedHandle;

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

        if (isGrabbingHandle)
        {
            Transform handle = grabbedHandle.transform;
            Transform zoneObj = handle.parent;

            Vector3 newPos;
            Vector2 newScale;
            Vector3 newHandlePos;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            switch (handle.gameObject.name)
            {
                case "NW":
                    if(mousePos.y >= .8f + handle.position.y) //check for positive y
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(0, .8f);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + new Vector3(0, .8f);
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + new Vector3(0, .8f);
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    if (mousePos.x <= handle.position.x - .8f) //check for negative x
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(-.8f, 0);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + new Vector3(-.8f, 0);
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + new Vector3(-.8f, 0);
                        zoneObj.Find("NW").position = newHandlePos;
                    }
                    break;
                case "NE":
                    if (mousePos.y >= .8f + handle.position.y) //check for positive y
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(0, .8f);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + new Vector3(0, .8f);
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + new Vector3(0, .8f);
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    if (mousePos.x >= handle.position.x + .8f) //check for positive x
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(.8f, 0);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SE").position + new Vector3(.8f, 0);
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + new Vector3(.8f, 0);
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    break;
                case "SW":
                    if (mousePos.y <= handle.position.y - .8f) //check for negative y
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(0, -.8f);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + new Vector3(0, -.8f);
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + new Vector3(0, -.8f);
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    if (mousePos.x <= handle.position.x - .8f) //check for negative x
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(-.8f, 0);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SW").position + new Vector3(-.8f, 0);
                        zoneObj.Find("SW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NW").position + new Vector3(-.8f, 0);
                        zoneObj.Find("NW").position = newHandlePos;
                    }
                    break;
                case "SE":
                    if (mousePos.y <= handle.position.y - .8f) //check for negative y
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(0, -.8f);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(0, .16f);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("NW").position + new Vector3(0, -.8f);
                        zoneObj.Find("NW").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + new Vector3(0, -.8f);
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    if (mousePos.x >= handle.position.x + .8f) //check for positive x
                    {
                        //change pos
                        newPos = zoneObj.position + new Vector3(.8f, 0);
                        zoneObj.position = newPos;
                        //change scale
                        newScale = zoneObj.GetComponent<SpriteRenderer>().size + new Vector2(.16f, 0);
                        zoneObj.GetComponent<SpriteRenderer>().size = newScale;
                        //change handle pos
                        newHandlePos = zoneObj.Find("SE").position + new Vector3(.8f, 0);
                        zoneObj.Find("SE").position = newHandlePos;
                        newHandlePos = zoneObj.Find("NE").position + new Vector3(.8f, 0);
                        zoneObj.Find("NE").position = newHandlePos;
                    }
                    break;
            }
        }
    }
    public static void SetSystemCursor(int cursorId)
    {
        IntPtr cursor = LoadCursor(IntPtr.Zero, cursorId);
        SetCursor(cursor);
    }
}
