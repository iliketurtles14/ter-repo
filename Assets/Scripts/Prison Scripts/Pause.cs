using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour //lol i finally changed this script to be better buttons (July 3, 2026)
{
    private GameObject black;
    private PauseController pc;
    public bool paused = false;
    public bool isQuitting = false;

    public void Start()
    {
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        black = RootObjectCache.GetRoot("MenuCanvas").transform.Find("Black").gameObject;

        ClosePauseMenu(false);
    }
    public void Update()
    {
        paused = pc.isPaused;
        
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            PSoundController.PlaySound("open");
            OpenPauseMenu();
            return;
        } 

        if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            PSoundController.PlaySound("close");
            ClosePauseMenu(false);
            return;
        }
    }
    public void ClosePauseMenu(bool goingSomewhereElse)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        if (!goingSomewhereElse)
        {
            black.GetComponent<Image>().enabled = false;

            pc.Unpause();
        }
    }
    public void OpenPauseMenu()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        black.GetComponent<Image>().enabled = true;
        
        pc.Pause(true);
    }
}
