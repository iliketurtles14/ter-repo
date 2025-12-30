using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour //TilePlacer handles the background work that ObjectPlacer uses.
                                          //Probably should've just made another script to control that stuff, but it's whatever.
{
    public Transform tiles;
    public Transform canvases;
    public MouseCollisionOnMap mcs;
    public Transform highlight;
    public bool inDeleteMode;

    private Dictionary<int, int> layerDict = new Dictionary<int, int>() //releates LayerController.currentLayer with the sorting orders of objects depending on the layer
    {
        { 1, 2 }, { 0, 2 }, { 2, 5 }, { 3, 5 }
    };
    private void Update()
    {
        inDeleteMode = GetComponent<TilePlacer>().inDeleteMode;
        
        if(!inDeleteMode && highlight.position != new Vector3(9999, 9999) && !mcs.isTouchingMenu && !mcs.isTouchingButton && !mcs.isTouchingObject && Input.GetMouseButtonDown(0) && GetComponent<ObjectSelect>().hasSelected && GetComponent<TilePlacer>().layer != "Zones")
        {
            PlaceObject(GetComponent<TilePlacer>().layer, GetComponent<ObjectSelect>().selectedObj);
        }

        if (inDeleteMode)
        {
            highlight.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
            highlight.GetComponent<BoxCollider2D>().size = new Vector2(1.5f, 1.5f);
        }

        if(highlight.position != new Vector3(9999, 9999) && !mcs.isTouchingMenu && !mcs.isTouchingButton && !mcs.isTouchingObject && Input.GetMouseButton(0) && inDeleteMode)
        {
            int layer = GetComponent<LayerController>().currentLayer;
            foreach(Transform obj in tiles.Find(GetComponent<TilePlacer>().layer + "Objects"))
            {
                if (highlight.GetComponent<BoxCollider2D>().IsTouching(obj.GetComponent<BoxCollider2D>()) && layerDict[layer] == obj.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    foreach (Transform canvas in canvases.Find(GetComponent<TilePlacer>().layer))
                    {
                        if (highlight.GetComponent<BoxCollider2D>().IsTouching(canvas.GetComponent<BoxCollider2D>()))
                        {
                            Destroy(canvas.gameObject);
                            break;
                        }
                    }
                    Destroy(obj.gameObject);
                    break;
                }
            }
        }

        if(!inDeleteMode && GetComponent<PanelSelect>().currentPanel == "ObjectsPanel" && !highlight.gameObject.activeInHierarchy && GetComponent<ObjectSelect>().hasSelected)
        {
            highlight.gameObject.SetActive(true);
            highlight.GetComponent<SpriteRenderer>().size = GetComponent<ObjectSelect>().selectedObj.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f);
            highlight.GetComponent<BoxCollider2D>().size = GetComponent<ObjectSelect>().selectedObj.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f) - new Vector2(.1f, .1f);
        }
    }
    private void PlaceObject(string layer, GameObject objToPlace)
    {
        bool canPlace = true;
        foreach(Transform obj in tiles.Find(layer + "Objects"))
        {
            if (highlight.GetComponent<BoxCollider2D>().IsTouching(obj.GetComponent<BoxCollider2D>()))
            {
                canPlace = false;
                break;
            }
        }
        if(GetComponent<TilePlacer>().layer == "Zones")
        {
            canPlace = false;
        }
        if (canPlace)
        {
            GameObject placedObj = new GameObject();
            placedObj.AddComponent<SpriteRenderer>();
            placedObj.AddComponent<BoxCollider2D>();
            placedObj.name = objToPlace.name;
            placedObj.transform.position = highlight.position;
            placedObj.transform.SetParent(tiles.Find(layer + "Objects"));
            placedObj.GetComponent<BoxCollider2D>().size = highlight.GetComponent<BoxCollider2D>().size + new Vector2(.1f, .1f);
            placedObj.GetComponent<SpriteRenderer>().sprite = RemovePaddingToSprite(objToPlace.GetComponent<Image>().sprite, 1);
            placedObj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            placedObj.GetComponent<SpriteRenderer>().size = highlight.GetComponent<BoxCollider2D>().size + new Vector2(.1f, .1f);
            
            if(GetComponent<TilePlacer>().layer == "Ground"  || GetComponent<TilePlacer>().layer == "Underground")
            {
                placedObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            else
            {
                placedObj.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }

            //special properties for certain objects
            switch (objToPlace.name)
            {
                case "Item":
                    GameObject canvas = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                    canvas.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "item";
                    canvas.transform.parent = canvases.Find(layer);
                    canvas.transform.position = placedObj.transform.position;
                    canvas.name = "SpecialObjectCanvas";
                    placedObj.AddComponent<MEItemIDContainer>();
                    break;
                case "ChristmasDesk":
                case "DTAFSpecialDesk":
                case "ETSpecialDesk":
                    GameObject canvas1 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                    canvas1.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "desk";
                    canvas1.transform.parent = canvases.Find(layer);
                    canvas1.transform.position = placedObj.transform.position;
                    canvas1.name = "SpecialObjectCanvas";
                    placedObj.AddComponent<MEDeskListContainer>();
                    break;
                case "DTAFSign":
                    GameObject canvas2 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                    canvas2.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "whiteSign";
                    canvas2.transform.parent = canvases.Find(layer);
                    canvas2.transform.position = placedObj.transform.position;
                    canvas2.name = "SpecialObjectCanvas";
                    placedObj.AddComponent<MESignTextContainer>();
                    break;
                case "SSSign":
                    GameObject canvas3 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                    canvas3.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "blueSign";
                    canvas3.transform.parent = canvases.Find(layer);
                    canvas3.transform.position = placedObj.transform.position;
                    canvas3.name = "SpecialObjectCanvas";
                    placedObj.AddComponent<MESignTextContainer>();
                    break;
                case "DTAFPlaque":
                    GameObject canvas4 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                    canvas4.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "blueSign";
                    canvas4.transform.parent = canvases.Find(layer);
                    canvas4.transform.position = placedObj.transform.position;
                    canvas4.name = "SpecialObjectCanvas";
                    canvas4.GetComponent<RectTransform>().sizeDelta = new Vector2(1.6f, 3.2f);
                    canvas4.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 3.2f);
                    placedObj.AddComponent<MESignTextContainer>();
                    break;
            }
        }
    }
    public Sprite RemovePaddingToSprite(Sprite paddedSprite, int padding)
    {
        // Get the original padded texture and rect
        Texture2D paddedTexture = paddedSprite.texture;
        Rect paddedRect = paddedSprite.rect;

        int paddedWidth = (int)paddedRect.width;
        int paddedHeight = (int)paddedRect.height;

        // Calculate the size of the cropped (unpadded) area
        int croppedWidth = paddedWidth - padding * 2;
        int croppedHeight = paddedHeight - padding * 2;

        // Safety check
        if (croppedWidth <= 0 || croppedHeight <= 0)
        {
            Debug.LogError("Padding is too large for the sprite size.");
            return paddedSprite;
        }

        // Create a new texture for the cropped area
        Texture2D croppedTexture = new Texture2D(croppedWidth, croppedHeight, paddedTexture.format, false);
        croppedTexture.filterMode = paddedTexture.filterMode;
        croppedTexture.wrapMode = TextureWrapMode.Clamp;

        // Copy the central region (removing padding)
        Color[] pixels = paddedTexture.GetPixels(
            (int)paddedRect.x + padding,
            (int)paddedRect.y + padding,
            croppedWidth,
            croppedHeight
        );
        croppedTexture.SetPixels(0, 0, croppedWidth, croppedHeight, pixels);
        croppedTexture.Apply();

        // Create a new sprite from the cropped texture
        Sprite croppedSprite = Sprite.Create(
            croppedTexture,
            new Rect(0, 0, croppedWidth, croppedHeight),
            new Vector2(0.5f, 0.5f), // pivot
            paddedSprite.pixelsPerUnit
        );

        return croppedSprite;
    }
}
