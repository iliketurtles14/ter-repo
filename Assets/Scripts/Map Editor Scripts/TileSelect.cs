using UnityEngine;
using UnityEngine.UI;

public class TileSelect : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public GameObject selectedTile;
    public Transform tilesPanel;
    public bool hasSelected;
    private void Update()
    {
        if(mcs.isTouchingButton && mcs.touchedButton.name.StartsWith("tile") && Input.GetMouseButtonDown(0))
        {
            SelectTile(mcs.touchedButton);
        }
    }
    private void SelectTile(GameObject tile)
    {
        hasSelected = true;
        selectedTile = tile;
        tilesPanel.Find("Outline").GetComponent<Image>().enabled = true;
        tilesPanel.Find("Outline").position = tile.transform.position;
    }
}
