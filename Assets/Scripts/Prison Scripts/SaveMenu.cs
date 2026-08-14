using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    private Saving savingScript;
    private bool canSave;
    private bool menuIsOpen;
    private Transform mc;
    private PauseController pc;
    private MouseCollisionOnItems mcs;
    private string currentType;
    private bool isQuitting;
    private void Start()
    {
        savingScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Saving>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        Close(false);
    }
    private void Update()
    {
        canSave = savingScript.canSave;

        if (!mcs.isTouchingIDPanel && !mcs.isTouchingButton && !mcs.isTouchingInvSlot && !mcs.isTouchingExtra && !mcs.isTouchingIDSlot && Input.GetMouseButtonDown(0) && menuIsOpen)
        {
            PSoundController.PlaySound("close");
            Close(false);
        }
    }
    public void Open()
    {
        if(DataSender.instance.currentSave != -1)
        {
            transform.Find("SaveButton").gameObject.SetActive(true);
            transform.Find("LoadButton").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("NoSaveText").gameObject.SetActive(true);
        }
        transform.Find("FavorButton").gameObject.SetActive(true);
        transform.Find("IDButton").gameObject.SetActive(true);
        transform.Find("TitleText").gameObject.SetActive(true);
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        menuIsOpen = true;
    }
    public void Close(bool goingToOther)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        menuIsOpen = false;
        if (!goingToOther)
        {
            mc.Find("Black").GetComponent<Image>().enabled = false;
            pc.Unpause();
        }
    }
    public void AskIfSure(string type)//save, load
    {
        transform.Find("SureText").GetComponent<TextMeshProUGUI>().text = "Are you sure you want to " + type + "?";
        transform.Find("SureText").gameObject.SetActive(true);
        transform.Find("SaveButton").gameObject.SetActive(false);
        transform.Find("LoadButton").gameObject.SetActive(false);
        transform.Find("YesButton").gameObject.SetActive(true);
        transform.Find("NoButton").gameObject.SetActive(true);
        currentType = type;
    }
    public void Yes()
    {
        if(currentType == "save")
        {
            savingScript.Save();
            No();
        }
        else if(currentType == "load" && !isQuitting)
        {
            //fade out
            Addressables.LoadSceneAsync("Prison");
            isQuitting = true;
        }
    }
    public void No()
    {
        transform.Find("SureText").gameObject.SetActive(false);
        transform.Find("SaveButton").gameObject.SetActive(true);
        transform.Find("LoadButton").gameObject.SetActive(true);
        transform.Find("YesButton").gameObject.SetActive(false);
        transform.Find("NoButton").gameObject.SetActive(false);
    }
}
