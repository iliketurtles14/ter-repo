using UnityEngine;
using UnityEngine.UI;

public class TilePlacer : MonoBehaviour
{
    public Transform highlight;
    public MouseCollisionOnMap mcs;
    public Transform tiles;
    public bool canPlace;
    private void Update()
    {
        if(highlight.position != new Vector3(9999, 9999) && !mcs.isTouchingMenu && !mcs.isTouchingButton && Input.GetMouseButton(0) && GetComponent<TileSelect>().hasSelected)
        {
            Debug.Log("In if");

            //change this for when you have all layers
            foreach(Transform tile in tiles.Find("Ground"))
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
                Debug.Log("Placing Tile");
                PlaceTile("Ground", GetComponent<TileSelect>().selectedTile);
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
        placedTile.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 1.6f);
        placedTile.GetComponent<SpriteRenderer>().sprite = tileToPlace.GetComponent<Image>().sprite;
        placedTile.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        placedTile.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
        Instantiate(placedTile, tiles.Find(layer));
    }
}
