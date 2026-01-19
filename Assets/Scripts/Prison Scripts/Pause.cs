using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private GameObject lastTouchedButton;
    private MouseCollisionOnItems mcs;
    public Sprite buttonNormalSprite;
    public Sprite buttonPressedSprite;
    private GameObject black;
    private PauseController pc;
    public bool paused = false;
    public bool isQuitting = false;

    public void Start()
    {
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        black = RootObjectCache.GetRoot("MenuCanvas").transform.Find("Black").gameObject;

        ClosePauseMenu();
    }
    public void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            OpenPauseMenu();
            return;
        } 

        if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            ClosePauseMenu();
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
                    ClosePauseMenu();
                }
                else if(lastTouchedButton.name == "QuitButton" && !isQuitting)
                {
                    isQuitting = true;
                    Addressables.LoadSceneAsync("Main Menu");
                }
            }
        }
        else if(!mcs.isTouchingButton && lastTouchedButton != null)
        {
            lastTouchedButton.GetComponent<Image>().sprite = buttonNormalSprite;
        }
    }
    private void ClosePauseMenu()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        black.GetComponent<Image>().enabled = false;
        
        pc.Unpause();
        
        paused = false;
    }
    private void OpenPauseMenu()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        black.GetComponent<Image>().enabled = true;
        
        pc.Pause(true);

        paused = true;
    }
}
