using UnityEngine;

public class LayerController : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public int currentLayer = 1;
    public Transform tiles;
    public Transform grounds;
    private bool hasSwitched;
    private void Update()
    {
        int oldLayer = currentLayer;
        if(mcs.isTouchingButton && Input.GetMouseButtonDown(0))
        {
            switch (mcs.touchedButton.name)
            {
                case "GroundButton":
                    currentLayer = 1;
                    break;
                case "UndergroundButton":
                    currentLayer = 0;
                    break;
                case "VentsButton":
                    currentLayer = 2;
                    break;
                case "RoofButton":
                    currentLayer = 3;
                    break;
                case "ZonesButton":
                    currentLayer = 4;
                    break;
            }
        }

        if(oldLayer != currentLayer)
        {
            hasSwitched = false;
        }

        if (!hasSwitched)
        {
            DisableAllLayers();
            switch (currentLayer)
            {
                case 0:
                    EnableUnderground();
                    break;
                case 1:
                    EnableGround();
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
    }
    private void EnableGround()
    {
        tiles.Find("Ground").gameObject.SetActive(true);
        tiles.Find("GroundObjects").gameObject.SetActive(true);
        grounds.Find("Ground").GetComponent<SpriteRenderer>().enabled = true;
    }
    private void EnableUnderground()
    {
        tiles.Find("Underground").gameObject.SetActive(true);
        tiles.Find("UndergroundObjects").gameObject.SetActive(true);
        grounds.Find("Underground").GetComponent<SpriteRenderer>().enabled = true;
    }
    private void EnableVents()
    {
        tiles.Find("Vent").gameObject.SetActive(true);
        tiles.Find("VentObjects").gameObject.SetActive(true);
        grounds.Find("Vent").GetComponent<SpriteRenderer>().enabled = true;
    }
    private void EnableRoof()
    {
        tiles.Find("Roof").gameObject.SetActive(true);
        tiles.Find("RoofObjects").gameObject.SetActive(true);
        grounds.Find("Roof").GetComponent<SpriteRenderer>().enabled = true;
    }
    private void EnableZones()
    {
        tiles.Find("Zones").gameObject.SetActive(true);
        grounds.Find("Zones").GetComponent<SpriteRenderer>().enabled = true;
    }
}
