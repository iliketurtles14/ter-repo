using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogoMenu : MonoBehaviour
{
    private bool shouldStay = true;
    private bool hasStarted;
    public bool canStart = false;
    private string currentScreen = "reinc";
    public Transform titlePanel;
    public MMSoundController sc;
    public MMMusicController mc;
    public Transform mmc;
    private IEnumerator LogoWait()
    {
        for(int i = 0; i < 4; i++)
        {
            yield return new WaitForEndOfFrame();
            mc.canStartMusic = true;
            float time = 0f;
            while (shouldStay && time < 1.2f)
            {
                yield return null;
                time += Time.deltaTime;
            }
            ChangeScreen();
            shouldStay = true;
        }
    }
    private void Update()
    {
        if (!canStart)
        {
            return;
        }
        else if(canStart && !hasStarted)
        {
            StartCoroutine(LogoWait());
            hasStarted = true;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            shouldStay = false;
        }
    }
    private void ChangeScreen()
    {
        if(currentScreen == "reinc")
        {
            currentScreen = "turt";
            transform.Find("TextBlocker").localPosition = new Vector2(0, -250);
        }
        else if(currentScreen == "turt")
        {
            currentScreen = "based";
            transform.Find("TextBlocker").localPosition = new Vector2(0, -430);
        }
        else if(currentScreen == "based")
        {
            currentScreen = "text";
            transform.Find("TextBlocker").localPosition = new Vector2(0, -785);
        }
        else if(currentScreen == "text")
        {
            EndLogos();
        }
    }
    private void EndLogos()
    {
        sc.PlaySound("rumble");
        
        titlePanel.transform.Find("PlayButton").GetComponent<Button>().enabled = true;
        titlePanel.transform.Find("OptionsButton").GetComponent<Button>().enabled = true;
        titlePanel.transform.Find("MapEditorButton").GetComponent<Button>().enabled = true;

        if(titlePanel.transform.Find("VersionText").GetComponent<TextMeshProUGUI>().text.Contains("PLAY-TEST"))
        {
            OpenPlayTestMenu();
        }

        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    private void OpenPlayTestMenu()
    {
        mmc.Find("PlayTestPanel").gameObject.SetActive(true);
        mmc.Find("Black").GetComponent<Image>().enabled = true;
        foreach (Transform button in mmc.Find("TitlePanel"))
        {
            if (button.GetComponent<Button>() != null)
            {
                button.GetComponent<Button>().enabled = false;
                button.GetComponent<EventTrigger>().enabled = false;
            }
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
