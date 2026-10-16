using System.Collections;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public int currentLayer = 1;
    public Transform tiles;
    public Transform grounds;
    public Transform canvases;
    private bool hasSwitched;
    private void Start()
    {
        StartCoroutine(LayerLoop());
    }
    private IEnumerator LayerLoop()
    {
        while (true)
        {
            int oldLayer = currentLayer;
            yield return new WaitForEndOfFrame();
            if (oldLayer != currentLayer)
            {
                hasSwitched = false;
            }
        }
    }
    private void Update()
    {
        if (!hasSwitched)
        {
            DisableAllLayers();
            switch (currentLayer)
            {
                case 0:
                    EnableUnderground();
                    break;
                case 1:
                    EnableGround(true);
                    break;
                case 2:
                    EnableVents();
                    break;
                case 3:
                    EnableRoof();
                    break;
                case 4:
                    EnableZones();
                    break;
            }
            hasSwitched = true;
        }
    }
    private void DisableAllLayers()
    {
        tiles.Find("Ground").gameObject.SetActive(false);
        tiles.Find("Underground").gameObject.SetActive(false);
        tiles.Find("Vent").gameObject.SetActive(false);
        tiles.Find("Roof").gameObject.SetActive(false);
        tiles.Find("Zones").gameObject.SetActive(false);
        tiles.Find("GroundObjects").gameObject.SetActive(false);
        tiles.Find("UndergroundObjects").gameObject.SetActive(false);
        tiles.Find("VentObjects").gameObject.SetActive(false);
        tiles.Find("RoofObjects").gameObject.SetActive(false);
        grounds.Find("Ground").GetComponent<SpriteRenderer>().enabled = false;
        grounds.Find("Underground").GetComponent<SpriteRenderer>().enabled = false;
        grounds.Find("Vent").GetComponent<SpriteRenderer>().enabled = false;
        grounds.Find("Roof").GetComponent<SpriteRenderer>().enabled = false;
        grounds.Find("Zones").GetComponent<SpriteRenderer>().enabled = false;
        canvases.Find("Ground").gameObject.SetActive(false);
        canvases.Find("Underground").gameObject.SetActive(false);
        canvases.Find("Vent").gameObject.SetActive(false);
        canvases.Find("Roof").gameObject.SetActive(false);
        DisableFunctionlessObjects("Ground");
        DisableFunctionlessObjects("Vent");
        DisableFunctionlessObjects("Roof");
        DisableFunctionlessObjects("Underground");
    }
    private void EnableGround(bool withCanvases)
    {
        tiles.Find("Ground").gameObject.SetActive(true);
        tiles.Find("GroundObjects").gameObject.SetActive(true);
        grounds.Find("Ground").GetComponent<SpriteRenderer>().enabled = true;
        if (withCanvases)
        {
            canvases.Find("Ground").gameObject.SetActive(true);
            EnableFunctionlessObjects("Ground");
        }
    }
    private void EnableUnderground()
    {
        tiles.Find("Underground").gameObject.SetActive(true);
        tiles.Find("UndergroundObjects").gameObject.SetActive(true);
        grounds.Find("Underground").GetComponent<SpriteRenderer>().enabled = true;
        canvases.Find("Underground").gameObject.SetActive(true);
        EnableFunctionlessObjects("Underground");
    }
    private void EnableVents()
    {
        tiles.Find("Vent").gameObject.SetActive(true);
        tiles.Find("VentObjects").gameObject.SetActive(true);
        grounds.Find("Vent").GetComponent<SpriteRenderer>().enabled = true;
        canvases.Find("Vent").gameObject.SetActive(true);
        EnableGround(false);
        EnableFunctionlessObjects("Vent");
    }
    private void EnableRoof()
    {
        tiles.Find("Roof").gameObject.SetActive(true);
        tiles.Find("RoofObjects").gameObject.SetActive(true);
        grounds.Find("Roof").GetComponent<SpriteRenderer>().enabled = true;
        canvases.Find("Roof").gameObject.SetActive(true);
        EnableGround(false);
        EnableFunctionlessObjects("Roof");
    }
    private void EnableZones()
    {
        tiles.Find("Zones").gameObject.SetActive(true);
        grounds.Find("Zones").GetComponent<SpriteRenderer>().enabled = true;
        EnableGround(false);
    }
    private void EnableFunctionlessObjects(string layer) //Ground, Vent, Roof, Underground
    {
        foreach(Transform obj in tiles.Find(layer + "Objects"))
        {
            if (obj.name.Contains("-"))
            {
                obj.Find("FunctionlessObj").gameObject.SetActive(true);
            }
        }
    }
    private void DisableFunctionlessObjects(string layer)
    {
        foreach(Transform obj in tiles.Find(layer + "Objects"))
        {
            if (obj.name.Contains("-"))
            {
                obj.Find("FunctionlessObj").gameObject.SetActive(false);
            }
        }
    }
}
