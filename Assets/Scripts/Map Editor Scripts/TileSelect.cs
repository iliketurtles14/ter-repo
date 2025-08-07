using UnityEngine;
using UnityEngine.UI;

public class TileSelect : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public GameObject selectedTile;
    public Transform tilesPanel;
    public bool hasSelected;
    public GameObject highlight;
    private void Update()
    {
        string currentPanel = GetComponent<PanelSelect>().currentPanel;

        if (currentPanel == "TilesPanel" && mcs.isTouchingButton && mcs.touchedButton.name.StartsWith("tile") && Input.GetMouseButtonDown(0))
        {
            SelectTile(mcs.touchedButton);
        }

        if(currentPanel != "TilesPanel" && hasSelected)
        {
            DeselectTile();
        }
    }
    private void SelectTile(GameObject tile)
    {
        hasSelected = true;
        selectedTile = tile;
        tilesPanel.Find("Outline").GetComponent<Image>().enabled = true;
        tilesPanel.Find("Outline").position = tile.transform.position;

        highlight.SetActive(true);
        highlight.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
    }
    private void DeselectTile()
    {
        hasSelected = false;
        selectedTile = null;
        tilesPanel.Find("Outline").GetComponent<Image>().enabled = false;

        highlight.SetActive(false);
    }
}
