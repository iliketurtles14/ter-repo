using UnityEngine;
using UnityEngine.UI;

public class PatchNotes : MonoBehaviour
{
    public MouseCollisionOnButtons mcs;
    public GameObject titlePanel;
    public GameObject black;
    public Sprite buttonNormalSprite;
    public Sprite buttonPressedSprite;
    private GameObject lastTouchedButton;
    public void Update()
    {
        if(mcs.isTouchingButton && mcs.touchedButton.name == "BackButton")
        {
            mcs.touchedButton.GetComponent<Image>().sprite = buttonPressedSprite;
            lastTouchedButton = mcs.touchedButton;
            if (Input.GetMouseButtonDown(0))
            {
                foreach (Transform child in titlePanel.transform)
                {
                    if (child.GetComponent<BoxCollider2D>() != null)
                    {
                        child.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
                black.GetComponent<Image>().enabled = false;
                gameObject.SetActive(false);
            }
        }
        else if(lastTouchedButton != null)
        {
            lastTouchedButton.GetComponent<Image>().sprite = buttonNormalSprite;
        }
    }
}
