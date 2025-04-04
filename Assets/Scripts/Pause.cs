using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private GameObject lastTouchedButton;
    public MouseCollisionOnItems mcs;
    public Sprite buttonNormalSprite;
    public Sprite buttonPressedSprite;
    public GameObject player;
    public GameObject aStar;
    public GameObject timeObject;
    public GameObject black;
    public GameObject inventoryCanvas;
    public bool paused = false;
    public bool isQuitting = false;

    public void Start()
    {
        //disable panel
        foreach (Transform child in transform)
        {
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if (child.GetComponent<TextMeshProUGUI>() != null)
            {
                child.GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
        black.GetComponent<Image>().enabled = false;
        GetComponent<Image>().enabled = false;
    }
    public void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            paused = true;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<BoxCollider2D>() != null)
                {
                    child.GetComponent<BoxCollider2D>().enabled = true;
                }
                if (child.GetComponent<Image>() != null)
                {
                    child.GetComponent<Image>().enabled = true;
                }
                if (child.GetComponent<TextMeshProUGUI>() != null)
                {
                    child.GetComponent<TextMeshProUGUI>().enabled = true;
                }
            }
            black.GetComponent<Image>().enabled = true;
            GetComponent<Image>().enabled = true;
            foreach (Transform child in aStar.transform)
            {
                child.GetComponent<AILerp>().speed = 0;
                child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                child.GetComponent<NPCAnimation>().enabled = false;
            }
            timeObject.GetComponent<Routine>().enabled = false;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            player.GetComponent<PlayerCtrl>().enabled = false;
            player.GetComponent<PlayerAnimation>().enabled = false;
            mcs.DisableTag("Bars");
            mcs.DisableTag("Fence");
            mcs.DisableTag("ElectricFence");
            mcs.DisableTag("Digable");
            mcs.DisableTag("Wall");
            mcs.DisableTag("Item");
            mcs.DisableTag("Desk");
            inventoryCanvas.transform.Find("PlayerIDButton").GetComponent<BoxCollider2D>().enabled = false;
            return;
        } 

        if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            paused = false;
            
            foreach (Transform child in transform)
            {
                if (child.GetComponent<BoxCollider2D>() != null)
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }
                if (child.GetComponent<Image>() != null)
                {
                    child.GetComponent<Image>().enabled = false;
                }
                if (child.GetComponent<TextMeshProUGUI>() != null)
                {
                    child.GetComponent<TextMeshProUGUI>().enabled = false;
                }
            }
            black.GetComponent<Image>().enabled = false;
            GetComponent<Image>().enabled = false;
            foreach (Transform child in aStar.transform)
            {
                child.GetComponent<AILerp>().speed = 10;
                child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                child.GetComponent<NPCAnimation>().enabled = true;
            }
            timeObject.GetComponent<Routine>().enabled = true;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            player.GetComponent<PlayerCtrl>().enabled = true;
            player.GetComponent<PlayerAnimation>().enabled = true;
            mcs.EnableTag("Bars");
            mcs.EnableTag("Fence");
            mcs.EnableTag("ElectricFence");
            mcs.EnableTag("Digable");
            mcs.EnableTag("Wall");
            mcs.EnableTag("Item");
            mcs.EnableTag("Desk");
            inventoryCanvas.transform.Find("PlayerIDButton").GetComponent<BoxCollider2D>().enabled = true;
            return;
        }

        if (mcs.isTouchingButton && paused)
        {
            lastTouchedButton = mcs.touchedButton;
            mcs.touchedButton.GetComponent<Image>().sprite = buttonPressedSprite;

            if (Input.GetMouseButtonDown(0))
            {
                if(lastTouchedButton.name == "ContinueButton")
                {
                    foreach(Transform child in transform)
                    {
                        if(child.GetComponent<BoxCollider2D>() != null)
                        {
                            child.GetComponent<BoxCollider2D>().enabled = false;
                        }
                        if(child.GetComponent<Image>() != null)
                        {
                            child.GetComponent<Image>().enabled = false;
                        }
                        if(child.GetComponent<TextMeshProUGUI>() != null)
                        {
                            child.GetComponent<TextMeshProUGUI>().enabled = false;
                        }
                    }
                    black.GetComponent<Image>().enabled = false;
                    GetComponent<Image>().enabled = false;
                    foreach(Transform child in aStar.transform)
                    {
                        child.GetComponent<AILerp>().speed = 10;
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                        child.GetComponent<NPCAnimation>().enabled = true;
                    }
                    timeObject.GetComponent<Routine>().enabled = true;
                    player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    player.GetComponent<PlayerCtrl>().enabled = true;
                    player.GetComponent<PlayerAnimation>().enabled = true;
                    mcs.EnableTag("Bars");
                    mcs.EnableTag("Fence");
                    mcs.EnableTag("ElectricFence");
                    mcs.EnableTag("Digable");
                    mcs.EnableTag("Wall");
                    mcs.EnableTag("Item");
                    mcs.EnableTag("Desk");
                    inventoryCanvas.transform.Find("PlayerIDButton").GetComponent<BoxCollider2D>().enabled = true;
                    paused = false;
                }
                else if(lastTouchedButton.name == "QuitButton" && !isQuitting)
                {
                    isQuitting = true;
                    AsyncOperation loadOperation = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
                    loadOperation.completed += (AsyncOperation op) =>
                    {
                        SceneManager.UnloadSceneAsync(1);
                    };
                }
            }
        }
        else if(!mcs.isTouchingButton && lastTouchedButton != null)
        {
            lastTouchedButton.GetComponent<Image>().sprite = buttonNormalSprite;
        }
    }
}
