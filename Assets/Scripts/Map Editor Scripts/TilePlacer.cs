using UnityEngine;
using UnityEngine.UI;

public class TilePlacer : MonoBehaviour
{
    public Transform highlight;
    public MouseCollisionOnMap mcs;
    public Transform tiles;
    public bool canPlace;
    public LayerController layerControllerScript;
    public bool inDeleteMode = false;
    public string layer;
    private void Start()
    {
        foreach(Transform layer in tiles)
        {
            GameObject empty = new GameObject();
            empty.name = "empty";
            empty.transform.position = new Vector3(9999, 9999);
            empty.transform.SetParent(layer);
            empty.AddComponent<BoxCollider2D>();
        }
    }
    private void Update()
    {
        string currentPanel = GetComponent<PanelSelect>().currentPanel;

        switch (layerControllerScript.currentLayer)
        {
            case 0:
                layer = "Underground";
                break;
            case 1:
                layer = "Ground";
                break;
            case 2:
                layer = "Vent";
                break;
            case 3:
                layer = "Roof";
                break;
            case 4:
                layer = "Zones";
                break;
        }

        if (!inDeleteMode && highlight.position != new Vector3(9999, 9999) && !mcs.isTouchingMenu && !mcs.isTouchingButton && Input.GetMouseButton(0) && GetComponent<TileSelect>().hasSelected && layer != "Zones")
        {
            foreach(Transform tile in tiles.Find(layer))
            {
                if(tile.position == highlight.position)
                {
                    canPlace = false;
                    break;
                }
                else
                {
                    canPlace = true;
                }
            }

            if (canPlace)
            {
                PlaceTile(layer, GetComponent<TileSelect>().selectedTile);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !inDeleteMode && (currentPanel == "TilesPanel" || currentPanel == "ObjectsPanel"))
        {
            inDeleteMode = true;

            highlight.gameObject.SetActive(true);
            highlight.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
            highlight.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 0, 0, 150f / 255f);
        }
        else if((Input.GetKeyDown(KeyCode.Space) && inDeleteMode) || (currentPanel != "TilesPanel" && currentPanel != "ObjectsPanel"))
        {
            inDeleteMode = false;

            if (!GetComponent<TileSelect>().hasSelected)
            {
                highlight.gameObject.SetActive(false);
            }

            highlight.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 220f / 255f, 0, 150f / 255f);
        }

        if(highlight.position != new Vector3(9999, 9999) && !mcs.isTouchingMenu && !mcs.isTouchingButton && Input.GetMouseButton(0) && inDeleteMode)
        {
            foreach(Transform tile in tiles.Find(layer))
            {
                if(tile.position == highlight.position)
                {
                    Destroy(tile.gameObject);
                    break;
                }
            }
        }
    }
    private void PlaceTile(string layer, GameObject tileToPlace)
    {
        GameObject placedTile = new GameObject();
        placedTile.AddComponent<SpriteRenderer>();
        placedTile.AddComponent<BoxCollider2D>();
        placedTile.name = tileToPlace.name;
        placedTile.transform.position = highlight.position;
        placedTile.transform.SetParent(tiles.Find(layer));
        placedTile.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 1.6f);
        placedTile.GetComponent<SpriteRenderer>().sprite = tileToPlace.GetComponent<Image>().sprite;
        placedTile.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        placedTile.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);

        if(layer == "Ground" || layer == "Underground")
        {
            placedTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            placedTile.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }
}
