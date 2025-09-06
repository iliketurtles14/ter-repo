using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HoleClimb : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private GameObject player;
    private Transform tiles;
    private GameObject undergroundLight;
    private GameObject globalLight;
    private GameObject currentHole;
    private bool hasPickedUp;
    public void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        undergroundLight = RootObjectCache.GetRoot("UndergroundLight");
        globalLight = RootObjectCache.GetRoot("GlobalLight");

        undergroundLight.SetActive(false);

        mcs.DisableTag("Dirt");
        mcs.DisableTag("EmptyDirt");

        foreach (Transform child in tiles.Find("UndergroundObjects"))
        {
            if (child.gameObject.CompareTag("Item"))
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    public void Update()
    {
        if(player.layer == 11)
        {
            undergroundLight.transform.position = player.transform.position;
        }

        if(player.layer != 11)
        {
            foreach (Transform child in tiles.Find("UndergroundObjects"))
            {
                if (child.gameObject.CompareTag("Item"))
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }

        if (mcs.isTouchingHoleDown && player.layer == 3)
        {
            float distance = Vector2.Distance(player.transform.position, mcs.touchedHoleDown.transform.position);
            if (Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Desk"))
                {
                    if (obj.layer == 14)
                    {
                        hasPickedUp = true;
                        break;
                    }
                    else
                    {
                        hasPickedUp = false;
                    }
                }

                if (!hasPickedUp)
                {
                    currentHole = mcs.touchedHoleDown;
                    StartCoroutine(ClimbDown());
                }
            }
        }
        else if(mcs.isTouchingHoleUp && player.layer == 11)
        {
            float distance = Vector2.Distance(player.transform.position, mcs.touchedHoleUp.transform.position);
            if (Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                currentHole = mcs.touchedHoleUp;
                StartCoroutine(ClimbUp());
            }
        }
    }
    public IEnumerator ClimbUp()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForEndOfFrame();

        player.transform.position = currentHole.transform.position;
        player.layer = 3;
        player.GetComponent<SpriteRenderer>().sortingOrder = 6;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
        foreach(Transform tile in tiles.Find("Underground"))
        {
            tile.GetComponent<SpriteRenderer>().sortingOrder = -5;

            if (tile.name == ("Dirt(Clone)"))
            {
                tile.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        foreach(Transform obj in tiles.Find("UndergroundObjects"))
        {
            if(obj.name == "Rock(Clone)" || obj.name == "Mine(Clone)" || obj.name == "Brace(Clone)")
            {
                obj.GetComponent<SpriteRenderer>().sortingOrder = -4;
            }
            if(obj.name == "100%HoleUp(Clone)" || obj.name == "HalfHoleUp(Clone)")
            {
                obj.GetComponent<Light2D>().intensity = 0;
            }
        }
        tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingOrder = -6;
        undergroundLight.SetActive(false);
        globalLight.SetActive(true);

        mcs.DisableTag("Dirt");
        mcs.DisableTag("EmptyDirt");
        foreach(Transform child in tiles.Find("GroundObjects"))
        {
            if (child.gameObject.CompareTag("Item"))
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        foreach (Transform child in tiles.Find("UndergroundObjects"))
        {
            if (child.gameObject.CompareTag("Item"))
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        mcs.EnableTag("Bars");
        mcs.EnableTag("Fence");
        mcs.EnableTag("ElectricFence");
        mcs.EnableTag("Digable");
        mcs.EnableTag("Wall");
        mcs.EnableTag("Ladder(Ground)");
        mcs.EnableTag("HoleDown");
        mcs.EnableTag("Desk");//currently the only menu

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public IEnumerator ClimbDown()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForEndOfFrame();

        player.transform.position = currentHole.transform.position;
        player.layer = 11;
        player.GetComponent<SpriteRenderer>().sortingOrder = 12;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 13;
        foreach(Transform tile in tiles.Find("Underground"))
        {
            tile.GetComponent<SpriteRenderer>().sortingOrder = 10;

            if (tile.name.StartsWith("Dirt(Clone)"))
            {
                tile.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        foreach (Transform obj in tiles.Find("UndergroundObjects"))
        {
            if(obj.name == "Rock(Clone)" || obj.name == "Mine(Clone)" || obj.name == "Brace(Clone)")
            {
                obj.GetComponent<SpriteRenderer>().sortingOrder = 11;
            }
            if (obj.name == "100%HoleUp(Clone)" || obj.name == "HalfHoleUp(Clone)")
            {
                obj.GetComponent<Light2D>().intensity = 15;
            }
        }
        tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingOrder = 9;
        undergroundLight.SetActive(true);
        globalLight.SetActive(false);

        mcs.EnableTag("Dirt");
        mcs.EnableTag("EmptyDirt");
        foreach (Transform child in tiles.Find("GroundObjects"))
        {
            if (child.gameObject.CompareTag("Item"))
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        foreach(Transform child in tiles.Find("UndergroundObjects"))
        {
            if (child.gameObject.CompareTag("Item"))
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        mcs.DisableTag("Bars");
        mcs.DisableTag("Fence");
        mcs.DisableTag("ElectricFence");
        mcs.DisableTag("Digable");
        mcs.DisableTag("Wall");
        mcs.DisableTag("Ladder(Ground)");
        mcs.DisableTag("HoleDown");
        mcs.DisableTag("Desk");//currently the only menu

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
