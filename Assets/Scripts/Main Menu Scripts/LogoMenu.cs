using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogoMenu : MonoBehaviour
{
    private bool shouldStay = true;
    private bool hasStarted;
    public bool canStart = false;
    private string currentScreen = "blank"; //text, logos, blank
    public Transform titlePanel;
    public MMSoundController sc;
    public MMMusicController mc;
    private IEnumerator LogoWait()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForEndOfFrame();
            float time = 0f;
            while (shouldStay && time < 2.4f)
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
        if(currentScreen == "blank")
        {
            transform.Find("IntroText").gameObject.SetActive(true);
            mc.canStartMusic = true;
            currentScreen = "text";
        }
        else if(currentScreen == "text")
        {
            transform.Find("IntroText").gameObject.SetActive(false);
            transform.Find("Logos").gameObject.SetActive(true);
            currentScreen = "logos";
        }
        else if(currentScreen == "logos")
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

        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
