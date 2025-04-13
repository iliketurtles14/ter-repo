using UnityEngine;

public class OutfitController : MonoBehaviour
{
    public int currentOutfitID;
    private GameObject currentIDPanel;
    public GameObject mc;
    public void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            currentIDPanel = mc.transform.Find("PlayerMenuPanel").gameObject;
            if (currentIDPanel.GetComponent<PlayerIDInv>().idInv[0] != null)
            {
                currentOutfitID = currentIDPanel.GetComponent<PlayerIDInv>().idInv[0].itemData.id;
            }
            else
            {
                currentOutfitID = -1;
            }

            switch (currentOutfitID)
            {
                case 29:
                case 30:
                case 31:
                case 32:
                    GetComponent<PlayerAnimation>().outfitDirSprites = DataSender.instance.
            }
        }
        else if(gameObject.CompareTag("Inmate") || gameObject.CompareTag("Guard"))
        {
            
        }
    }
}
